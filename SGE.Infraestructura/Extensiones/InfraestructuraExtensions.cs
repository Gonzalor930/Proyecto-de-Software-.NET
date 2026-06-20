using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Infraestructura.Persistencia;
using SGE.Infraestructura.Seguridad;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura;

public static class InfraestructuraExtensions
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services)
    {
        //Configuramos la base de datos SQLite
        services.AddDbContext<SgeContext>(options => options.UseSqlite("Data Source=SGE.sqlite"));
        //Persistencia - En teoria habria que cambiar los txt por eso tira error por ahora
        services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
        services.AddScoped<ITramiteRepository, TramiteRepository>();

        //Unidad de trabajo (unit of work) - falta crear la unidad de trabajo
        services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

        //Seguridad - 
        services.AddScoped<IAutorizacionService, AutorizacionService>();

        return services;
    }
}