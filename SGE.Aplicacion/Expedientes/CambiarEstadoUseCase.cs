using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repositorio)
{
    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        var expediente = repositorio.ObtenerPorId(request.ExpedienteId);
        if(expediente == null) return new CambiarEstadoExpedienteResponse(false);
        
        var nuevoEstado = (EstadoExpediente)request.NuevoEstado;
        expediente.CambiarEstado(nuevoEstado, request.IdUsuario);
        
        repositorio.Modificar(expediente);
        return new CambiarEstadoExpedienteResponse(true);
    }
}