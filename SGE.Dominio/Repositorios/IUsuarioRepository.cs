using SGE.Dominio.Usuarios;

namespace SGE.Dominio.Repositorios;
public interface IUsuarioRepository
{
    Usuario? ObtenerPorId(Guid id);
    IEnumerable<Usuario> ObtenerTodos();
    void Agregar(Usuario usuario);
    void Modificar(Usuario usuario);
    void Eliminar(Usuario usuario);
    Usuario? ObtenerPorCorreo(string correo);
}