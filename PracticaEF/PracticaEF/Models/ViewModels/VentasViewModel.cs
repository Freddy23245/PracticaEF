using System.ComponentModel.DataAnnotations;

namespace PracticaEF.Models.ViewModels
{
    public class VentasViewModel
    {
        public int IdVenta { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public int cantidad { get; set; }

        public DateTime Fecha { get; set; }


    }
}
