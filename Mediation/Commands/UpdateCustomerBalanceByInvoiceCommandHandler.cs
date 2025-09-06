using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Wheelzy.Data;

namespace WheelzyProject.Mediation.Commands
{
    public class UpdateCustomerBalanceByInvoiceCommandHandler : IRequestHandler<UpdateCustomerBalanceByInvoiceRequest, UpdateCustomerBalanceByInvoiceResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateCustomerBalanceByInvoiceCommandHandler> _logger;
        private readonly WheelzyDbContext _wheelzyDbContext;

        public UpdateCustomerBalanceByInvoiceCommandHandler(IMediator mediator, ILogger<UpdateCustomerBalanceByInvoiceCommandHandler> logger, WheelzyDbContext wheelzyDbContext)
        {
            _mediator = mediator;
            _logger = logger;
            _wheelzyDbContext = wheelzyDbContext;
        }

        public async Task<UpdateCustomerBalanceByInvoiceResponse> Handle(UpdateCustomerBalanceByInvoiceRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdateCustomerBalanceByInvoiceResponse();

            if (request.Invoices.Count == 0)
            {
                _logger.LogInformation("Trying to update customers by Invoice. No Invoices.");
                response.Success = false;
                response.ErrorMessage = "No invoices were provided.";
                return response;
            }

            try
            {
                var customersIds = request.Invoices
                    .GroupBy(x => x.CustomerId)
                    .ToDictionary(y => y.Key, y => y.Sum(x => x.Total));

                var customers = await _wheelzyDbContext.Customers
                    .Where(x => customersIds.Keys.Contains(x.CustomerId))
                    .ToListAsync(cancellationToken);

                if (customers.Count == 0)
                {
                    _logger.LogWarning("No customers found for the provided invoices.");
                    response.Success = false;
                    response.ErrorMessage = "No matching customers found.";
                    return response;
                }

                foreach (var c in customers)
                {
                    c.Balance -= customersIds[c.CustomerId];
                }

                await _wheelzyDbContext.SaveChangesAsync(cancellationToken);

                response.Customers = customers;
                response.Customers = customers;
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating customers by Invoices.");
                response.Success = false;
                response.ErrorMessage = ex.ToString();
            }

            return response;
        }
    }
}