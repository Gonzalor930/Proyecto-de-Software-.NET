using SGE.Aplicacion.Expedientes;

public record CrearExpedientePayload(string DetalleCaratula);
public record ModificarCaratulaPayload(string NuevaCaratula);
public record CambiarEstadoPayload(int NuevoEstado);