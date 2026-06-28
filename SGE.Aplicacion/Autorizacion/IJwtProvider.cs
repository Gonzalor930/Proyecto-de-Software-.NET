using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Autorizacion
{
    public interface IJwtProvider
    {
        string GenerarToken(Usuario usuario);
    }
}