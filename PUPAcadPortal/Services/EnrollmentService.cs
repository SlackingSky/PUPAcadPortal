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
        public async Task<bool> IsStudentEnrolled(int studentID, string activePeriod)
        {
            using (var context = new AppDbContext())
            {
                if (await context.Enrollments.AllAsync(e => e.StudentId == studentID && e.AcademicPeriodId == activePeriod)) return true;
                else return false;
            }
        }

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

        public async Task<List<EnrollmentData>> GetAvailableSubjectsAsync(int studentId, string academicPeriodId, string program, int yearLevel, int semesterIndex)
        {
            using (var context = new AppDbContext())
            {
                var student = await context.Students.FindAsync(studentId);
                if (student == null) throw new Exception("Student not found.");

                var existingEnrollments = await context.EnrollmentSubjects
                    .Where(es => es.Enrollment.StudentId == studentId
                              && es.Enrollment.AcademicPeriodId == academicPeriodId
                              && (es.SubjectStatus == "Officially Enrolled" || es.SubjectStatus == "Pending Payment"))
                    .ToDictionaryAsync(es => es.SubjectOfferingId, es => es.SubjectStatus);

                var passedSubjectIdsQuery = context.FinalCourseGrades
                    .Where(f => f.EnrollmentSubj.Enrollment.StudentId == studentId
                                && f.FinalRating <= 3.00m && f.FinalRating >= 1.00m)
                    .Select(f => f.EnrollmentSubj.SubjectOffering.SubjectId);

                var allowedSubjectIdsQuery = context.Curricula
                    .Where(c => c.Program == program
                        && c.RevisionYear == student.CurriculumYear
                        && (c.YearLevel < yearLevel || (c.YearLevel == yearLevel && c.SemesterIndex <= semesterIndex)))
                    .Select(c => c.SubjectId)
                    .Distinct();

                var offerings = await context.SubjectOfferings
                    .Include(so => so.Subject)
                        .ThenInclude(s => s.SubjectPrerequisiteSubjects)
                            .ThenInclude(pr => pr.RequiredSubject)
                    .Include(so => so.RoomSchedules)
                    .Where(so => so.AcademicPeriodId == academicPeriodId
                        && so.Status == "Active"
                        && allowedSubjectIdsQuery.Contains(so.SubjectId)
                        && !passedSubjectIdsQuery.Contains(so.SubjectId))
                    .ToListAsync();

                var passedSubjectIdsList = await passedSubjectIdsQuery.ToListAsync();

                var curriculumMap = await context.Curricula
                    .Where(c => c.Program == program && c.RevisionYear == student.CurriculumYear)
                    .Select(c => new { c.SubjectId, c.YearLevel, c.SemesterIndex })
                    .ToListAsync();

                return offerings.Select(so =>
                {
                    bool isProcessed = existingEnrollments.ContainsKey(so.SubjectOfferingId);
                    string dbStatus = isProcessed ? existingEnrollments[so.SubjectOfferingId] : null;

                    var currInfo = curriculumMap.FirstOrDefault(c => c.SubjectId == so.SubjectId);
                    bool isBackSubject = currInfo != null &&
                                         (currInfo.YearLevel < yearLevel ||
                                         (currInfo.YearLevel == yearLevel && currInfo.SemesterIndex < semesterIndex));

                    var missingPrereqs = so.Subject.SubjectPrerequisiteSubjects
                        .Where(pr => !passedSubjectIdsList.Contains(pr.RequiredSubjectId))
                        .Select(pr => pr.RequiredSubject != null ? $"{pr.RequiredSubject.SubjectCode}" : "Unknown Subject")
                        .ToList();

                    bool isEligible = !missingPrereqs.Any();
                    string rowStatus = isProcessed ? dbStatus : (isEligible ? "Pending" : "Locked");

                    string displayTitle = so.Subject.SubjectName;
                    if (isBackSubject && !isProcessed)
                    {
                        displayTitle = $"[BACK SUBJECT] {displayTitle}";
                    }

                    return new EnrollmentData
                    {
                        SubjectOfferingID = so.SubjectOfferingId,
                        IsSelected = isProcessed,
                        Code = so.Subject.SubjectCode,
                        CourseTitle = displayTitle,
                        Units = so.Subject.Units,
                        Schedule = FormatSchedules(so.RoomSchedules),
                        Status = rowStatus,
                        IsEligible = isEligible,
                        PrerequisiteMessage = isEligible ? "" : $"Missing Prerequisite: {string.Join(", ", missingPrereqs)}"
                    };
                }).OrderBy(s => s.Status == "Locked").ThenBy(s => s.CourseTitle).ToList();
            }
        }

        private string FormatSchedules(IEnumerable<RoomSchedule> schedules)
        {
            if (schedules == null || !schedules.Any()) return "TBA";
            var grouped = schedules.GroupBy(s => s.DayOfWeek);
            var lines = grouped.Select(g =>
            {
                var times = g.Select(s => $"{DateTime.Today.Add(s.StartTime):h:mm tt} - {DateTime.Today.Add(s.EndTime):h:mm tt} ({s.SessionType})");
                return $"{g.Key} {string.Join(", ", times)}";
            });
            return string.Join(Environment.NewLine, lines);
        }
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
                            int totalUnits = selectedSubjects.Sum(s => s.Units);
                            decimal costPerUnit = 200.00m;
                            decimal tuitionFee = totalUnits * costPerUnit;
                            decimal miscAndLabFees = 1500.00m;
                            decimal totalAssessment = tuitionFee + miscAndLabFees;

                            var activeDiscount = await context.StudentDiscounts
                                .FirstOrDefaultAsync(d => d.StudentId == studentId && d.IsActive == true && d.DiscountName.Contains("RA 10931"));

                            string parentStatus;
                            string childSubjectStatus;

                            if (activeDiscount != null)
                            {
                                parentStatus = "Officially Enrolled";
                                childSubjectStatus = "Officially Enrolled";
                                totalAssessment = 0;
                            }
                            else
                            {
                                parentStatus = "Pending Payment";
                                childSubjectStatus = "Pending Payment";
                            }

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