using SGE.Aplicacion;
using SGE.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using System;

namespace SGE.Infraestructura.Repositorios
{
    public class UnidadDeTrabajo : IUnidadDeTrabajo
    {
        private readonly SgeContext _context;

        public UnidadDeTrabajo(SgeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Guardar()
        {
            _context.SaveChanges();
        }
    }
}