using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Repositorios;

namespace SGE.Aplicacion.GestionUsuarios{
public class ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo unidadDeTrabajo)
{
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request)
    {
        // 1.El UserId del token debe coincidir con el que se modifica
        if (request.IdUsuarioAutenticado != request.IdUsuarioAModificar)
        {
            throw new AutorizacionException("Permiso denegado para editar los datos del usuario");
        }
        // 2. Buscamos al usuario real
        var usuario = repo.ObtenerPorId(request.IdUsuarioAModificar);
        if (usuario == null)
        {
            throw new EntidadNoEncontradaException("El usuario no existe");
        }
        // 3. Modificamos los datos
        usuario.ModificarDatos(request.NuevoNombre, request.NuevaContrasenaPlana);

        // 4. Marcamos la modificación y guardamos los cambios
        repo.Modificar(usuario);
        unidadDeTrabajo.Guardar(); 

        return new ModificarMisDatosResponse(true);
    }
}
}