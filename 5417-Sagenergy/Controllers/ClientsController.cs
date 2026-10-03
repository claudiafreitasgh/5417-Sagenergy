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
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;

        public ClientsController(
            IClientRepository clientRepository,
            IUserHelper userHelper)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
        }

        // Admin sees all clients. Customer sees only their own client.
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(_clientRepository.GetAll().OrderBy(model => model.Name));
            }

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

            return View(new[] { client }.AsQueryable());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientRepository.CreateAsync(client);
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user == null || client.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Admin"))
                {
                    var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    if (user == null)
                    {
                        return Forbid();
                    }

                    var existingClient = await _clientRepository.GetByIdAsync(client.Id);

                    if (existingClient == null || existingClient.UserId != user.Id)
                    {
                        return Forbid();
                    }

                    
                    client.UserId = existingClient.UserId;
                }

                try
                {
                    await _clientRepository.UpdateAsync(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clientRepository.ExistAsync(client.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);

            if (client != null)
            {
                try
                {
                    await _clientRepository.DeleteAsync(client);
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Não foi possível eliminar o cliente porque existem registos associados."
                    );

                    return View(client);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}