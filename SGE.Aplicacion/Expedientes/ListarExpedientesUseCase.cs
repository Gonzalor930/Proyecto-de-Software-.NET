using System.Collections.Generic;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes{
    public class ListarExpedientesUseCase
    {
        private readonly IExpedienteRepository _repositorio;
        public ListarExpedientesUseCase(IExpedienteRepository repositorio)
        {
            _repositorio = repositorio;
        }
        public ListarExpedientesResponse Ejecutar()
        {
            //Busca la entidad en el repositorio
            IEnumerable<Expediente> expedientesDominio = _repositorio.ObtenerTodos();

            List<ExpedienteDTO> listaDtos = new List<ExpedienteDTO>();
            foreach (var exp in expedientesDominio)
            {
                ExpedienteDTO dto = new ExpedienteDTO(
                    exp.Id,
                    exp.Caratula.ToString(),
                    exp.FechaCreacion,
                    exp.FechaUltimaModificacion,
                    exp.Estado.ToString()
                );
                listaDtos.Add(dto);
            }
            return new ListarExpedientesResponse(listaDtos);
        }
    }
}