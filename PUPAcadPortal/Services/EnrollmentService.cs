using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUPAcadPortal.Services
{
    public class EnrollmentService
    {
        // ─────────────────────────────────────────────────────────────────
        // 1. PROFILE FETCHER
        // ─────────────────────────────────────────────────────────────────
        public async Task<Student> GetStudentProfileAsync(int studentId)
        {
            using (var context = new AppDbContext())
            {
                var student = await context.Students
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (student == null) throw new Exception("Critical Error: Academic profile could not be found.");

                return student;
            }
        }

        public async Task<bool> IsStudentIskolar(int studentId)
        {
            using (var context = new AppDbContext())
            {
                if (await context.StudentDiscounts.AnyAsync(sd => sd.StudentId == studentId)) return true;
                else return false;
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // 2. PREREQUISITE ENGINE & DATA FETCHING
        // ─────────────────────────────────────────────────────────────────
        public async Task<List<EnrollmentData>> GetAvailableSubjectsAsync(int studentId, string academicPeriodId, string program, int yearLevel, int semesterIndex)
        {
            using (var context = new AppDbContext())
            {
                // 1. Get already enrolled or pending payment subjects as a Dictionary
                var processedStatuses = new List<string> { "Enrolled", "Pending Payment" };

                var existingEnrollments = await context.EnrollmentSubjects
                    .Where(es => es.Enrollment.StudentId == studentId
                              && es.Enrollment.AcademicPeriodId == academicPeriodId
                              && (es.SubjectStatus == "Enrolled" || es.SubjectStatus == "Pending Payment")) // <-- THE FIX
                    .ToDictionaryAsync(es => es.SubjectOfferingId, es => es.SubjectStatus);

                // 2. Get passed subjects for prerequisites
                var passedSubjectIds = await context.FinalCourseGrades
                    .Include(f => f.EnrollmentSubj)
                    .Where(f => f.EnrollmentSubj.Enrollment.StudentId == studentId
                             && f.FinalRating <= 3.00m && f.FinalRating >= 1.00m)
                    .Select(f => f.EnrollmentSubj.SubjectOffering.SubjectId)
                    .ToListAsync();

                // 3. MySQL Subquery Fix (Do NOT use ToListAsync here)
                var requiredSubjectIdsQuery = context.Curricula
                    .Where(c => c.Program == program && c.YearLevel == yearLevel && c.SemesterIndex == semesterIndex)
                    .Select(c => c.SubjectId);

                // 4. Fetch the Offerings using the "Active" status fix
                var offerings = await context.SubjectOfferings
                    .Include(so => so.Subject)
                        .ThenInclude(s => s.SubjectPrerequisiteSubjects)
                            .ThenInclude(pr => pr.RequiredSubject)
                    .Include(so => so.RoomSchedules)
                    .Where(so => so.AcademicPeriodId == academicPeriodId
                              && so.Status == "Active"
                              && requiredSubjectIdsQuery.Contains(so.SubjectId))
                    .ToListAsync();

                // 5. Map to UI
                return offerings.Select(so =>
                {
                    // Check if the subject is in our Dictionary
                    bool isProcessed = existingEnrollments.ContainsKey(so.SubjectOfferingId);
                    string dbStatus = isProcessed ? existingEnrollments[so.SubjectOfferingId] : null;

                    var missingPrereqs = so.Subject.SubjectPrerequisiteSubjects
                        .Where(pr => !passedSubjectIds.Contains(pr.RequiredSubjectId))
                        .Select(pr => pr.RequiredSubject != null
                            ? $"{pr.RequiredSubject.SubjectCode} ({pr.RequiredSubject.SubjectName})"
                            : "Unknown Subject")
                        .ToList();

                    bool isEligible = !missingPrereqs.Any();

                    // DYNAMIC STATUS LABELING
                    string rowStatus;
                    if (isProcessed)
                    {
                        rowStatus = dbStatus; // Directly passes "Enrolled" or "Pending Payment" to the grid
                    }
                    else
                    {
                        rowStatus = isEligible ? "Pending" : "Locked";
                    }

                    return new EnrollmentData
                    {
                        SubjectOfferingID = so.SubjectOfferingId,
                        IsSelected = isProcessed,
                        Code = so.Subject.SubjectCode,
                        CourseTitle = so.Subject.SubjectName,
                        Units = so.Subject.Units,
                        Schedule = FormatSchedules(so.RoomSchedules),
                        Status = rowStatus,
                        IsEligible = isEligible,
                        PrerequisiteMessage = isEligible ? "" : $"Missing Prerequisite: {string.Join(", ", missingPrereqs)}"
                    };
                }).ToList();
            }
        }

        private string FormatSchedules(IEnumerable<RoomSchedule> schedules)
        {
            if (schedules == null || !schedules.Any()) return "TBA";
            var grouped = schedules.GroupBy(s => s.DayOfWeek);
            var lines = grouped.Select(g =>
            {
                // Added SessionType for Lecture/Lab differentiation
                var times = g.Select(s => $"{DateTime.Today.Add(s.StartTime):h:mm tt} - {DateTime.Today.Add(s.EndTime):h:mm tt} ({s.SessionType})");
                return $"{g.Key} {string.Join(", ", times)}";
            });
            return string.Join(Environment.NewLine, lines);
        }

        // ─────────────────────────────────────────────────────────────────
        // 3. THE SAVE AND ASSESS TRANSACTION
        // ─────────────────────────────────────────────────────────────────
        public async Task<(bool Success, string Message)> ProcessSaveAndAssessAsync(int studentId, string academicPeriodId, List<EnrollmentData> selectedSubjects)
        {
            using (var context = new AppDbContext())
            {
                var strategy = context.Database.CreateExecutionStrategy();

                return await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // 1. DO THE FINANCIAL MATH FIRST
                            int totalUnits = selectedSubjects.Sum(s => s.Units);
                            decimal costPerUnit = 200.00m;
                            decimal tuitionFee = totalUnits * costPerUnit;
                            decimal miscAndLabFees = 1500.00m;
                            decimal totalAssessment = tuitionFee + miscAndLabFees;

                            // 2. CHECK FOR FREE TUITION DISCOUNT BEFORE SAVING ANYTHING
                            var activeDiscount = await context.StudentDiscounts
                                .FirstOrDefaultAsync(d => d.StudentId == studentId && d.IsActive == true && d.DiscountName.Contains("RA 10931"));

                            // 3. DETERMINE THE EXACT DATABASE STATUSES
                            string parentStatus;
                            string childSubjectStatus;

                            if (activeDiscount != null)
                            {
                                parentStatus = "Officially Enrolled";
                                childSubjectStatus = "Enrolled";
                                totalAssessment = 0;
                            }
                            else
                            {
                                parentStatus = "Assessed / Pending Payment";
                                childSubjectStatus = "Pending Payment";
                            }

                            // 4. NOW BUILD THE RECORDS WITH THE CORRECT STATUS
                            string newEnrollmentId = $"ENR-{academicPeriodId}-{studentId}";

                            var enrollment = new Enrollment
                            {
                                EnrollmentId = newEnrollmentId,
                                StudentId = studentId,
                                AcademicPeriodId = academicPeriodId,
                                Status = parentStatus,
                                EnrollmentDate = DateTime.Now
                            };
                            context.Enrollments.Add(enrollment);

                            foreach (var subject in selectedSubjects)
                            {
                                context.EnrollmentSubjects.Add(new Models.EnrollmentSubject
                                {
                                    EnrollmentSubjId = $"{newEnrollmentId}-{subject.Code.Replace(" ", "")}",
                                    EnrollmentId = newEnrollmentId,
                                    SubjectOfferingId = subject.SubjectOfferingID,
                                    SubjectStatus = childSubjectStatus
                                });
                            }

                            // 5. BUILD THE LEDGER
                            var account = new StudentAccount
                            {
                                StudentId = studentId,
                                AcademicPeriodId = academicPeriodId,
                                TotalAssessment = totalAssessment,
                                CreatedAt = DateTime.Now
                            };
                            context.StudentAccounts.Add(account);
                            await context.SaveChangesAsync();

                            context.FeeBreakdowns.Add(new FeeBreakdown { AccountId = account.AccountId, FeeName = $"Tuition ({totalUnits} Units)", Amount = tuitionFee });
                            context.FeeBreakdowns.Add(new FeeBreakdown { AccountId = account.AccountId, FeeName = "Miscellaneous & Lab Fees", Amount = miscAndLabFees });

                            if (activeDiscount != null)
                            {
                                context.FeeBreakdowns.Add(new FeeBreakdown
                                {
                                    AccountId = account.AccountId,
                                    FeeName = activeDiscount.DiscountName,
                                    Amount = -(tuitionFee + miscAndLabFees)
                                });
                            }

                            // 6. COMMIT EVERYTHING
                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            return (true, "Enrollment successfully saved and assessed!");
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            context.ChangeTracker.Clear();
                            return (false, $"Transaction failed. Error: {ex.Message}");
                        }
                    }
                });
            }
        }
    }
}