namespace WheelzyProject.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public decimal Total { get; set; }

        public Customer Customer { get; set; } = null!;
        public OrderStatus Status { get; set; } = null!;
    }
}
