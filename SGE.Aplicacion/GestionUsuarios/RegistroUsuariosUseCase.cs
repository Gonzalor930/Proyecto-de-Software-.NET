using SGE.Dominio.Usuarios;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Repositorios;

namespace SGE.Aplicacion.GestionUsuarios;

public class RegistrarUsuarioUseCase(IUsuarioRepository repo, IUnidadDeTrabajo unidadDeTrabajo)
{
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        // 1. Verificamos que el correo no exista
        var usuarioExistente = repo.ObtenerPorCorreo(request.Correo);
        if (usuarioExistente != null)
        {
            throw new DominioException("El correo electrónico ya se encuentra registrado");
        }
        //2. Creamos el usuario 
        Usuario nuevoUsuario = new Usuario(request.Nombre, request.Correo, request.ContrasenaPlana);
        //3. Lo agregamos a la memoria del repositorio
        repo.Agregar(nuevoUsuario);
        //4. Confirmamos los cambios en la base de datos de forma atómica
        unidadDeTrabajo.Guardar();
        return new RegistrarUsuarioResponse(nuevoUsuario.Id);
    }
}