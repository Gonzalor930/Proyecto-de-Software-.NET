using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.GestionUsuarios
{
    public record RegistrarUsuarioRequest(string Nombre, string Correo, string ContrasenaPlana);
    public record LoginRequest(string Correo, string ContrasenaPlana);
    public record LoginResponse(string Token);
    public record RegistrarUsuarioResponse(Guid UsuarioId);
    public record ModificarMisDatosRequest(Guid IdUsuarioAutenticado, Guid IdUsuarioAModificar, string NuevoNombre, string NuevaContrasenaPlana);
    public record ModificarMisDatosResponse(bool Exito);
}