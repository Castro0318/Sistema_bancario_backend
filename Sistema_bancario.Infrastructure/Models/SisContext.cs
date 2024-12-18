using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Sistema_bancario.Infrastructure.Models;

public partial class SisContext : DbContext
{
    public SisContext()
    {
    }

    public SisContext(DbContextOptions<SisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cuentum> Cuenta { get; set; }

    public virtual DbSet<EstadoCuentum> EstadoCuenta { get; set; }

    public virtual DbSet<HistorialCuentum> HistorialCuenta { get; set; }

    public virtual DbSet<TipoCliente> TipoClientes { get; set; }

    public virtual DbSet<TipoCuentum> TipoCuenta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=34.130.213.103;database=sis;uid=root;", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PRIMARY");

            entity.ToTable("Cliente");

            entity.HasIndex(e => e.IdTipoCliente, "id_tipo_cliente");

            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.IdTipoCliente).HasColumnName("id_tipo_cliente");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .HasColumnName("Nombre__cliente")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NumeroIdentificacionCliente)
                .HasMaxLength(100)
                .HasColumnName("Numero_identificacion_cliente")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RepresentanteLegalIdentificacion)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RepresentanteLegalNombre)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RepresentanteLegalTelefono)
                .HasMaxLength(20)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.IdTipoClienteNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdTipoCliente)
                .HasConstraintName("Cliente_ibfk_1");
        });

        modelBuilder.Entity<Cuentum>(entity =>
        {
            entity.HasKey(e => e.IdCuenta).HasName("PRIMARY");

            entity.ToTable("cuenta");

            entity.HasIndex(e => e.IdCliente, "Id_cliente");

            entity.HasIndex(e => e.IdTipoCuenta, "Id_tipo_cuenta");

            entity.HasIndex(e => e.IdEstadoCuenta, "id_estado_cuenta");

            entity.Property(e => e.IdCuenta).HasColumnName("Id_cuenta");
            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.IdEstadoCuenta).HasColumnName("id_estado_cuenta");
            entity.Property(e => e.IdTipoCuenta).HasColumnName("Id_tipo_cuenta");
            entity.Property(e => e.InteresCuenta).HasColumnName("interes_cuenta");
            entity.Property(e => e.Saldo).HasColumnName("saldo");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("cuenta_ibfk_2");

            entity.HasOne(d => d.IdEstadoCuentaNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.IdEstadoCuenta)
                .HasConstraintName("cuenta_ibfk_3");

            entity.HasOne(d => d.IdTipoCuentaNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.IdTipoCuenta)
                .HasConstraintName("cuenta_ibfk_1");
        });

        modelBuilder.Entity<EstadoCuentum>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCuenta).HasName("PRIMARY");

            entity.ToTable("Estado_Cuenta");

            entity.Property(e => e.IdEstadoCuenta).HasColumnName("Id_estado_cuenta");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<HistorialCuentum>(entity =>
        {
            entity.HasKey(e => e.IdHistorialCuenta).HasName("PRIMARY");

            entity.ToTable("Historial_cuenta");

            entity.HasIndex(e => e.IdCuenta, "Id_cuenta");

            entity.Property(e => e.IdHistorialCuenta).HasColumnName("id_Historial_cuenta");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCuenta).HasColumnName("Id_cuenta");
            entity.Property(e => e.SaldoAnterior).HasColumnName("saldo_anterior");
            entity.Property(e => e.SaldoNuevo).HasColumnName("saldo_nuevo");

            entity.HasOne(d => d.IdCuentaNavigation).WithMany(p => p.HistorialCuenta)
                .HasForeignKey(d => d.IdCuenta)
                .HasConstraintName("Historial_cuenta_ibfk_1");
        });

        modelBuilder.Entity<TipoCliente>(entity =>
        {
            entity.HasKey(e => e.IdTipoCliente).HasName("PRIMARY");

            entity.ToTable("Tipo_Cliente");

            entity.Property(e => e.IdTipoCliente).HasColumnName("id_tipo_cliente");
            entity.Property(e => e.NombreTipoCliente)
                .HasMaxLength(100)
                .HasColumnName("Nombre_tipo_cliente")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<TipoCuentum>(entity =>
        {
            entity.HasKey(e => e.IdTipoCuenta).HasName("PRIMARY");

            entity.ToTable("tipo_cuenta");

            entity.Property(e => e.IdTipoCuenta).HasColumnName("Id_tipo_cuenta");
            entity.Property(e => e.NombreTipoCuenta)
                .HasMaxLength(200)
                .HasColumnName("nombre_tipo_cuenta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
