using MediatR;
using Microsoft.EntityFrameworkCore;
using Wheelzy.Data;
using WheelzyProject.Data.Models;
using WheelzyProject.Mediation.Commands;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WheelzyProject.Mediation.Queries
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<Order>>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetOrdersQueryHandler> _logger;
        private readonly WheelzyDbContext _wheelzyDbContext;

        public GetOrdersQueryHandler(IMediator mediator, ILogger<GetOrdersQueryHandler> logger, WheelzyDbContext wheelzyDbContext)
        {
            _mediator = mediator;
            _logger = logger;
            _wheelzyDbContext = wheelzyDbContext;
        }
        public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var orders = _wheelzyDbContext.Orders.AsNoTracking().AsQueryable();

                if (request.DateFrom.HasValue)
                    orders = orders.Where(o => o.OrderDate >= request.DateFrom.Value);

                if (request.DateTo.HasValue)
                    orders = orders.Where(o => o.OrderDate < request.DateTo.Value);

                if (request.CustomerIds.Count > 0)
                    orders = orders.Where(o => request.CustomerIds.Contains(o.CustomerId));

                if (request.StatusIds.Count > 0)
                    orders = orders.Where(o => request.StatusIds.Contains(o.StatusId));

                if (request.isActive.HasValue)
                    orders = orders.Where(o => o.IsActive == request.isActive.Value);

                var result = await orders.Select(o => new Order
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId,
                    StatusId = o.StatusId,
                    IsActive = o.IsActive,
                    Total = o.Total
                })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} orders.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving orders. Filters: DateFrom={DateFrom}, DateTo={DateTo}, Customers={Customers}, Statuses={Statuses}, IsActive={IsActive}",
                    request.DateFrom, request.DateTo,
                    string.Join(",", request.CustomerIds ?? new List<int>()),
                    string.Join(",", request.StatusIds ?? new List<int>()),
                    request.isActive);

                throw; // rethrow so it’s not silently swallowed
            }
        }
    }
}
