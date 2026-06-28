using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;// Para poder usar el ActualizacionEstadoExpedienteService
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Autorizacion;//para usar las excepciones

namespace SGE.Aplicacion.Tramites;

public class BajaTramiteUseCase(
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion,
    ActualizacionEstadoExpedienteService Service,
    IUnidadDeTrabajo uow
    )
{
    public BajaTramiteResponse Ejecutar(BajaTramiteRequest request)
    {
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja))
        {
            throw new AutorizacionException("No tiene permisos para dar de baja tramites.");
        }
        Tramite? tramite = tramiteRepositorio.ObtenerPorId(request.TramiteId);
        if (tramite == null) throw new EntidadNoEncontradaException("El tramite no existe");

        Guid idExpedienteAActualizar = tramite.ExpedienteId;

        tramiteRepositorio.Eliminar(tramite);
        Service.ActualizarEstadoSiEsNecesario(idExpedienteAActualizar, request.IdUsuario);
        uow.Guardar();
        return new BajaTramiteResponse(true); 
    }
}