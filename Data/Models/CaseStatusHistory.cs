namespace WheelzyProject.Data.Models
{
    public class CaseStatusHistory
    {
        public long CaseStatusHistoryId { get; set; }
        public int CaseId { get; set; }
        public int StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public bool IsCurrent { get; set; }


        public Case Case { get; set; } = null!;
        public CaseStatus Status { get; set; } = null!;
    }
}
