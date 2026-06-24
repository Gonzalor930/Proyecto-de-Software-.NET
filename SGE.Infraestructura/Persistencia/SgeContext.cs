using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Usuarios;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Seguridad;
using System;
using System.Collections.Generic;
using System.Text.Json;
using SGE.Dominio.Autorizacion;

namespace SGE.Infraestructura.Persistencia
{
    public class SgeContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<Tramite> Tramites { get; set; }

        public SgeContext(DbContextOptions<SgeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configuracion de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                
                entity.Property(u => u.Permisos)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                          v => JsonSerializer.Deserialize<List<Dominio.Autorizacion.Permiso>>(v, (JsonSerializerOptions)null) ?? new List<Permiso>())
                      .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            //Configuracion de expediente
            modelBuilder.Entity<Expediente>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                //mapeo del Value Object
                entity.ComplexProperty(e => e.Caratula); 
            });

            //Configuracion de Tramite
            modelBuilder.Entity<Tramite>(entity =>
            {
                entity.HasKey(t => t.id);
                entity.ComplexProperty(t => t.Contenido, pr =>
                {
                    pr.Property(c => c.Valor).HasColumnName("ContenidoValor");
                });
                entity.Property(t => t.Etiqueta)
                      .HasConversion<string>(); 
            });

            //inyeccion de Datos Semilla
            SeedUsuarios(modelBuilder);
        }

        private void SeedUsuarios(ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var admin = new Usuario(adminId, "Administrador del Sistema", "admin@sge.com", HashHelper.HashearPassword("admin123"), true); 
            
            var prueba1Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var prueba1 = new Usuario(prueba1Id, "Usuario Básico", "basico@sge.com", HashHelper.HashearPassword("1234"), false);

            var prueba2Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var prueba2 = new Usuario(prueba2Id, "Usuario Avanzado", "avanzado@sge.com", HashHelper.HashearPassword("1234"), false);

            modelBuilder.Entity<Usuario>().HasData(admin, prueba1, prueba2);
        }
    }
}