using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.CasosDeUso;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.GestionUsuarios;
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
        services.AddScoped<ObtenerExpedienteUseCase>();

        //Casos de usos de tramites
        services.AddScoped<AgregarTramiteUseCase>();
        services.AddScoped<BajaTramiteUseCase>();
        services.AddScoped<ModificarTramiteUseCase>();
        services.AddScoped<ListarTramitesPorExpedienteUseCase>();

        //Casos de usos de Gestion de Usuarios
        services.AddScoped<EliminarUsuarioUseCase>();
        services.AddScoped<ListarUsuariosUseCase>();
        services.AddScoped<ModificarMisDatosUseCase>();
        services.AddScoped<ModificarPermisosUsuarioUseCase>();
        services.AddScoped<RegistrarUsuarioUseCase>();
        services.AddScoped<LoginUseCase>();

        return services;
    }
}