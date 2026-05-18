using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura.Seguridad
{
    public class AutorizacionService : IAutorizacionService
    {
        public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
        {
            Guid idAdmin = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Guid idEmpleado = Guid.Parse("22222222-2222-2222-2222-222222222222");

            if (idUsuario == idAdmin)
            {
                return true;
            }

            if (idUsuario == idEmpleado)
            {
                if (permiso == Permiso.ExpedienteBaja || permiso == Permiso.TramiteBaja)
                {
                    return false; 
                }
                return true;
            }

            return false;
        }
    }
}