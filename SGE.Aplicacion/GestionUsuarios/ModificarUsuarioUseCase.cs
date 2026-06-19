using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Repositorios;

namespace SGE.Aplicacion.CasosDeUso
{
    public class ModificarPermisosUsuarioUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ModificarPermisosUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _usuarioRepository = usuarioRepository;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Ejecutar(Guid usuarioEjecutorId, Guid usuarioAModificarId, List<Permiso> permisosDeseados)
        {
            var ejecutor = _usuarioRepository.ObtenerPorId(usuarioEjecutorId);
            
            if (ejecutor == null)
                throw new EntidadNoEncontradaException("El usuario ejecutor no existe.");
            if (!ejecutor.EsAdministrador)
                throw new AutorizacionException("Acceso denegado. Se requieren privilegios de administrador.");

            var usuarioAModificar = _usuarioRepository.ObtenerPorId(usuarioAModificarId);

            if (usuarioAModificar == null)
                throw new EntidadNoEncontradaException("El usuario a modificar no existe.");
                
            var permisosARemover = usuarioAModificar.Permisos.Except(permisosDeseados).ToList();

            foreach (var permiso in permisosARemover)
            {
                usuarioAModificar.RemoverPermiso(permiso);
            }
            var permisosAAsignar = permisosDeseados.Except(usuarioAModificar.Permisos).ToList();

            foreach (var permiso in permisosAAsignar)
            {
                usuarioAModificar.AsignarPermiso(permiso);
            }
            _usuarioRepository.Modificar(usuarioAModificar);
            _unidadDeTrabajo.Guardar();
        }
    }
}