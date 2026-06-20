
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class ListarTramitesPorExpedienteUseCase(
    ITramiteRepository tramiteRepo
    )
{
    public ListarTramitesPorExpedienteResponse Ejecutar(ListarTramitesPorExpedienteRequest request)
    {
        //traemos todos los tramites asociados al expediente
        IEnumerable<Tramite> tramites = tramiteRepo.ObtenerPorExpedienteId(request.ExpedienteId);
        List<TramiteDTO> listaDtos = new List<TramiteDTO>();
        foreach (var tmt in tramites)
        {
            TramiteDTO dto = new TramiteDTO(
                tmt.id,
                tmt.ExpedienteId,
                tmt.Etiqueta.ToString(),
                tmt.Contenido.Valor,
                tmt.FechaCreacion,
                tmt.FechaUltimaModificacion,
                tmt.UsuarioUltimoCambio
            );
            listaDtos.Add(dto);
        }
        return new ListarTramitesPorExpedienteResponse(listaDtos);
    }
}