using MediatR;
using WheelzyProject.Data.Models;

namespace WheelzyProject.Mediation.Queries
{
    public class GetOrdersQuery : IRequest<List<Order>>
    {
        public DateTime? DateFrom {  get; set; }

        public DateTime? DateTo { get; set; }
        
        public List<int> CustomerIds {  get; set; }

        public List<int> StatusIds { get; set; }

        public bool? isActive { get; set; }
    }
}
