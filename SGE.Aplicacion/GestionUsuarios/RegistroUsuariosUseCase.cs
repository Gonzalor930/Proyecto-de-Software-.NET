using SGE.Dominio.Usuarios;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Repositorios;

namespace SGE.Aplicacion.GestionUsuarios;

public class RegistrarUsuarioUseCase(IUsuarioRepository repo, IUnidadDeTrabajo unidadDeTrabajo)
{
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        var usuarioExistente = repo.ObtenerPorCorreo(request.Correo);
        if (usuarioExistente != null)
        {
            throw new DominioException("El correo electrónico ya se encuentra registrado");
        }
        Usuario nuevoUsuario = new Usuario(request.Nombre, request.Correo, request.ContrasenaPlana);
        repo.Agregar(nuevoUsuario);
        unidadDeTrabajo.Guardar();
        return new RegistrarUsuarioResponse(nuevoUsuario.Id);
    }
}