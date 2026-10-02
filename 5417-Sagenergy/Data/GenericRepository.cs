using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _5417_Sagenergy.Data.Entities;

namespace _5417_Sagenergy.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        // Obtém todos os registos da entidade sem os colocar em tracking
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        // Obtém um registo através do seu ID
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id);
        }

        // Cria um novo registo e guarda as alterações
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        // Atualiza um registo existente
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        // Elimina um registo
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        // Verifica se existe um registo com o ID indicado
        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(model => model.Id == id);
        }

        // Guarda as alterações efetuadas na base de dados
        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}