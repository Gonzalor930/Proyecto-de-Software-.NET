using SGE.Dominio.Usuarios;
using System.Collections.Generic;
using SGE.Dominio.Autorizacion;

namespace SGE.WebApi.Payloads
{
    public record RegistroPayload(string Nombre, string Correo, string ContrasenaPlana);
    public record LoginPayload(string Correo, string ContrasenaPlana);
    public record ModificarMisDatosPayload(string NuevoNombre, string NuevaContrasenaPlana);
    public record ModificarPermisosPayload(List<Permiso> PermisosDeseados);
}