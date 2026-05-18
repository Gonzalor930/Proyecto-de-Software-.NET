using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Tramites;

namespace SGE.Aplicacion.Expedientes
{
    public class ActualizacionEstadoExpedienteService
    {
        private readonly IExpedienteRepository _expedienteRepository;
        private readonly ITramiteRepository _tramiteRepository;

        public ActualizacionEstadoExpedienteService(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository)
        {
            _expedienteRepository = expedienteRepository;
            _tramiteRepository = tramiteRepository;
        }

        public void ActualizarEstadoSiEsNecesario(Guid expedienteId, Guid idUsuario)
        {
            Expediente? expediente = _expedienteRepository.ObtenerPorId(expedienteId);
            if (expediente == null)
            {
                // Usamos la excepción nueva
                throw new EntNoEncontradaExp("Expediente no encontrado al intentar actualizar su estado");
            }

            IEnumerable<Tramite> tramites = _tramiteRepository.ObtenerPorExpedienteId(expedienteId);

            Tramite? ultimoTramite = tramites.OrderByDescending(t => t.FechaCreacion).FirstOrDefault();

            EtiquetaTramite? ultimaEtiqueta = ultimoTramite?.Etiqueta;

            bool cambio = expediente.ActualizarEstado(ultimaEtiqueta, idUsuario);

            if (cambio)
            {
                _expedienteRepository.Modificar(expediente); 
            }
        }
    }
}
