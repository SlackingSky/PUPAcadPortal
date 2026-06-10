using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Data
{
    public class AccountingData
    {
        public int StudentId { get; set; }
        public string StudentNo { get; set; }
        public string FullName { get; set; }
        public string Program { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }

        public decimal UnpaidAmount => TotalAmount - PaidAmount;
        public string Status => UnpaidAmount <= 0 ? "Fully Paid" : "Pending Payment";
    }
}