using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionSolicitud.Models;

public partial class DbFlujosTestContext : DbContext
{
    public DbFlujosTestContext()
    {
    }

    public DbFlujosTestContext(DbContextOptions<DbFlujosTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FlCuentasContable> FlCuentasContables { get; set; }

    public virtual DbSet<Flarea> Flareas { get; set; }

    public virtual DbSet<Flauditorium> Flauditoria { get; set; }

    public virtual DbSet<Flflujo> Flflujos { get; set; }

    public virtual DbSet<FlflujoDet> FlflujoDets { get; set; }

    public virtual DbSet<FlflujoProceso> FlflujoProcesos { get; set; }

    public virtual DbSet<Flmaquina> Flmaquinas { get; set; }

    public virtual DbSet<Flrol> Flrols { get; set; }

    public virtual DbSet<Flsolicitud> Flsolicituds { get; set; }

    public virtual DbSet<FlsolicitudDet> FlsolicitudDets { get; set; }

    public virtual DbSet<Flstatus> Flstatuses { get; set; }

    public virtual DbSet<FltipoSolicitud> FltipoSolicituds { get; set; }

    public virtual DbSet<FltipoSxSolicitante> FltipoSxSolicitantes { get; set; }

    public virtual DbSet<Flusuario> Flusuarios { get; set; }

    public virtual DbSet<VwEmailsNotificarAut> VwEmailsNotificarAuts { get; set; }

    public virtual DbSet<VwEmpleado> VwEmpleados { get; set; }

    public virtual DbSet<VwFlujoDet> VwFlujoDets { get; set; }

    public virtual DbSet<VwProcesarEnErp> VwProcesarEnErps { get; set; }

    public virtual DbSet<VwProducto> VwProductos { get; set; }

    public virtual DbSet<VwRolUsuario> VwRolUsuarios { get; set; }

    public virtual DbSet<VwSolicitudDetalle> VwSolicitudDetalles { get; set; }

    public virtual DbSet<VwSolicitudEncabezado> VwSolicitudEncabezados { get; set; }

    public virtual DbSet<VwTipoSxSolicitante> VwTipoSxSolicitantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-8V0O92K\\SQLEXPRESS;Initial Catalog=DbFlujosTest;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<FlCuentasContable>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Cuenta)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Flarea>(entity =>
        {
            entity.HasKey(e => e.IdArea);

            entity.ToTable("FLArea");

            entity.Property(e => e.IdArea).HasColumnName("idArea");
            entity.Property(e => e.NombreArea)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Flauditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria);

            entity.ToTable("FLAuditoria");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Notas)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Flflujo>(entity =>
        {
            entity.HasKey(e => e.IdFlujo);

            entity.ToTable("FLFlujo");

            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.DescripcionFlujo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HorasReintento).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<FlflujoDet>(entity =>
        {
            entity.HasKey(e => e.IdFlujoDet);

            entity.ToTable("FLFlujoDet");

            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.StatusSiguiente)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAutorizadorNavigation).WithMany(p => p.FlflujoDetIdAutorizadorNavigations)
                .HasForeignKey(d => d.IdAutorizador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLFlujoDet_FLUsuarios");

            entity.HasOne(d => d.IdAutorizadorAlternoNavigation).WithMany(p => p.FlflujoDetIdAutorizadorAlternoNavigations)
                .HasForeignKey(d => d.IdAutorizadorAlterno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLFlujoDet_FLUsuarios1");

            entity.HasOne(d => d.IdFlujoNavigation).WithMany(p => p.FlflujoDets)
                .HasForeignKey(d => d.IdFlujo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLFlujoDet_FLFlujo");

            entity.HasOne(d => d.StatusSiguienteNavigation).WithMany(p => p.FlflujoDets)
                .HasForeignKey(d => d.StatusSiguiente)
                .HasConstraintName("FK_FLFlujoDet_FLStatus");
        });

        modelBuilder.Entity<FlflujoProceso>(entity =>
        {
            entity.HasKey(e => new { e.IdSolicitud, e.IdFlujoDet });

            entity.ToTable("FLFlujoProceso");

            entity.Property(e => e.Autorizado).HasDefaultValue(false);
            entity.Property(e => e.Ejecutado).HasDefaultValue(false);
            entity.Property(e => e.SiguienteEstado)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAutorizadorNavigation).WithMany(p => p.FlflujoProcesoIdAutorizadorNavigations)
                .HasForeignKey(d => d.IdAutorizador)
                .HasConstraintName("FK_FLFlujoProceso_FLUsuarios");

            entity.HasOne(d => d.IdAutorizadorAlternoNavigation).WithMany(p => p.FlflujoProcesoIdAutorizadorAlternoNavigations)
                .HasForeignKey(d => d.IdAutorizadorAlterno)
                .HasConstraintName("FK_FLFlujoProceso_FLUsuarios1");

            entity.HasOne(d => d.IdFlujoDetNavigation).WithMany(p => p.FlflujoProcesos)
                .HasForeignKey(d => d.IdFlujoDet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLFlujoProceso_FLFlujoDet");

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.FlflujoProcesos)
                .HasForeignKey(d => d.IdSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLFlujoProceso_FLSolicitud");
        });

        modelBuilder.Entity<Flmaquina>(entity =>
        {
            entity.HasKey(e => e.IdMaquina);

            entity.ToTable("FLMaquina");

            entity.Property(e => e.CodigoReferencia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreMaquina)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Flrol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("FLRol");

            entity.Property(e => e.NombreRol)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.IdUsuarios).WithMany(p => p.IdRols)
                .UsingEntity<Dictionary<string, object>>(
                    "FlrolUsuario",
                    r => r.HasOne<Flusuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FLRolUsuario_FLUsuarios"),
                    l => l.HasOne<Flrol>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FLRolUsuario_FLRol"),
                    j =>
                    {
                        j.HasKey("IdRol", "IdUsuario");
                        j.ToTable("FLRolUsuario");
                    });
        });

        modelBuilder.Entity<Flsolicitud>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud);

