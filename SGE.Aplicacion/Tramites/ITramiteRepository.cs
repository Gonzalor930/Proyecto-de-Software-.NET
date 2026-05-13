namespace SGE.Aplicacion.Tramites
{
    public interface ITramiteRepository
    {
        void Agregar(Tramite tramite);
        void Modificar(Tramite tramite);
        void Eliminar(int tramiteId);
        Tramite? ObtenerPorId(int tramiteId);
        List<Tramite> ObtenerPorExpedienteId(int expedienteId);
    }
}