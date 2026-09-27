using System.Collections.Generic;
using System.Threading.Tasks;
using _5417_Sagenergy.Data.Entities;

namespace _5417_Sagenergy.Data
{
    public interface IServiceRepository
    {
        // Obtém todos os serviços existentes
        IEnumerable<Service> GetAll();

        // Obtém um serviço específico através do seu ID
        Task<Service> GetByIdAsync(int id);

        // Cria um novo serviço
        Task CreateAsync(Service service);

        // Atualiza um serviço existente
        Task UpdateAsync(Service service);

        // Elimina um serviço
        Task DeleteAsync(Service service);

        // Verifica se existe um serviço com o ID indicado
        Task<bool> ExistAsync(int id);
    }
}