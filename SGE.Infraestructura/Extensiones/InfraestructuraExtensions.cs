using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SGE.Dominio.Repositorios;
using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Infraestructura.Persistencia;
using SGE.Infraestructura.Repositorios;
using SGE.Infraestructura.Servicios;
using System;
using SGE.Infraestructura.Seguridad;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Expedientes;

namespace SGE.Infraestructura.Extensiones
{
    public static class InfraestructuraExtensions
    {
        public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SgeContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SgeDatabase")));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
            services.AddScoped<ITramiteRepository, TramiteRepository>();
            services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
            services.AddScoped<IAutorizacionService, AutorizacionService>();
            services.AddSingleton<IHashService, HashService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            
            return services;
        }

        public static void InicializarBaseDeDatos(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SgeContext>();

            if (context.Database.EnsureCreated())
            {
                var connection = context.Database.GetDbConnection();
                connection.Open();
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "PRAGMA journal_mode=DELETE;";
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}