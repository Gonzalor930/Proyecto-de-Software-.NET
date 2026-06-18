using System;
using System.Collections.Generic;
using SGE.Dominio.Autorizacion;

namespace SGE.Dominio.Usuario
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string CorreoElectronico { get; private set; }
        public string ContrasenaHash { get; private set; }
        public bool EsAdministrador { get; private set; }

        private readonly List<Permiso> _permisos;

        public IReadOnlyCollection<Permiso> Permisos => _permisos.AsReadOnly();

        public Usuario(string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador = false)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio");
            
            if (string.IsNullOrWhiteSpace(correoElectronico))
                throw new ArgumentException("El correo electronico es obligatorio");
            
            if (string.IsNullOrWhiteSpace(contrasenaHash))
                throw new ArgumentException("El hash de la contraseña no puede ser nulo o vacío");

            Id = Guid.NewGuid();
            Nombre = nombre;
            CorreoElectronico = correoElectronico;
            ContrasenaHash = contrasenaHash;
            EsAdministrador = esAdministrador;
            _permisos = new List<Permiso>();
        }

        // Metodos publicos para la asignación de permisos de forma segura
        public void AsignarPermiso(Permiso permiso)
        {
            if (!_permisos.Contains(permiso))
            {
                _permisos.Add(permiso);
            }
        }

        public void RemoverPermiso(Permiso permiso)
        {
            if (_permisos.Contains(permiso))
            {
                _permisos.Remove(permiso);
            }
        }
    }
}