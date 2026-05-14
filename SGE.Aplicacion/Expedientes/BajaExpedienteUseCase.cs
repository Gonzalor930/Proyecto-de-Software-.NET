
namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(IExpedienteRepository repositorio)
{
    public BajaExpedienteResponse Ejecutar(BajaExpedienteRequest request)
    {
        var expediente = repositorio.ObtenerPorId(request.ExpedienteId);
        
        if (expediente == null) return new BajaExpedienteResponse(false);
        
        repositorio.Eliminar(request.ExpedienteId);
        return new BajaExpedienteResponse(true);
    }
}