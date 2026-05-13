namespace SGE.Aplicacion.Expedientes
{
    public interface IExpedienteRepository
    {
        void Agregar(Expediesnte expediente);
        void Modificar(Expediente expediente);
        void Eliminar(Guid expedienteId);
        Expediente? ObtenerPorId(Guid expedienteId);
        IEnumerable<Expediente> ObtenerTodos();
    }
}