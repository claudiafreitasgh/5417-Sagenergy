using _5417_Sagenergy.Data.Entities;

namespace _5417_Sagenergy.Data
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}