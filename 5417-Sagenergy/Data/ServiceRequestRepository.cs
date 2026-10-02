using _5417_Sagenergy.Data.Entities;

namespace _5417_Sagenergy.Data
{
    public class ServiceRequestRepository : GenericRepository<ServiceRequest>, IServiceRequestRepository
    {
        private readonly DataContext _context;

        public ServiceRequestRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}