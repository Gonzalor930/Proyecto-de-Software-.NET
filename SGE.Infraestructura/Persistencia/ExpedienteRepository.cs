using SGE.Dominio.Expedientes;
using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
namespace SGE.Infraestructura.Persistencia;

public class ExpedienteRepository : IExpedienteRepository
{
    private readonly SgeContext _context;
    public ExpedienteRepository(SgeContext context) => _context = context;

    public Expediente? ObtenerPorId(Guid id) => _context.Expedientes.FirstOrDefault(e => e.Id == id);
    public IEnumerable<Expediente> ObtenerTodos() => _context.Expedientes.AsNoTracking().ToList();

    public void Agregar(Expediente expediente) => _context.Expedientes.Add(expediente);

    public void Modificar(Expediente expediente) => _context.Expedientes.Update(expediente);

    public void Eliminar(Expediente expediente) => _context.Expedientes.Remove(expediente);
}