using _5417_Sagenergy.Data;
using _5417_Sagenergy.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceRepository _serviceRepository;

        public ServiceRequestsController(
            IServiceRequestRepository serviceRequestRepository,
            IClientRepository clientRepository,
            IServiceRepository serviceRepository)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
        }


        // Apresenta a lista dos pedidos de assistência
        public async Task<IActionResult> Index()
        {
            var model = await _serviceRequestRepository.GetServiceRequestsAsync();

            return View(model);
        }

        // Apresenta o formulário para criar um novo pedido
        public IActionResult Create()
        {
            ViewBag.Clients = _clientRepository.GetAll()
                .OrderBy(client => client.Name);

            return View();
        }

        // Recebe os dados do formulário e cria o pedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest, int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);

            if (client == null)
            {
                ModelState.AddModelError("Client", "Selecione um cliente válido.");
            }
            else
            {
                ModelState.Remove(nameof(ServiceRequest.Client));
            }

            if (ModelState.IsValid)
            {
                await _serviceRequestRepository.CreateServiceRequestAsync(
                    serviceRequest,
                    clientId);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = _clientRepository.GetAll()
                .OrderBy(c => c.Name);

            return View(serviceRequest);
        }

        // Apresenta os detalhes de um pedido específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _serviceRequestRepository.GetServiceRequestsAsync();

            var serviceRequest = requests
                .FirstOrDefault(request => request.Id == id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // Apresenta o formulário para adicionar um serviço ao pedido
        public IActionResult AddService(int id)
        {
            ViewBag.ServiceRequestId = id;
            ViewBag.Services = _serviceRepository.GetComboServices();

            return View();
        }

        // Adiciona o serviço selecionado ao pedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(int serviceRequestId, int serviceId)
        {
            await _serviceRequestRepository.AddServiceToRequestAsync(
                serviceRequestId,
                serviceId);

            return RedirectToAction(nameof(Details), new { id = serviceRequestId });
        }

        public async Task<IActionResult> DeleteService(int? id, int serviceRequestId)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _serviceRequestRepository.DeleteServiceFromRequestAsync(id.Value);

            return RedirectToAction(nameof(Details), new { id = serviceRequestId });
        }

        // Apresenta o formulário para eliminar um pedido
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _serviceRequestRepository.GetServiceRequestsAsync();

            var serviceRequest = requests
                .FirstOrDefault(request => request.Id == id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // Elimina o pedido depois da confirmação
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(id);

            if (serviceRequest != null)
            {
                try
                {
                    await _serviceRequestRepository.DeleteAsync(serviceRequest);
                }
                catch (DbUpdateException)
                {
                    return View("Error");
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}