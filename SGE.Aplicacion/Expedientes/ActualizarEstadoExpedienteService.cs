using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Expedientes
{
    public class ActualizacionEstadoExpedienteService
    {
        private readonly IExpedienteRepository _expedienteRepository;
        private readonly ITramiteRepository _tramiteRepository;

        // APLICANDO INVERSIÓN DE DEPENDENCIAS: 
        // Inyectamos los contratos, no las implementaciones concretas.
        public ActualizacionEstadoExpedienteService(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository)
        {
            _expedienteRepository = expedienteRepository;
            _tramiteRepository = tramiteRepository;
        }

        public void ActualizarEstadoSiEsNecesario(Guid expedienteId, Guid idUsuario)
        {
            //1. Recuperará el expediente 
            Expediente? expediente = _expedienteRepository.ObtenerPorId(expedienteId);
            if (expediente == null)
            {
                throw new Exception("Expediente no encontrado al intentar actualizar su estado");
            }

           // 2. Buscará todos sus trámites para determinar cuál es el "último" 
            Tramite tramites = _tramiteRepository.ObtenerPorExpedienteId(expedienteId);

            // Ordenamos por fecha de creación descendente para obtener el más reciente (el "último")
            Tramite ultimoTramite = tramites.OrderByDescending(t => t.FechaCreacion).FirstOrDefault();

           // 3. Extraerá la etiqueta de dicho trámite. Si no hay trámites, queda en null.
            EtiquetaTramite? ultimaEtiqueta = ultimoTramite?.Etiqueta;

            // 4. Le pedirá a la entidad que evalúe su nuevo estado
            bool cambio = expediente.ActualizarEstado(ultimaEtiqueta, idUsuario);

            // 5. Si retorna true llama a modificar(exp)
            if (cambio)
            {
                _expedienteRepository.Modificar(expediente); 
            }
        }
    }
}