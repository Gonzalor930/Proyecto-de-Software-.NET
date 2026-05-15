using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repositorio)
{
    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        var caratulaNueva = new Caratula(request.DetalleCaratula);
        var nuevoExpediente = new Expediente(caratulaNueva, request.IdUsuario);

        repositorio.Agregar(nuevoExpediente);

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }
}