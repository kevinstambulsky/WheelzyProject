namespace WheelzyProject.Mediation.DTOs
{
    public class InvoiceDTO
    {
        public required int CustomerId { get; set; }

        public required decimal Total {  get; set; }
    }
}
