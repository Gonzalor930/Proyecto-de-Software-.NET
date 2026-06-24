using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Repositorios;
public class EliminarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public void Ejecutar(Guid usuarioEjecutorId, Guid usuarioAEliminarId)
    {
        var ejecutor = _usuarioRepository.ObtenerPorId(usuarioEjecutorId);
        
        if (ejecutor == null)
            throw new EntidadNoEncontradaException("El usuario ejecutor no existe.");

        if (!ejecutor.EsAdministrador)
            throw new AutorizacionException("Acceso denegado se necesita permisos de administrador."); // 

        var usuarioAEliminar = _usuarioRepository.ObtenerPorId(usuarioAEliminarId);
        
        if (usuarioAEliminar == null)
            throw new EntidadNoEncontradaException("El usuario que se desea eliminar no existe.");

        _usuarioRepository.Eliminar(usuarioAEliminar);
        _unidadDeTrabajo.Guardar();
    }
}