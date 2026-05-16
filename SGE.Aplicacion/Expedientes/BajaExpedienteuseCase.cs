using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(
    IExpedienteRepository repositorio, 
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion)
{
    public BajaExpedienteResponse Ejecutar(BajaExpedienteRequest request)
    {
        
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("No tiene permisos para eliminar expedientes.");
        }

        
        Expediente? expediente = repositorio.ObtenerPorId(request.ExpedienteId);
        if (expediente == null) throw new Exception("El expediente no existe");
        
        
        Tramite tramites = tramiteRepositorio.ObtenerPorExpedienteId(request.ExpedienteId);
        foreach (var tramite in tramites)
        {
            tramiteRepositorio.Eliminar(tramite.id);
        }

        // 4. Eliminación del expediente
        repositorio.Eliminar(request.ExpedienteId);
        
        return new BajaExpedienteResponse(true);
    }
}