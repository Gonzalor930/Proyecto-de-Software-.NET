using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Excepciones;

namespace SGE.Aplicacion.GestionUsuarios;

public class LoginUseCase(IUsuarioRepository repo, IJwtProvider jwtProvider)
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        // 1. Buscamos al usuario por correo
        var usuario = repo.ObtenerPorCorreo(request.Correo); 
        // Si no Existe, devolvemos una excepcion
        if (usuario == null)
        {
            throw new EntidadNoEncontradaException("Usuario no encontrado"); 
        }
        // 2. Validamos la contraseña 
        if (!usuario.ValidarContrasena(request.ContrasenaPlana))
        {
            throw new EntidadNoEncontradaException("Credenciales incorrectas");
        }
        // 3. Generamos el token JWT
        string token = jwtProvider.GenerarToken(usuario);
        return new LoginResponse(token);
    }
}