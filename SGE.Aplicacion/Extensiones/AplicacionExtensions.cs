using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;

namespace SGE.Aplicacion;

public static class AplicacionExtensions
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        //Servicios
        services.AddScoped<ActualizacionEstadoExpedienteService>();

        //Casos de usos de expedientes
        services.AddScoped<AltaExpedienteUseCase>();
        services.AddScoped<BajaExpedienteUseCase>();
        services.AddScoped<CambiarEstadoExpedienteUseCase>();
        services.AddScoped<ListarExpedientesUseCase>();
        services.AddScoped<ModificarCaratulaExpedienteUseCase>();

        //Casos de usos de tramites
        services.AddScoped<AgregarTramiteUseCase>();
        services.AddScoped<BajaTramiteUseCase>();
        services.AddScoped<ModificarTramiteUseCase>();
        return services;
    }
}