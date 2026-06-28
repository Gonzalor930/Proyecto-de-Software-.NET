using SGE.Dominio.Usuarios;
using SGE.Dominio.Repositorios;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Autorizacion; // Asegúrate de incluir el namespace donde pusiste tus interfaces

namespace SGE.Aplicacion.GestionUsuarios
{
    public class LoginUseCase
    {
        private readonly IUsuarioRepository _repo;
        private readonly IHashService _hashService;
        private readonly IJwtProvider _jwtProvider;

        public LoginUseCase(IUsuarioRepository repo, IHashService hashService, IJwtProvider jwtProvider)
        {
            _repo = repo;
            _hashService = hashService;
            _jwtProvider = jwtProvider;
        }

        public LoginResponse Ejecutar(LoginRequest request)
        {
            var usuario = _repo.ObtenerPorCorreo(request.Correo); 
            
            if (usuario == null)
            {
                throw new EntidadNoEncontradaException("Usuario no encontrado"); 
            }

            string hashIngresado = _hashService.HashearPassword(request.ContrasenaPlana);

            if (!usuario.ValidarContrasena(hashIngresado))
            {
                throw new EntidadNoEncontradaException("Credenciales incorrectas");
            }

            string token = _jwtProvider.GenerarToken(usuario);
            return new LoginResponse(token);
        }
    }
}