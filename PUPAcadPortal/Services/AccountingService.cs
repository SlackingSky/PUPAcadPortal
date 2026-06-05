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

        public async Task<bool> RecordPaymentAsync(int accountId, decimal amount, string referenceId, int adminUserId)
        {
            using (var context = new AppDbContext())
            {
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
                await context.SaveChangesAsync();

                return true;
            }
        }
    }
}