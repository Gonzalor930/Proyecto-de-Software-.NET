using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repositorio, IAutorizacionService autorizacion)
{
    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("No tiene permisos para crear expedientes");
        }

        Caratula caratulaNueva = new Caratula(request.DetalleCaratula);
        Expediente nuevoExpediente = new Expediente(caratulaNueva, request.IdUsuario);

        repositorio.Agregar(nuevoExpediente);

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }
}