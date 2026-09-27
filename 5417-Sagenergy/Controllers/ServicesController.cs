using _5417_Sagenergy.Data;
using _5417_Sagenergy.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Controllers
{
    public class ServicesController : Controller
    {
        // Repository responsável pelo acesso aos dados dos serviços
        private readonly IServiceRepository _serviceRepository;

        public ServicesController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        // Apresenta a lista de serviços ordenada pelo nome
        public IActionResult Index()
        {
            return View(_serviceRepository.GetAll().OrderBy(model => model.Name));
        }


        //GET create
        public IActionResult Create()
        {
            return View();
        }

        // Post. Recebe os dados do formulário e guarda o novo serviço na base de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                await _serviceRepository.CreateAsync(service);

                return RedirectToAction(nameof(Index));
            }

            return View(service);
        }

        // Apresenta os detalhes de um serviço específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
    }
}
