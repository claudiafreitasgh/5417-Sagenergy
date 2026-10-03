using _5417_Sagenergy.Data;
using _5417_Sagenergy.Data.Entities;
using _5417_Sagenergy.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace _5417_Sagenergy.Controllers
{
    public class ServicesController : Controller
    {
        // Repository responsável pelo acesso aos dados dos serviços
        private readonly IServiceRepository _serviceRepository;
        private readonly IImageHelper _imageHelper;

        public ServicesController(
            IServiceRepository serviceRepository,
            IImageHelper imageHelper)
        {
            _serviceRepository = serviceRepository;
            _imageHelper = imageHelper;
        }

        // Apresenta a lista de serviços ordenada pelo nome
        public IActionResult Index()
        {
            return View(_serviceRepository.GetAll().OrderBy(model => model.Name));
        }


        //GET create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Post. Recebe os dados do formulário e guarda o novo serviço na base de dados
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Service service, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    service.ImageUrl = await _imageHelper.UploadImageAsync(
                        imageFile,
                        "services");
                }

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

        // Apresenta o formulário para editar um serviço existente
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Procura o serviço através do Repository
            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Service service, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var currentService = await _serviceRepository.GetByIdAsync(service.Id);

                if (currentService == null)
                {
                    return NotFound();
                }

                if (imageFile != null)
                {
                    service.ImageUrl = await _imageHelper.UploadImageAsync(
                        imageFile,
                        "Services");
                }
                else
                {
                    service.ImageUrl = currentService.ImageUrl;
                }

                try
                {
                    await _serviceRepository.UpdateAsync(service);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _serviceRepository.ExistAsync(service.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(service);
        }


        // Apresenta a confirmação antes de eliminar um serviço
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Procura o serviço através do Repository
            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // Elimina o serviço depois da confirmação
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            if (service != null)
            {
                try
                {
                    await _serviceRepository.DeleteAsync(service);
                }
                catch (DbUpdateException)
                {
                    // Se existir uma relação com outro registo,
                    // a eliminação pode ser impedida pela base de dados.
                    ModelState.AddModelError(
                        string.Empty,
                        "Não foi possível eliminar o serviço porque existem registos associados."
                    );

                    return View(service);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
