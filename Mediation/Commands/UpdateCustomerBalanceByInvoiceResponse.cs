using WheelzyProject.Data.Models;

namespace WheelzyProject.Mediation.Commands
{
    public class UpdateCustomerBalanceByInvoiceResponse
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
