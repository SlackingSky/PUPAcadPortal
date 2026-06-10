using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Data
{
    public class AccountDetailsData
    {
        public int AccountId { get; set; }
        public List<FeeItemData> Fees { get; set; } = new List<FeeItemData>();
        public List<PaymentItemData> Payments { get; set; } = new List<PaymentItemData>();
    }

    public class FeeItemData
    {
        public string FeeName { get; set; }
        public decimal Amount { get; set; }
    }

    public class PaymentItemData
    {
        public string ReferenceID { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}
