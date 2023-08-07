using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaEF.Models;
using System.Diagnostics;

namespace PracticaEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PracticaEntityFrameworkContext _context;
        public HomeController(ILogger<HomeController> logger, PracticaEntityFrameworkContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            var cliente = _context.Clientes.Count();
            var productos = _context.Productos.Count();
            var ventas = _context.Ventas.Count();
            var recaudacion = _context.Ventas.Select(x => new { Total = x.IdProductoNavigation.Precio * x.Cantidad }).ToList();
          decimal? recauda =  recaudacion.Sum(v => v.Total);
            ViewBag.Recaudacion = recauda;
            ViewBag.Cliente = cliente;
            ViewBag.Productos = productos;
            ViewBag.Ventas = ventas;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}