using System.ComponentModel.DataAnnotations;

namespace PracticaEF.Models.ViewModels
{
    public class ProductosViewModel
    {
        public int IdProducto { get; set; }
        [Required(ErrorMessage ="El campo nombre es obligatorio")]
        public string Nombre { get; set; }

       
        [RegularExpression("^[0-9]+$", ErrorMessage = "El Stock debe contener solo números.")]
        public int Stock { get; set; }
        
        
        [RegularExpression("^[0-9]+$", ErrorMessage = "El Precio debe contener solo números.")]
        public decimal Precio { get; set; }

    }
}
