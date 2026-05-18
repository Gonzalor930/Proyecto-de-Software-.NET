namespace SGE.Aplicacion.Autorizacion;
    public class AutorizacionProvisionalService : IAutorizacionService
    {
        // Devuelve siempre true para que te deje probar todas las opciones del menú
        public bool PoseeElPermiso(Guid idUsuario, Permiso permiso) => true;
    }