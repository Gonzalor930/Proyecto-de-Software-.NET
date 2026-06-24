using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes; // Para poder usar el ActualizacionEstadoExpedienteService
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class AgregarTramiteUseCase(
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion,
    ActualizacionEstadoExpedienteService Service,
    IUnidadDeTrabajo uow
    )
{
    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        // 1. Autorización
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta))
        {
            throw new AutorizacionException("No tiene permisos para dar de alta trámites.");
        }

        ContenidoTramite contenidoNuevo = new ContenidoTramite(request.Contenido);
        Tramite nuevoTramite = new Tramite(
            request.ExpedienteId, 
            (EtiquetaTramite)request.Etiqueta, 
            contenidoNuevo, 
            request.IdUsuario);
        tramiteRepositorio.Agregar(nuevoTramite);

        // 3.Aca usamos el service que esta en expediente
        Service.ActualizarEstadoSiEsNecesario(request.ExpedienteId, request.IdUsuario);
        uow.Guardar();
        return new AgregarTramiteResponse(nuevoTramite.id);
    }
}