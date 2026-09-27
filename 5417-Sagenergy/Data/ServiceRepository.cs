using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _5417_Sagenergy.Data.Entities;

namespace _5417_Sagenergy.Data
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DataContext _context;

        public ServiceRepository(DataContext context)
        {
            _context = context;
        }

        // Obtém todos os serviços existentes
        public IEnumerable<Service> GetAll()
        {
            return _context.Services;
        }

        // Obtém um serviço específico através do seu ID
        public async Task<Service> GetByIdAsync(int id)
        {
            return await _context.Services
                .FirstOrDefaultAsync(model => model.Id == id);
        }

        // Cria um novo serviço na base de dados
        public async Task CreateAsync(Service service)
        {
            _context.Add(service);
            await _context.SaveChangesAsync();
        }

        // Atualiza um serviço existente
        public async Task UpdateAsync(Service service)
        {
            _context.Update(service);
            await _context.SaveChangesAsync();
        }

        // Elimina um serviço da base de dados
        public async Task DeleteAsync(Service service)
        {
            _context.Remove(service);
            await _context.SaveChangesAsync();
        }

        // Verifica se existe um serviço com o ID indicado
        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Services
                .AnyAsync(model => model.Id == id);
        }
    }
}