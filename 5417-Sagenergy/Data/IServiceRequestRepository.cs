using _5417_Sagenergy.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Data
{
    public interface IServiceRequestRepository : IGenericRepository<ServiceRequest>
    {
        // Obtém os pedidos de assistência com o cliente e os serviços associados
        Task<IQueryable<ServiceRequest>> GetServiceRequestsAsync();

        // Cria um pedido associando um cliente existente
        Task CreateServiceRequestAsync(ServiceRequest serviceRequest, int clientId);

        // Adiciona um serviço a um pedido de assistência
        Task AddServiceToRequestAsync(int serviceRequestId, int serviceId);

        // Remove um serviço de um pedido de assistência
        Task DeleteServiceFromRequestAsync(int id);
    }
}