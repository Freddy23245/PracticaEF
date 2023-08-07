using System;
using System.Collections.Generic;

namespace PracticaEF.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
