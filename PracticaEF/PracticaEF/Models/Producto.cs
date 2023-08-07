﻿using System;
using System.Collections.Generic;

namespace PracticaEF.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public int Stock { get; set; }

    public decimal Precio { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
