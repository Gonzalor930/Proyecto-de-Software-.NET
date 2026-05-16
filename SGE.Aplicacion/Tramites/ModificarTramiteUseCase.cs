using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;// Para poder usar el ActualizacionEstadoExpedienteService
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase(
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion,
    ActualizacionEstadoExpedienteService Service)
{
    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        // 1. Autorización
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion))
        {
            throw new AutorizacionException("No tiene permiso para modificar trámites");
        }
        // 2. Buscamos el trámite
        Tramite tramite = tramiteRepositorio.ObtenerPorId(request.TramiteId);
        if (tramite == null) throw new Exception("El trámite es nulo");
        // 3. Aca pedimos el nuevo contenido y vamos a modificarlo
        ContenidoTramite nuevoContenido = new ContenidoTramite(request.NuevoContenido);
        tramite.ModificarContenido(nuevoContenido, request.IdUsuario);
        // 4. Aca lo modifica
        tramiteRepositorio.Modificar(tramite);
        // 5. Usamos el service para actualizar
        Service.ActualizarEstadoSiEsNecesario(tramite.ExpedienteId, request.IdUsuario);

        return new ModificarTramiteResponse(true); 
    }
}