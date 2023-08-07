using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaEF.Models;
using PracticaEF.Models.ViewModels;

namespace PracticaEF.Controllers
{
    public class ClientesController : Controller
    {
        private readonly PracticaEntityFrameworkContext _context;

        public ClientesController(PracticaEntityFrameworkContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           var cli = await _context.Clientes.ToListAsync();
            Cliente cliente = new Cliente();

            ViewBag.Clientes = cli;
            return View(cliente);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ClientesViewModel cliente)
        {
          
            if (ModelState.IsValid)
            {
                _context.AgregarCliente(cliente);
                    return RedirectToAction(nameof(Index));

            }
            return View();
        }

        public async Task<IActionResult> Modificar(int id)
        {
            var clienteSeleccionado = await _context.Clientes.FindAsync(id);

            return View(clienteSeleccionado);
        }
        [BindProperty]
        public Cliente cli3 { get; set; } = null!;
        [HttpPost]
   
        public IActionResult Modificar(ClientesViewModel modif)
        {
            if(modif.IdCliente != cli3.IdCliente)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.EditarClientes(modif);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _context.Clientes.FindAsync(id);
            if (eliminado == null)
                return StatusCode(404);
            else
                _context.EliminarCliente(id);

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Busqueda(string dni)
        {
            List<Cliente> clientes = _context.Busqueda(dni);
            return View(clientes);
        }
    }
}
