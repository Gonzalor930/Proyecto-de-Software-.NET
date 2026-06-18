using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
//BORRAR COMENTARIOS LUEGO
//using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura;

public class SgeContext : DbContext
{
    // Representan las tablas en la base de datos SQLite
    public DbSet<Expediente> Expedientes { get; set; }
    public DbSet<Tramite> Tramites { get; set; }
//BORRAR COMENTARIOS LUEGO
//    public DbSet<Usuario> Usuarios { get; set; }

    public SgeContext(DbContextOptions<SgeContext> options) : base(options)
    {
        // Inicializa la base de datos si no existe 
        if (this.Database.EnsureCreated())
        {
            // Establecemos la propiedad journal_mode de SQLite en DELETE
            var connection = this.Database.GetDbConnection();
            connection.Open();
            using (var command = connection.CreateCommand())
                command.CommandText = "PRAGMA journal_mode=DELETE;";
//BORRAR COMENTARIOS LUEGO
//                command.ExecuteNonQuery();
            }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expediente>()
            .ComplexProperty(e => e.Caratula);

        modelBuilder.Entity<Tramite>()
            .ComplexProperty(t => t.Contenido);
            
    }
}