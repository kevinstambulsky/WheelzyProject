namespace WheelzyProject.Data.Models
{
    public class Case
    {
        public int CaseId { get; set; }
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public string ZipCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; }


        public Customer Customer { get; set; } = null!;
        public Car Car { get; set; } = null!;
        public ZipCode Zip { get; set; } = null!;
    }
}
