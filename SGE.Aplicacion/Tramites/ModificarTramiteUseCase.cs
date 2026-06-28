using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase(
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion,
    ActualizacionEstadoExpedienteService Service,
    IUnidadDeTrabajo uow
    )
{
    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion))
        {
            throw new AutorizacionException("No tiene permiso para modificar tramites");
        }

        Tramite? tramite = tramiteRepositorio.ObtenerPorId(request.TramiteId);
        if (tramite == null) throw new EntidadNoEncontradaException("El tramite es nulo");

        ContenidoTramite nuevoContenido = new ContenidoTramite(request.NuevoContenido);
        tramite.ModificarContenido(nuevoContenido, request.IdUsuario);

        tramiteRepositorio.Modificar(tramite);

        Service.ActualizarEstadoSiEsNecesario(tramite.ExpedienteId, request.IdUsuario);
        uow.Guardar();
        return new ModificarTramiteResponse(true); 
    }
}