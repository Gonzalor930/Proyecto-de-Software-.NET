using System;
using System.Collections.Generic;
using SGE.Dominio.Autorizacion;

namespace SGE.Dominio.Usuarios
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

        // Constructor para infraestructura y WebAPI
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
        //constructor para la db
        protected Usuario()
        {
            _permisos = new List<Permiso>();
        }
        
        // Constructor para las Seeds
        public Usuario(Guid id, string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador)
        {
            Id = id;
            Nombre = nombre;
            CorreoElectronico = correoElectronico;
            ContrasenaHash = contrasenaHash;
            EsAdministrador = esAdministrador;
            _permisos = new List<Permiso>();
        }
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

       public void ModificarDatos(string nuevoNombre, string nuevaContrasenaHash)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre es obligatorio.");
        
            if (string.IsNullOrWhiteSpace(nuevaContrasenaHash))
                throw new ArgumentException("El hash de la contraseña no puede ser nulo o vacio.");

            Nombre = nuevoNombre;
            ContrasenaHash = nuevaContrasenaHash;
        }
        public bool ValidarContrasena(string hashAComparar)
        {
            return ContrasenaHash == hashAComparar;
        }
    }
}