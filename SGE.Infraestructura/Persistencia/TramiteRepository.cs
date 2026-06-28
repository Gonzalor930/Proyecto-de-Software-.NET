using SGE.Dominio.Tramites;
using SGE.Dominio.Repositorios;
using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Tramites;

namespace SGE.Infraestructura.Persistencia;

public class TramiteRepository : ITramiteRepository
{
    private readonly SgeContext _context;
    public TramiteRepository(SgeContext context) => _context = context;

    public Tramite? ObtenerPorId(Guid id) => _context.Tramites.Find(id);

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId) => 
        _context.Tramites.Where(t => t.ExpedienteId == expedienteId).ToList();

    public void Agregar(Tramite tramite) => _context.Tramites.Add(tramite);

    public void Modificar(Tramite tramite) => _context.Tramites.Update(tramite);

    public void Eliminar(Tramite tramite) => _context.Tramites.Remove(tramite);
}