            entity.ToTable("FLSolicitud", tb => tb.HasTrigger("CrearFlujosProcesos"));

            entity.Property(e => e.Cancelada).HasDefaultValue(false);
            entity.Property(e => e.Comentarios)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DocNumErp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DocNumERP");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.Flsolicituds)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK_FLSolicitud_FLArea");

            entity.HasOne(d => d.IdSolicitanteNavigation).WithMany(p => p.Flsolicituds)
                .HasForeignKey(d => d.IdSolicitante)
                .HasConstraintName("FK_FLSolicitud_FLUsuarios");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Flsolicituds)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_FLSolicitud_FLStatus");

            entity.HasOne(d => d.IdTipoSolicitudNavigation).WithMany(p => p.Flsolicituds)
                .HasForeignKey(d => d.IdTipoSolicitud)
                .HasConstraintName("FK_FLSolicitud_FLTipoSolicitud");
        });

        modelBuilder.Entity<FlsolicitudDet>(entity =>
        {
            entity.HasKey(e => new { e.IdSolicitud, e.IdProducto, e.IdMaquina });

            entity.ToTable("FLSolicitudDet");

            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 4)");
            entity.Property(e => e.ComentariosDet)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Umedida)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMaquinaNavigation).WithMany(p => p.FlsolicitudDets)
                .HasForeignKey(d => d.IdMaquina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLSolicitudDet_FLMaquina");

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.FlsolicitudDets)
                .HasForeignKey(d => d.IdSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLSolicitudDet_FLSolicitud");
        });

        modelBuilder.Entity<Flstatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus);

            entity.ToTable("FLStatus");

            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.NombreStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Orden).HasColumnName("orden");
        });

        modelBuilder.Entity<FltipoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdTipoSolicitud).HasName("PK_FLTipodeSolicitud");

            entity.ToTable("FLTipoSolicitud");

            entity.Property(e => e.ActivoTipoS)
                .HasDefaultValue(true)
                .HasColumnName("activoTipoS");
            entity.Property(e => e.NombreTipoSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FltipoSxSolicitante>(entity =>
        {
            entity.HasKey(e => e.IdTipoSxSolicitante);

            entity.ToTable("FLTipoSxSolicitante");

            entity.HasIndex(e => new { e.IdTipoSolicitud, e.IdSolicitante }, "IX_FLTipoSxSolicitante").IsUnique();

            entity.HasOne(d => d.IdSolicitanteNavigation).WithMany(p => p.FltipoSxSolicitantes)
                .HasForeignKey(d => d.IdSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLTipoSxSolicitante_FLUsuarios");

            entity.HasOne(d => d.IdTipoSolicitudNavigation).WithMany(p => p.FltipoSxSolicitantes)
                .HasForeignKey(d => d.IdTipoSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FLTipoSxSolicitante_FLTipoSolicitud");
        });

        modelBuilder.Entity<Flusuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("FLUsuarios", tb => tb.HasTrigger("CargaEmpleado"));

            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.CodEmplea)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<VwEmailsNotificarAut>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwEmailsNotificarAUT");

            entity.Property(e => e.EmailEncargadoBodega)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmailSolicitante)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwEmpleado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwEmpleados");

            entity.Property(e => e.EmpEstado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("emp_estado");
            entity.Property(e => e.ExpCodigoAlternativo)
                .HasMaxLength(36)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("exp_codigo_alternativo");
            entity.Property(e => e.ExpEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("exp_email");
            entity.Property(e => e.NomDepto)
                .HasMaxLength(250)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS");
            entity.Property(e => e.Nombre)
                .HasMaxLength(152)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("nombre");
            entity.Property(e => e.Usuario)
                .HasMaxLength(51)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<VwFlujoDet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwFlujoDet");

            entity.Property(e => e.Comentarios)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DescripcionFlujo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DocNumErp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DocNumERP");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.HorasReintento).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.NombreTipoSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusSiguiente)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwProcesarEnErp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProcesarEnERP");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 4)");
            entity.Property(e => e.Comentarios)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ComentariosDet)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.NombreArea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Umedida)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<VwProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwProductos");

            entity.Property(e => e.AcctCode)
                .HasMaxLength(15)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.AreaRepuesto)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS")
                .HasColumnName("codigo");
            entity.Property(e => e.Existencia)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("existencia");
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.ItmsGrpNam)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.UniMed)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS")
                .HasColumnName("uni_med");
            entity.Property(e => e.WhsCode)
                .HasMaxLength(8)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
        });

        modelBuilder.Entity<VwRolUsuario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwRolUsuario");

            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CodEmplea)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fecharegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreRol)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<VwSolicitudDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwSolicitudDetalle");

            entity.Property(e => e.AcctCode)
                .HasMaxLength(15)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.AreaRepuesto)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.Cantidad).HasColumnType("decimal(20, 4)");
            entity.Property(e => e.CodEmplea)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Comentarios)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ComentariosDet)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DocNumErp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DocNumERP");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(216)
                .UseCollation("SQL_Latin1_General_CP850_CI_AS");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreArea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreMaquina)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NombreTipoSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Umedida)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<VwSolicitudEncabezado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwSolicitudEncabezado");

            entity.Property(e => e.CodEmplea)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Comentarios)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DocNumErp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DocNumERP");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdStatus)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreArea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NombreTipoSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<VwTipoSxSolicitante>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTipoSxSolicitante");

            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.ActivoTipoS).HasColumnName("activoTipoS");
            entity.Property(e => e.CodEmplea)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fecharegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreTipoSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
