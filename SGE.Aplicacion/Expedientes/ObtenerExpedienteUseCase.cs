using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Excepciones;

namespace SGE.Aplicacion.Expedientes;

public class ObtenerExpedienteUseCase(
    IExpedienteRepository expedienteRepo, 
    ITramiteRepository tramiteRepo
    )
{
    public ObtenerExpedienteResponse Ejecutar(ObtenerExpedienteRequest request)
    {
        Expediente? exp = expedienteRepo.ObtenerPorId(request.ExpedienteId);
        if(exp == null) throw new EntidadNoEncontradaException("Expediente no encontrado");

        var expDto = new ExpedienteDTO(
            exp.Id,
            exp.Caratula.Valor,
            exp.FechaCreacion,
            exp.FechaUltimaModificacion,
            exp.Estado.ToString()
        );
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
        return new ObtenerExpedienteResponse(expDto, listaDtos);
    }
}