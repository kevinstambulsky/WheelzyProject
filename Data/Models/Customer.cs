namespace WheelzyProject.Data.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
