using System;
using System.Collections.Generic;
using System.IO;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites; 
using SGE.Infraestructura.Comun;

namespace SGE.Infraestructura.Persistencia{
    public class TramiteTxtRepository : ITramiteRepository
    {
        private readonly string _nombreArchivo = "tramites.txt";

        public void Agregar(Tramite tramite)
        {
            string linea = $"{tramite.id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido.Valor}|{tramite.FechaCreacion:O}|{tramite.FechaUltimaModificacion:O}|{tramite.UsuarioUltimoCambio}";
            File.AppendAllLines(_nombreArchivo, new[] { linea });
        }

        public IEnumerable<Tramite> ObtenerTodos()
        {
            if (!File.Exists(_nombreArchivo))
            {
                return new List<Tramite>();
            }

            List<Tramite> lista = new List<Tramite>();
            string[] lineas = File.ReadAllLines(_nombreArchivo);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                string[] datos = linea.Split('|');

                Guid idRecuperado = Guid.Parse(datos[0]);
                Guid expedienteId = Guid.Parse(datos[1]);
                EtiquetaTramite etiqueta = (EtiquetaTramite)Enum.Parse(typeof(EtiquetaTramite), datos[2]);
                ContenidoTramite contenidoObj = new ContenidoTramite(datos[3]);
                
                DateTime fechaCreacion = DateTime.Parse(datos[4]);
                DateTime fechaModif = DateTime.Parse(datos[5]);
                Guid usuarioId = Guid.Parse(datos[6]);

                Tramite tramiteReconstruido = Tramite.Reconstruir(idRecuperado, expedienteId, etiqueta, contenidoObj, fechaCreacion, fechaModif, usuarioId);
                lista.Add(tramiteReconstruido);
            }

            return lista;
        }

        public Tramite? ObtenerPorId(Guid id)
        {
            foreach (var tramite in ObtenerTodos())
            {
                if (tramite.id == id)
                {
                    return tramite;
                }
            }
            return null;
        }

        public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
        {
            List<Tramite> filtrados = new List<Tramite>();
            foreach (var tramite in ObtenerTodos())
            {
                if (tramite.ExpedienteId == expedienteId)
                {
                    filtrados.Add(tramite);
                }
            }
            return filtrados;
        }

        public void Modificar(Tramite tramiteModificado)
        {
            List<Tramite> todos = new List<Tramite>();
            foreach (var t in ObtenerTodos())
            {
                todos.Add(t);
            }

            int indice = -1;
            for (int i = 0; i < todos.Count; i++)
            {
                if (todos[i].id == tramiteModificado.id)
                {
                    indice = i;
                    break;
                }
            }

            if (indice == -1)
            {
                throw new RepositorioException($"No se encontró el trámite con ID {tramiteModificado.id} para modificar.");
            }

            todos[indice] = tramiteModificado;
            GuardarTodo(todos);
        }

        public void Eliminar(Guid id)
        {
            List<Tramite> todos = new List<Tramite>();
            foreach (var t in ObtenerTodos())
            {
                todos.Add(t);
            }

            Tramite? elementoAEliminar = null;
            foreach (var t in todos)
            {
                if (t.id == id)
                {
                    elementoAEliminar = t;
                    break;
                }
            }

            if (elementoAEliminar == null)
            {
                throw new RepositorioException($"No se encontró el trámite con ID {id} para eliminar.");
            }

            todos.Remove(elementoAEliminar);
            GuardarTodo(todos);
        }

        private void GuardarTodo(List<Tramite> lista)
        {
            List<string> lineas = new List<string>();
            foreach (var t in lista)
            {
                lineas.Add($"{t.id}|{t.ExpedienteId}|{t.Etiqueta}|{t.Contenido.Valor}|{t.FechaCreacion:O}|{t.FechaUltimaModificacion:O}|{t.UsuarioUltimoCambio}");
            }
            File.WriteAllLines(_nombreArchivo, lineas);
        }
    }
}