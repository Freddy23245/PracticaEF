using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using PracticaEF.Models.ViewModels;

namespace PracticaEF.Models;

public partial class PracticaEntityFrameworkContext : DbContext
{
    public PracticaEntityFrameworkContext()
    {
    }

    public PracticaEntityFrameworkContext(DbContextOptions<PracticaEntityFrameworkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public void AgregarVenta(VentasViewModel vent)
    {
        var idCliente = new SqlParameter("@idCliente", vent.IdCliente);
        var idProducto = new SqlParameter("@idProducto", vent.IdProducto);
        var cantidad = new SqlParameter("@cantidad", vent.cantidad);
        Database.ExecuteSqlRaw("EXEC AgregarVenta @idCliente, @idProducto,@cantidad", idCliente, idProducto,cantidad);

    }
    public void DisminuirStock(int idProd,int? cantidades)
    {
        var idProducto = new SqlParameter("@idProducto", idProd);
        var cantidad = new SqlParameter("@cantidad",cantidades);
        Database.ExecuteSqlRaw("EXEC SpDisminuirStock @idProducto, @cantidad", idProducto, cantidad);
    }
    public void AumentarStock(int idProd, int? cantidades)
    {
        var idProducto = new SqlParameter("@idProducto", idProd);
        var cantidad = new SqlParameter("@cantidad", cantidades);
        Database.ExecuteSqlRaw("EXEC SpAumentarStock @idProducto, @cantidad", idProducto, cantidad);
    }
    public void AgregarCliente(ClientesViewModel cli)
    {
        var nombre = new SqlParameter("@nombre", cli.Nombre);
        var apellido = new SqlParameter("@apellido", cli.Apellido);
        var dni = new SqlParameter("@dni", cli.Dni);
        var correo = new SqlParameter("@correo", cli.Correo);
        var telefono = new SqlParameter("@telefono", cli.Telefono);
        Database.ExecuteSqlRaw("EXEC AgregarCliente @nombre,@apellido,@dni,@correo,@telefono", nombre, apellido, dni, correo, telefono);
    }
    public void EditarClientes(ClientesViewModel cli)
    {
        var idCliente = new SqlParameter("@idCliente", cli.IdCliente);
        var nombre = new SqlParameter("@nombre", cli.Nombre);
        var apellido = new SqlParameter("@apellido", cli.Apellido);
        var dni = new SqlParameter("@dni", cli.Dni);
        var correo = new SqlParameter("@correo", cli.Correo);
        var telefono = new SqlParameter("@telefono", cli.Telefono);
        Database.ExecuteSqlRaw("EXEC EditarClientes @idCliente,@nombre,@apellido,@dni,@correo,@telefono", idCliente, nombre, apellido, dni, correo, telefono);

    }
    public void EliminarCliente(int id)
    {
        var idCliente = new SqlParameter("@idCliente", id);
        Database.ExecuteSqlRaw("EXEC EliminarCliente @idCliente", idCliente);
    }
    public List<Cliente> Busqueda(string dni)
    {
        SqlParameter parametroDNI = dni != null ? new SqlParameter("@dni", dni) : new SqlParameter("@dni", DBNull.Value);

        List<Cliente> clientes = Clientes.FromSqlRaw("EXEC SpBuscar @dni", parametroDNI).ToList();
        return clientes;
    }
    public int CantidadVentas()
    {
        int cantidad = 0;
        using (var connection = Database.GetDbConnection())
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SpCantidadVentas";

                //using (var reader = cmd.ExecuteReader())
                //{
                //    if (reader.Read())
                //    {
                //        cantidad = reader.GetInt32(0);
                //    }
                //}
                object? cant = cmd.ExecuteScalar();
                cantidad = Convert.ToInt32(cant);

            }
        }
        return cantidad;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__885457EE70BB2C20");

            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);

            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Clientes");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Productos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
