using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Data
{
    public interface IGenericRepository<T> where T : class
    {
        // Obtém todos os registos da entidade
        IQueryable<T> GetAll();

        // Obtém um registo através do seu ID
        Task<T> GetByIdAsync(int id);

        // Cria um novo registo
        Task CreateAsync(T entity);

        // Atualiza um registo existente
        Task UpdateAsync(T entity);

        // Elimina um registo
        Task DeleteAsync(T entity);

        // Verifica se existe um registo com o ID indicado
        Task<bool> ExistAsync(int id);
    }
}