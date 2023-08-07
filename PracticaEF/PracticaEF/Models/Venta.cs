using System;
using System.Collections.Generic;

namespace PracticaEF.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public int IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
