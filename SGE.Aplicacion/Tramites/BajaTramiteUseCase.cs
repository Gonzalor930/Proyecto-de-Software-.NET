using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;// Para poder usar el ActualizacionEstadoExpedienteService

namespace SGE.Aplicacion.Tramites;

public class BajaTramiteUseCase(
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion,
    ActualizacionEstadoExpedienteService actualizacionService)
{
    public BajaTramiteResponse Ejecutar(BajaTramiteRequest request)
    {
        // 1. Autorización
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja))
        {
            throw new AutorizacionException("No tiene permisos para dar de baja trámites.");
        }
        // 2. Buscamos el trámite
        Tramite tramite = tramiteRepositorio.ObtenerPorId(request.TramiteId);
        if (tramite == null) throw new Exception("El trámite no existe.");
        // 2. Aca guardamos el id del exp para cuando vayamos a actualizar el estado, ya que si
        //lo borramos y no guardamos el id no vamos a saber que expediente tenemos que actualizar
        Guid idExpedienteAActualizar = tramite.ExpedienteId;

        // 4. Eliminamos (realizamos la baja)
        tramiteRepositorio.Eliminar(request.TramiteId);

        // 4. Usamos el ID que nos guardamos, para actualizar
        actualizacionService.ActualizarEstadoSiEsNecesario(idExpedienteAActualizar, request.IdUsuario);

        return new BajaTramiteResponse(true); 
    }
}