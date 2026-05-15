using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repositorio, IAutorizacionService autorizacion)
{
    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        
        if (!autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("No tiene permisos para crear expedientes.");
        }

        var caratulaNueva = new Caratula(request.DetalleCaratula);
        var nuevoExpediente = new Expediente(caratulaNueva, request.IdUsuario);

        repositorio.Agregar(nuevoExpediente);

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }
}