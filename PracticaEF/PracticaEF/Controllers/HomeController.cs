using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaEF.Models;
using System.Diagnostics;
using System.Reflection.Metadata;

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
          decimal? rec =  recaudacion.Sum(v => v.Total);
            ViewBag.Recaudacion = rec; 
            ViewBag.Cliente = cliente;
            ViewBag.Productos = productos;
            ViewBag.Ventas = ventas;
            return View();
        }

        public IActionResult Privacy()
        {
            var reporte = _context.Ventas.Select(x => new
            {
                Cliente = x.IdClienteNavigation.Nombre + x.IdClienteNavigation.Apellido,
                x.Fecha,
                x.IdProductoNavigation.Nombre,
                x.IdProductoNavigation.Precio,
                x.Cantidad,
                totalVenta = x.IdProductoNavigation.Precio * x.Cantidad
            }).ToList();

            using (MemoryStream ms = new MemoryStream())
            {


                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                PdfPTable table = new PdfPTable(6);
                table.AddCell("Cliente");
                table.AddCell("Fecha");
                table.AddCell("Producto");
                table.AddCell("Precio");
                table.AddCell("Cantidad");
                table.AddCell("Total");

                if (reporte != null)
                {
                    foreach (var item in reporte)
                    {
                        table.AddCell(item.Cliente);
                        table.AddCell(item.Fecha.ToString("dd/MM/yyyy"));
                        table.AddCell(item.Nombre);
                        table.AddCell(item.Precio.ToString());
                        table.AddCell(item.Cantidad.ToString());
                        table.AddCell(item.totalVenta.ToString());
                    }
                    
                }
                document.Add(table);
                document.Close();
                return File(ms.ToArray(), "application/pdf", "ejemplo.pdf");
            }


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}