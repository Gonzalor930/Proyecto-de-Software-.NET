using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;
using SGE.Dominio.Repositorios;
using SGE.Aplicacion.Excepciones;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListarUsuariosUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public IEnumerable<Usuario> Ejecutar(Guid usuarioEjecutorId)
    {
        var ejecutor = _usuarioRepository.ObtenerPorId(usuarioEjecutorId);
        
        if (ejecutor == null)
            throw new EntidadNoEncontradaException("El usuario ejecutor no existe.");

        if (!ejecutor.EsAdministrador)
            throw new AutorizacionException("Acceso denegado. Se requieren permisos de administrador."); // 

        return _usuarioRepository.ObtenerTodos(); // 
    }
}