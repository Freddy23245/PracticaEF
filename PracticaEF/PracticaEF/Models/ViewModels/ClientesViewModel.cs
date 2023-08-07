using System.ComponentModel.DataAnnotations;

namespace PracticaEF.Models.ViewModels
{
    public class ClientesViewModel
    {
        public int IdCliente { get; set; }
        [Required(ErrorMessage ="Nombre requerido")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Apellido requerido")]
        public string Apellido { get; set; } = null!;
        [Required(ErrorMessage = "Dni requerido")]
        [StringLength(8,ErrorMessage ="El dni solo lleva 8 numeros")]
        [MinLength(8,ErrorMessage ="El campo acepta como minimo 8 caracteres")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El DNI debe contener solo números.")]
        public string Dni { get; set; } = null!;
        [Required(ErrorMessage = "Correo requerido")]
        public string Correo { get; set; } = null!;
        [Required(ErrorMessage = "Telefono requerido")]
        [StringLength(10, ErrorMessage = "El Telefono solo lleva 10 numeros")]
        [MinLength(8, ErrorMessage = "El campo acepta como minimo 10 caracteres")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El Teléfono debe contener solo números.")]
        public string? Telefono { get; set; }
    }
}
