namespace WheelzyProject.Data.Models
{
    public class CaseStatus
    {
        public int StatusId { get; set; }
        public string Name { get; set; } = null!;
        public bool RequiresStatusDate { get; set; }
    }
}
