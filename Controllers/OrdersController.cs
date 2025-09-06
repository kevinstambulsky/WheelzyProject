using MediatR;
using Microsoft.AspNetCore.Mvc;
using WheelzyProject.Data.Models;
using WheelzyProject.Mediation.DTOs;
using WheelzyProject.Mediation.Queries;

namespace WheelzyProject.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public OrdersController(ILogger<CustomerController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders([FromQuery] GetOrdersDTO payload)
        {
            var request = new GetOrdersQuery 
            { CustomerIds = payload.CustomerIds,
               DateFrom = payload.DateFrom,
               DateTo = payload.DateTo,
               StatusIds = payload.StatusIds,
               isActive = payload.isActive};

            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
