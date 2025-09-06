using MediatR;
using Microsoft.AspNetCore.Mvc;
using WheelzyProject.Mediation.Commands;
using WheelzyProject.Mediation.DTOs;

namespace WheelzyProject.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<ActionResult<UpdateCustomerBalanceByInvoiceResponse>> UpdateCustomerBalanceByInvoice([FromBody] List<InvoiceDTO> payload)
        {
            var request = new UpdateCustomerBalanceByInvoiceRequest { Invoices = payload };

            // Use MediatR to handle the command
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
