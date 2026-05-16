using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repositorio, IAutorizacionService autorizacion)
{
    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("No tiene permisos para modificar expedientes");
        }

        Expediente? expediente = repositorio.ObtenerPorId(request.ExpedienteId);
        if (expediente == null) throw new Exception("El expediente no existe");
        
        
        var nuevoEstado = (EstadoExpediente)request.NuevoEstado;
        expediente.CambiarEstado(nuevoEstado, request.IdUsuario);
        
        repositorio.Modificar(expediente);
        return new CambiarEstadoExpedienteResponse(true);
    }
}