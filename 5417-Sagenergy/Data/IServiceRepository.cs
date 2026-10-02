using _5417_Sagenergy.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Data
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        IEnumerable<SelectListItem> GetComboServices();
    }
}