using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Repositorios;

namespace SGE.Aplicacion.GestionUsuarios{
public class ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo unidadDeTrabajo)
{
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request)
    {
        if (request.IdUsuarioAutenticado != request.IdUsuarioAModificar)
        {
            throw new AutorizacionException("Permiso denegado para editar los datos del usuario");
        }
        var usuario = repo.ObtenerPorId(request.IdUsuarioAModificar);
        if (usuario == null)
        {
            throw new EntidadNoEncontradaException("El usuario no existe");
        }
        usuario.ModificarDatos(request.NuevoNombre, request.NuevaContrasenaPlana);

        repo.Modificar(usuario);
        unidadDeTrabajo.Guardar(); 

        return new ModificarMisDatosResponse(true);
    }
}
}