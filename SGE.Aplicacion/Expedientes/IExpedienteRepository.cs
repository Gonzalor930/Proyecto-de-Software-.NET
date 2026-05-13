namespace SGE.Aplicacion.Expedientes
{
    public interface IExpedienteRepository
    {
        void Agregar(Expediente expediente);
        void Modificar(Expediente expediente);
        void Eliminar(int expedienteId);
        Expediente? ObtenerPorId(int expedienteId);
        IEnumerable<Expediente> ObtenerTodos();
    }
}