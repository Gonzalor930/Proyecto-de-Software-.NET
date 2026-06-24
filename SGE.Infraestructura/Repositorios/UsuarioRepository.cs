using SGE.Dominio.Repositorios;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGE.Infraestructura.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SgeContext _context;

        public UsuarioRepository(SgeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Usuario ObtenerPorId(Guid id)
        {
            return _context.Usuarios.Find(id);
        }

        public IEnumerable<Usuario> ObtenerTodos()
        {
            return _context.Usuarios.AsNoTracking().ToList(); //mejor para lecturas
        }

        public void Agregar(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            _context.Usuarios.Add(usuario);
        }

        public void Modificar(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            _context.Usuarios.Update(usuario);
        }

        public void Eliminar(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            _context.Usuarios.Remove(usuario);
        }
    }
}