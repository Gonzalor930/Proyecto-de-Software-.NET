namespace SGE.Aplicacion.Autorizacion
{
    public interface IAutorizacionService
    {
        bool PoseeElPermiso(int idUsuario, Permiso permiso);
    }
}