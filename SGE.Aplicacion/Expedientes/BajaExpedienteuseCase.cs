using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(
    IExpedienteRepository repositorio, 
    ITramiteRepository tramiteRepositorio, 
    IAutorizacionService autorizacion,
    IUnidadDeTrabajo uow
    )
{
    public BajaExpedienteResponse Ejecutar(BajaExpedienteRequest request)
    {
        
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("No tiene permisos para eliminar expedientes.");
        }

        Expediente? expediente = repositorio.ObtenerPorId(request.ExpedienteId);
        
        if (expediente == null) throw new EntidadNoEncontradaException("El expediente no existe");
        
        //Nos devuelve todos los tramites de ese expediente que tenemos que hacer la baja
        IEnumerable<Tramite> tramites = tramiteRepositorio.ObtenerPorExpedienteId(request.ExpedienteId);
        
        foreach (var tramite in tramites)
        {
            tramiteRepositorio.Eliminar(tramite);
        }

        repositorio.Eliminar(expediente);
        uow.Guardar();
        return new BajaExpedienteResponse(true);
    }   
}