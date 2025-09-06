namespace WheelzyProject.Data.Models
{
    public class BuyerZipQuote
    {
        public int BuyerId { get; set; }
        public string ZipCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public bool IsActive { get; set; } = true;


        public Buyer Buyer { get; set; } = null!;
        public ZipCode Zip { get; set; } = null!;
    }
}
