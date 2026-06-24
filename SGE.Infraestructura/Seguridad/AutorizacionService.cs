using SGE.Aplicacion.Autorizacion; // Asumiendo que IAutorizacionService está en SGE.Aplicacion.Servicios
using SGE.Dominio.Usuarios;
using SGE.Dominio.Repositorios;
using System;
using System.Linq;
using SGE.Dominio.Autorizacion;

namespace SGE.Infraestructura.Servicios
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutorizacionService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }
        public bool PoseeElPermiso(Guid usuarioId, Permiso permisoRequerido)
        {
            var usuario = _usuarioRepository.ObtenerPorId(usuarioId);
            
            if (usuario == null) return false;

            if (usuario.EsAdministrador) return true;

            var permisos = usuario.Permisos;

            if (permisos.Contains(permisoRequerido)) return true;

            if (permisoRequerido == Permiso.TramiteBaja && permisos.Contains(Permiso.ExpedienteBaja))
            {
                return true;
            }

            return false;
        }
    }
}