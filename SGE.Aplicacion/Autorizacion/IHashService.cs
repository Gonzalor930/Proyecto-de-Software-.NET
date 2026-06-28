namespace SGE.Aplicacion.Autorizacion
{
    public interface IHashService
    {
        string HashearPassword(string password);
    }
}