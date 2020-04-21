using System.Collections.Generic;

namespace SimchaFund.Web.Models
{
    public class HistoryViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public string ContributorName { get; set; }
        public decimal ContributorBalance { get; set; }
    }
}