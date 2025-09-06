using MediatR;
using WheelzyProject.Mediation.DTOs;

namespace WheelzyProject.Mediation.Commands
{
    public class UpdateCustomerBalanceByInvoiceRequest : IRequest<UpdateCustomerBalanceByInvoiceResponse>
    {
        public List<InvoiceDTO> Invoices { get; set; } = new List<InvoiceDTO>();
    }
}
