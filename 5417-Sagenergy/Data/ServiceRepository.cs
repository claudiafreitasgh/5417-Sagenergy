using _5417_Sagenergy.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace _5417_Sagenergy.Data
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly DataContext _context;

        public ServiceRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboServices()
        {
            return _context.Services
                .OrderBy(service => service.Name)
                .Select(service => new SelectListItem
                {
                    Text = service.Name,
                    Value = service.Id.ToString()
                })
                .ToList();
        }
    }
}