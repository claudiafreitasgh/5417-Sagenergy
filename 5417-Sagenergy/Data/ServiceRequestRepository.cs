using _5417_Sagenergy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Data
{
    public class ServiceRequestRepository : GenericRepository<ServiceRequest>, IServiceRequestRepository
    {
        private readonly DataContext _context;

        public ServiceRequestRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        // Obtém os pedidos com o cliente e os serviços associados
        public async Task<IQueryable<ServiceRequest>> GetServiceRequestsAsync()
        {
            return _context.ServiceRequests
                .Include(request => request.Client)
                .Include(request => request.Items)
                .ThenInclude(detail => detail.Service)
                .OrderByDescending(request => request.RequestDate);
        }

        // Adiciona um serviço a um pedido de assistência
        public async Task AddServiceToRequestAsync(int serviceRequestId, int serviceId)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(serviceRequestId);
            var service = await _context.Services.FindAsync(serviceId);

            if (serviceRequest == null || service == null)
            {
                return;
            }

            var detail = new ServiceRequestDetail
            {
                ServiceRequest = serviceRequest,
                Service = service,
                Price = service.Price
            };

            _context.ServiceRequestDetails.Add(detail);

            await _context.SaveChangesAsync();
        }

        // Remove um serviço de um pedido de assistência
        public async Task DeleteServiceFromRequestAsync(int id)
        {
            var detail = await _context.ServiceRequestDetails.FindAsync(id);

            if (detail == null)
            {
                return;
            }

            _context.ServiceRequestDetails.Remove(detail);

            await _context.SaveChangesAsync();
        }
    }
}