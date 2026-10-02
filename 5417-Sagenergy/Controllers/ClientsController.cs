using _5417_Sagenergy.Data;
using _5417_Sagenergy.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Controllers
{
    public class ClientsController : Controller
    {
        // Repository responsável pelo acesso aos dados dos clientes
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Apresenta a lista de clientes ordenada pelo nome
        public IActionResult Index()
        {
            return View(_clientRepository.GetAll().OrderBy(model => model.Name));
        }

        // Apresenta o formulário para criar um novo cliente
        public IActionResult Create()
        {
            return View();
        }

        // Recebe os dados do formulário e guarda o novo cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientRepository.CreateAsync(client);
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }

        // Apresenta os detalhes de um cliente específico
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

            return View(client);
        }

        // Apresenta o formulário para editar um cliente existente
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

            return View(client);
        }

        // Recebe os dados alterados e atualiza o cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clientRepository.UpdateAsync(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Verifica se o cliente ainda existe antes de devolver erro
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

        // Apresenta a confirmação antes de eliminar um cliente
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

        // Elimina o cliente depois da confirmação
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
                    // Impede a eliminação quando existem registos associados
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