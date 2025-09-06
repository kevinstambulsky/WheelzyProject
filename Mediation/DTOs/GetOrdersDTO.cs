namespace WheelzyProject.Mediation.DTOs
{
    public class GetOrdersDTO
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public List<int> CustomerIds { get; set; }

        public List<int> StatusIds { get; set; }

        public bool? isActive { get; set; }
    }
}
