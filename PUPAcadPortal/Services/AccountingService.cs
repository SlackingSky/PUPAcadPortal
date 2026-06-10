using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public class AccountingService
    {
        public async Task<List<AccountingData>> GetStudentAccountingRecordsAsync(string academicPeriodId)
        {
            using (var context = new AppDbContext())
            {
                var records = await context.StudentAccounts
                    .Where(sa => sa.AcademicPeriodId == academicPeriodId &&
                                 context.Enrollments.Any(e => e.StudentId == sa.StudentId &&
                                                              e.AcademicPeriodId == academicPeriodId &&
                                                              (e.Status == "Officially Enrolled" ||
                                                               e.Status == "Pending Payment")))
                    .Select(sa => new AccountingData
                    {
                        StudentId = sa.StudentId,
                        StudentNo = sa.Student.StudentNumber,
                        Program = sa.Student.Program,
                        FullName = sa.Student.User.LastName + ", " + sa.Student.User.FirstName,
                        TotalAmount = sa.TotalAssessment,
                        PaidAmount = context.PaymentHistories
                                            .Where(p => p.AccountId == sa.AccountId && p.Status == "Paid")
                                            .Sum(p => (decimal?)p.Amount) ?? 0m
                    })
                    .ToListAsync();

                return records;
            }
        }

        public async Task<AccountDetailsData> GetAccountDetailsAsync(int studentId, string academicPeriodId)
        {
            using (var context = new AppDbContext())
            {
                var account = await context.StudentAccounts
                    .Include(sa => sa.FeeBreakdowns)
                    .Include(sa => sa.PaymentHistories)
                    .FirstOrDefaultAsync(sa => sa.StudentId == studentId && sa.AcademicPeriodId == academicPeriodId);

                if (account == null) return null;

                var details = new AccountDetailsData
                {
                    AccountId = account.AccountId,
                    Fees = account.FeeBreakdowns.Select(f => new FeeItemData
                    {
                        FeeName = f.FeeName,
                        Amount = f.Amount
                    }).ToList(),
                    Payments = account.PaymentHistories.OrderByDescending(p => p.CreatedAt).Select(p => new PaymentItemData
                    {
                        ReferenceID = p.ReferenceId ?? "N/A",
                        PaidDate = p.PaidDate,
                        Amount = p.Amount,
                        Status = p.Status
                    }).ToList()
                };

                return details;
            }
        }

        public async Task<(bool Success, string Message)> RecordPaymentAsync(int accountId, decimal amount, string referenceId, int adminUserId)
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
                            var account = await context.StudentAccounts
                                .FirstOrDefaultAsync(a => a.AccountId == accountId);

                            if (account == null) return (false, "Account not found.");

                            var newPayment = new PaymentHistory
                            {
                                AccountId = accountId,
                                Amount = amount,
                                ReferenceId = referenceId,
                                Description = "Counter Payment",
                                DueDate = DateTime.Now,
                                PaidDate = DateTime.Now,
                                Status = "Paid",
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };

                            context.PaymentHistories.Add(newPayment);

                            decimal existingPayments = await context.PaymentHistories
                                .Where(p => p.AccountId == accountId && p.Status == "Paid")
                                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

                            decimal newTotalPaid = existingPayments + amount;
                            decimal balance = account.TotalAssessment - newTotalPaid;

                            if (balance <= 0)
                            {
                                var enrollment = await context.Enrollments
                                    .Include(e => e.EnrollmentSubjects)
                                    .FirstOrDefaultAsync(e => e.StudentId == account.StudentId
                                                           && e.AcademicPeriodId == account.AcademicPeriodId);

                                if (enrollment != null && enrollment.Status == "Pending Payment")
                                {
                                    enrollment.Status = "Officially Enrolled";

                                    if (enrollment.EnrollmentSubjects != null)
                                    {
                                        foreach (var subject in enrollment.EnrollmentSubjects)
                                        {
                                            if (subject.SubjectStatus == "Pending Payment")
                                            {
                                                subject.SubjectStatus = "Officially Enrolled";
                                            }
                                        }
                                    }
                                }
                            }

                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            return (true, "Payment recorded successfully.");
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            context.ChangeTracker.Clear();
                            return (false, $"Payment failed: {ex.Message}");
                        }
                    }
                });
            }
        }
    }
}