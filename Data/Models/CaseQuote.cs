namespace WheelzyProject.Data.Models
{
    public class CaseQuote
    {
        public long CaseQuoteId { get; set; }
        public int CaseId { get; set; }
        public int BuyerId { get; set; }
        public decimal Amount { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }


        public Case Case { get; set; } = null!;
        public Buyer Buyer { get; set; } = null!;
    }
}
