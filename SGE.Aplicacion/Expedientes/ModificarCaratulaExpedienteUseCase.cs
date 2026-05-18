using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes;

namespace SGE.Aplicacion.Expedientes;
public class ModificarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _repositorio;
    private readonly IAutorizacionService _autorizacion;

    // Inyectamos las abstracciones por constructor para desacoplar la lógica de negocio
    // de los detalles de infraestructura.
    public ModificarCaratulaExpedienteUseCase(IExpedienteRepository repositorio, IAutorizacionService autorizacion)
    {
        _repositorio = repositorio;
        _autorizacion = autorizacion;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        // 1. Autorización: El Caso de Uso invoca a la interfaz de autorización.
        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar expedientes.");
        }

        // 2. Obtener la entidad: Delegamos el acceso a datos a la interfaz del repositorio.
        Expediente? expediente = _repositorio.ObtenerPorId(request.ExpedienteId);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado."); 
        }

        // 3. Ejecutar comportamiento del Dominio: La lógica de negocio queda en la entidad.
        Caratula nuevaCaratula = new Caratula(request.NuevaCaratula);
        expediente.ModificarCaratula(nuevaCaratula, request.IdUsuario);

        // 4. Persistir: Le avisamos al repositorio que guarde los cambios.
        _repositorio.Modificar(expediente);

        // 5. Retornar el DTO de respuesta.
        return new ModificarCaratulaExpedienteResponse(true);
    }
}