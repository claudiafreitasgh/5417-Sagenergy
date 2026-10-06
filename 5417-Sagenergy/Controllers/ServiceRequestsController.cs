using _5417_Sagenergy.Data;
using _5417_Sagenergy.Data.Entities;
using _5417_Sagenergy.Helpers;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserHelper _userHelper;

        public ServiceRequestsController(
            IServiceRequestRepository serviceRequestRepository,
            IClientRepository clientRepository,
            IServiceRepository serviceRepository,
            IUserHelper userHelper)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
            _userHelper = userHelper;
        }

        // Admin sees all requests. Customer sees only their own requests.
        public async Task<IActionResult> Index()
        {
            var requests = await _serviceRequestRepository.GetServiceRequestsAsync();

            if (User.IsInRole("Admin"))
            {
                return View(requests);
            }

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            var model = requests
                .Where(request => request.Client.UserId == user.Id);

            return View(model);
        }

        // Admin can choose the client. Customer uses their own client automatically.
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Clients = _clientRepository.GetAll()
                    .OrderBy(client => client.Name);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ServiceRequest serviceRequest,
            int? clientId)
        {
            if (User.IsInRole("Admin"))
            {
                if (clientId == null)
                {
                    ModelState.AddModelError(
                        "Client",
                        "Please select a client.");
                }
                else
                {
                    ModelState.Remove(nameof(ServiceRequest.Client));
                }
            }
            else
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null)
                {
                    return NotFound();
                }

                var client = _clientRepository
                    .GetAll()
                    .FirstOrDefault(model => model.UserId == user.Id);

                if (client == null)
                {
                    return NotFound();
                }

                clientId = client.Id;

                ModelState.Remove(nameof(ServiceRequest.Client));
            }

            if (ModelState.IsValid)
            {
                await _serviceRequestRepository.CreateServiceRequestAsync(
                    serviceRequest,
                    clientId.Value);

                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Admin"))
            {
                ViewBag.Clients = _clientRepository.GetAll()
                    .OrderBy(client => client.Name);
            }

            return View(serviceRequest);
        }

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

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || serviceRequest.Client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            return View(serviceRequest);
        }

        public async Task<IActionResult> AddService(int id)
        {
            var requests = await _serviceRequestRepository.GetServiceRequestsAsync();

            var serviceRequest = requests
                .FirstOrDefault(request => request.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || serviceRequest.Client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            ViewBag.ServiceRequestId = id;
            ViewBag.Services = _serviceRepository.GetComboServices();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(
            int serviceRequestId,
            int serviceId)
        {
            var requests = await _serviceRequestRepository.GetServiceRequestsAsync();

            var serviceRequest = requests
                .FirstOrDefault(request => request.Id == serviceRequestId);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || serviceRequest.Client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            await _serviceRequestRepository.AddServiceToRequestAsync(
                serviceRequestId,
                serviceId);

            return RedirectToAction(nameof(Details), new { id = serviceRequestId });
        }


        public async Task<IActionResult> Edit(int? id)
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

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || serviceRequest.Client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            return View(serviceRequest);
        }


        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
                int id,
                ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            var requests = await _serviceRequestRepository
                .GetServiceRequestsAsync();

            var existingRequest = requests
                .FirstOrDefault(request => request.Id == id);

            if (existingRequest == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper
                    .GetUserByEmailAsync(User.Identity.Name);

                if (user == null ||
                    existingRequest.Client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            ModelState.Remove(nameof(ServiceRequest.Client));

            if (ModelState.IsValid)
            {
                existingRequest.RequestDate = serviceRequest.RequestDate;
                existingRequest.Description = serviceRequest.Description;

                await _serviceRequestRepository.UpdateAsync(existingRequest);

                return RedirectToAction(nameof(Index));
            }

            serviceRequest.Client = existingRequest.Client;

            return View(serviceRequest);
        }

        public async Task<IActionResult> DeleteService(
            int? id,
            int serviceRequestId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _serviceRequestRepository.GetServiceRequestsAsync();

            var serviceRequest = requests
                .FirstOrDefault(request => request.Id == serviceRequestId);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || serviceRequest.Client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            await _serviceRequestRepository.DeleteServiceFromRequestAsync(id.Value);

            return RedirectToAction(
                nameof(Details),
                new { id = serviceRequestId });
        }

        [Authorize(Roles = "Admin")]
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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