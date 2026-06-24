using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Excepciones;
namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repositorio, IAutorizacionService autorizacion, IUnidadDeTrabajo uow)
{
    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("No tiene permisos para modificar expedientes");
        }

        Expediente? expediente = repositorio.ObtenerPorId(request.ExpedienteId);
        if (expediente == null) throw new EntidadNoEncontradaException("El expediente no existe");
        
        
        var nuevoEstado = (EstadoExpediente)request.NuevoEstado;
        expediente.CambiarEstado(nuevoEstado, request.IdUsuario);
        
        repositorio.Modificar(expediente);
        uow.Guardar();
        return new CambiarEstadoExpedienteResponse(true);
    }
}