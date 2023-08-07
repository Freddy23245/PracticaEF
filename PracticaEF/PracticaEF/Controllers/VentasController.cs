using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaEF.Models;
using PracticaEF.Models.ViewModels;

namespace PracticaEF.Controllers
{
    public class VentasController : Controller
    {
        private readonly PracticaEntityFrameworkContext _context;
        public VentasController(PracticaEntityFrameworkContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas.Include(x => x.IdClienteNavigation).Include(p => p.IdProductoNavigation).ToListAsync();
            ViewBag.Ventas = ventas;
            var cantidad = _context.CantidadVentas();
            ViewBag.Cantidad = cantidad;
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Clientes = await _context.Clientes.Select(x => new { x.IdCliente, NombreCliente = x.Nombre + " " + x.Apellido }).ToListAsync();
            return View();
        }
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _context.Ventas.FindAsync(id);
            if (eliminado == null)
                return StatusCode(404);
            else
                _context.Remove(eliminado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Create(VentasViewModel venta)
        {
            if (ModelState.IsValid)
            {
                _context.AgregarVenta(venta);
                return RedirectToAction(nameof(Index));

            }
            return View();
        }
    }
}
