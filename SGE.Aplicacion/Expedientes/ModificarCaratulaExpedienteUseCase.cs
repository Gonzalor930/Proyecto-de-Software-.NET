using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Expedientes;
public class ModificarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _repositorio;
    private readonly IAutorizacionService _autorizacion;
    private readonly IUnidadDeTrabajo _uow;
    public ModificarCaratulaExpedienteUseCase(IExpedienteRepository repositorio, IAutorizacionService autorizacion, IUnidadDeTrabajo uow)
    {
        _repositorio = repositorio;
        _autorizacion = autorizacion;
        _uow = uow;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar expedientes.");
        }

        Expediente? expediente = _repositorio.ObtenerPorId(request.ExpedienteId);
        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("Expediente no encontrado."); 
        }

        Caratula nuevaCaratula = new Caratula(request.NuevaCaratula);
        expediente.ModificarCaratula(nuevaCaratula, request.IdUsuario);

        _repositorio.Modificar(expediente);
        _uow.Guardar();
        return new ModificarCaratulaExpedienteResponse(true);
    }
}