using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaEF.Models;
using PracticaEF.Models.ViewModels;

namespace PracticaEF.Controllers
{
    public class ProductosController : Controller
    {
        private readonly PracticaEntityFrameworkContext _context;

        public ProductosController(PracticaEntityFrameworkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Productos = await _context.Productos.ToListAsync();
            var prod = await _context.Productos.ToListAsync();
            ViewBag.Productos = prod;
            return View(prod);
        }
        public IActionResult PorcentajeAumento(int idProducto, decimal porcentaje)
        {
            _context.PorcentajeAumento(idProducto, porcentaje);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductosViewModel producto)
        {
            if (ModelState.IsValid)
            {
                var prod = new Producto()
                {
                    Nombre = producto.Nombre,
                    Stock = producto.Stock,
                    Precio =producto.Precio
                };
                _context.Add(prod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Modificar(int id)
        {
            var prodSeleccionado = await _context.Productos.FindAsync(id);

            return View(prodSeleccionado);
        }
        [BindProperty]
        public Producto producto1 { get; set; } = null!;
        [HttpPost]

        public async Task<IActionResult> Modificar(ProductosViewModel modif)
        {
            if (modif.IdProducto != producto1.IdProducto)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Productos.Attach(producto1);
                _context.Entry(producto1).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _context.Productos.FindAsync(id);
            if (eliminado == null)
                return StatusCode(404);
            else
                _context.Remove(eliminado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 

        }
    }
}
