using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClasesTallerMecanico.Datos
{
    public class FacturasDBContext : DbContext
    {
        public FacturasDBContext(DbContextOptions<FacturasDBContext> options) : base(options)
        {
        }

        public DbSet<CategoriaTrabajo> CategoriasTrabajos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<DetalleFacturaCompra> DetallesFacturasCompras { get; set; }
        public DbSet<DetalleFacturaVenta> DetallesFacturasVentas { get; set; }
        public DbSet<DetalleTurno> DetallesTurnos { get; set; }
        public DbSet<EstadoTurno> EstadosTurno { get; set; }
        public DbSet<FacturaCompra> FacturasCompras { get; set; }
        public DbSet<FacturaVenta> FacturasVentas { get; set; }
        public DbSet<FormaPago> FormasPago { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<InsumoPorTrabajo> InsumosPorTrabajo { get; set; }
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<SesionCaja> SesionesCaja { get; set; }
        public DbSet<TipoTurno> TiposTurno { get; set; }
        public DbSet<Trabajo> Trabajos { get; set; }
        public DbSet<TrabajoPorTurno> TrabajosPorTurno { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Relaciones que quedaban en Cascade por convención de EF Core ----
            // (toda FK requerida sin OnDelete explícito cae en Cascade por defecto).
            // Se fuerzan a Restrict porque son tablas de "catálogo" o "cabecera":
            // borrarlas jamás debería arrastrar en cadena usuarios, personas,
            // insumos o facturas ya emitidas. Con Restrict, si el registro está
            // en uso, el borrado se rechaza con un error claro en vez de
            // desaparecer datos en silencio.

            // Usuario -> Rol: borrar un Rol no debe borrar los Usuarios que lo tienen asignado.
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            // Persona -> Localidad: borrar una Localidad no debe borrar clientes/proveedores/usuarios que viven ahí.
            modelBuilder.Entity<Persona>()
                .HasOne(p => p.Localidad)
                .WithMany(l => l.Personas)
                .HasForeignKey(p => p.IdLocalidad)
                .OnDelete(DeleteBehavior.Restrict);

            // Insumo -> Proveedor: borrar un Proveedor no debe borrar su catálogo de Insumos.
            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.Proveedor)
                .WithMany(p => p.Insumos)
                .HasForeignKey(i => i.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            // Maquina -> Cliente: borrar un Cliente no debe borrar sus Maquinas registradas.
            modelBuilder.Entity<Maquina>()
                .HasOne(m => m.Cliente)
                .WithMany(c => c.Maquinas)
                .HasForeignKey(m => m.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Turno -> Cliente: borrar un Cliente no debe borrar sus Turnos.
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Cliente)
                .WithMany(c => c.Turnos)
                .HasForeignKey(t => t.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // FacturaCompra -> Proveedor: borrar un Proveedor no debe borrar su historial de compras.
            modelBuilder.Entity<FacturaCompra>()
                .HasOne(f => f.Proveedor)
                .WithMany(p => p.FacturasCompra)
                .HasForeignKey(f => f.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            // FacturaCompra -> FormaPago: borrar una FormaPago no debe borrar facturas de compra.
            modelBuilder.Entity<FacturaCompra>()
                .HasOne(f => f.FormaPago)
                .WithMany()
                .HasForeignKey(f => f.IdFormaPago)
                .OnDelete(DeleteBehavior.Restrict);

            // FacturaVenta -> FormaPago: borrar una FormaPago no debe borrar facturas de venta.
            modelBuilder.Entity<FacturaVenta>()
                .HasOne(f => f.FormaPago)
                .WithMany()
                .HasForeignKey(f => f.IdFormaPago)
                .OnDelete(DeleteBehavior.Restrict);

            // SesionCaja -> Usuario: borrar un Usuario no debe borrar sus sesiones de caja históricas.
            modelBuilder.Entity<SesionCaja>()
                .HasOne(s => s.Usuario)
                .WithMany(u => u.SesionesCaja)
                .HasForeignKey(s => s.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Turno -> Maquina
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Maquina)
                .WithMany(m => m.Turnos)
                .HasForeignKey(t => t.IdMaquina)
                .OnDelete(DeleteBehavior.NoAction);

            // FacturaVenta -> Turno
            modelBuilder.Entity<FacturaVenta>()
                .HasOne(f => f.Turno)
                .WithOne(t => t.FacturaVenta)
                .HasForeignKey<FacturaVenta>(f => f.IdTurno)
                .OnDelete(DeleteBehavior.NoAction);

            // FacturaVenta -> SesionCaja
            modelBuilder.Entity<FacturaVenta>()
                .HasOne(f => f.SesionCaja)
                .WithMany(s => s.FacturasVenta)
                .HasForeignKey(f => f.IdSesionCaja)
                .OnDelete(DeleteBehavior.NoAction);

            // FacturaCompra -> SesionCaja
            modelBuilder.Entity<FacturaCompra>()
                .HasOne(f => f.SesionCaja)
                .WithMany(s => s.FacturasCompra)
                .HasForeignKey(f => f.IdSesionCaja)
                .OnDelete(DeleteBehavior.NoAction);

            // DetalleFacturaCompra -> FacturaCompra
            modelBuilder.Entity<DetalleFacturaCompra>()
                .HasOne(d => d.FacturaCompra)
                .WithMany(f => f.Detalles)
                .HasForeignKey(d => d.IdFacturaCompra)
                .OnDelete(DeleteBehavior.Cascade);

            // DetalleFacturaCompra -> Insumo
            modelBuilder.Entity<DetalleFacturaCompra>()
                .HasOne(d => d.Insumo)
                .WithMany(i => i.DetallesCompra)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.Restrict);

            // DetalleFacturaVenta -> FacturaVenta
            modelBuilder.Entity<DetalleFacturaVenta>()
                .HasOne(d => d.FacturaVenta)
                .WithMany(f => f.DetallesFacturaVenta)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.Cascade);

            // DetalleFacturaVenta -> TrabajoPorTurno
            modelBuilder.Entity<DetalleFacturaVenta>()
                .HasOne(d => d.TrabajoPorTurno)
                .WithMany(t => t.DetallesVenta)
                .HasForeignKey(d => d.IdTrabajoPorTurno)
                .OnDelete(DeleteBehavior.Restrict);

            // DetalleFacturaVenta -> InsumoPorTrabajo
            modelBuilder.Entity<DetalleFacturaVenta>()
                .HasOne(d => d.InsumoPorTrabajo)
                .WithMany(i => i.DetallesVenta)
                .HasForeignKey(d => d.IdInsumoPorTrabajo)
                .OnDelete(DeleteBehavior.Restrict);

            // InsumoPorTrabajo -> TrabajoPorTurno
            modelBuilder.Entity<InsumoPorTrabajo>()
                .HasOne(i => i.TrabajoPorTurno)
                .WithMany(t => t.InsumosConsumidos)
                .HasForeignKey(i => i.IdTrabajoTurno)
                .OnDelete(DeleteBehavior.Cascade);

            // InsumoPorTrabajo -> Insumo
            modelBuilder.Entity<InsumoPorTrabajo>()
                .HasOne(i => i.Insumo)
                .WithMany(ins => ins.InsumosPorTrabajo)
                .HasForeignKey(i => i.IdInsumo)
                .OnDelete(DeleteBehavior.Restrict);

            // TrabajoPorTurno -> Turno
            modelBuilder.Entity<TrabajoPorTurno>()
                .HasOne(tp => tp.Turno)
                .WithMany(t => t.TrabajosPorTurno)
                .HasForeignKey(tp => tp.IdTurno)
                .OnDelete(DeleteBehavior.Cascade);

            // TrabajoPorTurno -> Trabajo
            modelBuilder.Entity<TrabajoPorTurno>()
                .HasOne(tp => tp.Trabajo)
                .WithMany(t => t.TrabajosPorTurno)
                .HasForeignKey(tp => tp.IdTrabajo)
                .OnDelete(DeleteBehavior.Restrict);

            // TrabajoPorTurno -> Usuario
            modelBuilder.Entity<TrabajoPorTurno>()
                .HasOne(tp => tp.Usuario)
                .WithMany(u => u.TrabajosRealizados)
                .HasForeignKey(tp => tp.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            PrepareIdentityKeysForInsert();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            PrepareIdentityKeysForInsert();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void PrepareIdentityKeysForInsert()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Added)
                {
                    continue;
                }

                var primaryKey = entry.Metadata.FindPrimaryKey();
                if (primaryKey is null || primaryKey.Properties.Count != 1)
                {
                    continue;
                }

                var keyProperty = primaryKey.Properties[0];
                if (keyProperty.ClrType == typeof(int) && keyProperty.ValueGenerated == ValueGenerated.OnAdd)
                {
                    entry.Property(keyProperty.Name).CurrentValue = 0;
                }
            }
        }

        public void DetachTrackedEntity<TEntity>(TEntity entity) where TEntity : class
        {
            var entityId = Entry(entity).Property("Id").CurrentValue;
            var trackedEntity = ChangeTracker.Entries<TEntity>()
                .FirstOrDefault(entry => !ReferenceEquals(entry.Entity, entity)
                    && Equals(entry.Property("Id").CurrentValue, entityId));

            if (trackedEntity is not null)
            {
                trackedEntity.State = EntityState.Detached;
            }
        }
    }
}
