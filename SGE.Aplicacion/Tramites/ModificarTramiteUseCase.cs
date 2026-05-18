using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;// Para poder usar el ActualizacionEstadoExpedienteService
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Excepciones;//para usar las excepciones

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
        Tramite? tramite = tramiteRepositorio.ObtenerPorId(request.TramiteId);
<<<<<<< HEAD
        if (tramite == null) throw new Exception("El trámite es nulo");
=======
        if (tramite == null) throw new EntNoEncontradaExp("El trámite es nulo");

>>>>>>> e327cdfa1b750f97eb61da8b66d1ca85315bc470
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