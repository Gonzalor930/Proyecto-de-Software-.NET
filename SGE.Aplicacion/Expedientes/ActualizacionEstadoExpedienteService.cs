using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Expedientes
{
    public class ActualizacionEstadoExpedienteService
    {
        private readonly IExpedienteRepository _expedienteRepository;
        private readonly ITramiteRepository _tramiteRepository;
        private readonly IAutorizacionService _autorizacionService;

        public ActualizacionEstadoExpedienteService(
            IExpedienteRepository expedienteRepository, 
            ITramiteRepository tramiteRepository,
            IAutorizacionService autorizacionService) 
        {
            _expedienteRepository = expedienteRepository;
            _tramiteRepository = tramiteRepository;
            _autorizacionService = autorizacionService;
        }
        public void ActualizarEstadoSiEsNecesario(Guid expedienteId, Guid idUsuario)
        {
            // 1. Validamos los permisos del usuario antes de hacer cualquier cosa
            if (!_autorizacionService.PoseeElPermiso(idUsuario, Permiso.ExpedienteModificacion))
            {
                throw new AutorizacionException("No tiene permisos para actualizar el estado del expediente");
            }

            // 2. Buscamos el expediente
            Expediente? expediente = _expedienteRepository.ObtenerPorId(expedienteId);
            if (expediente == null)
            {
                throw new EntidadNoEncontradaException("Expediente no encontrado al intentar actualizar su estado");
            }

            // 3. Traemos los trámites asociados
            IEnumerable<Tramite> tramites = _tramiteRepository.ObtenerPorExpedienteId(expedienteId);

            // 4. Buscamos el último trámite
            Tramite? ultimoTramite = tramites.OrderByDescending(t => t.FechaCreacion).FirstOrDefault();

            EtiquetaTramite? ultimaEtiqueta = ultimoTramite?.Etiqueta;

            // 5. Intentamos actualizar el estado
            bool cambio = expediente.ActualizarEstado(ultimaEtiqueta, idUsuario);

            // 6. Si hubo un cambio real, guardamos en el repositorio
            if (cambio)
            {
                _expedienteRepository.Modificar(expediente); 
            }
        }
    }
}
