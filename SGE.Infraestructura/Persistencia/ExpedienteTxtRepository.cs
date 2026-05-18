using System;
using System.Collections.Generic;
using System.IO;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes;
using SGE.Infraestructura.Comun;

namespace SGE.Infraestructura.Persistencia{
    public class ExpedienteTxtRepository : IExpedienteRepository
    {
        private readonly string _nombreArchivo = "expedientes.txt";

        public void Agregar(Expediente expediente)
        {
            string linea = $"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.FechaCreacion:O}|{expediente.FechaUltimaModificacion:O}|{expediente.UsuarioUltimoCambio}|{expediente.Estado}";
            File.AppendAllLines(_nombreArchivo, new[] { linea });
        }

        public IEnumerable<Expediente> ObtenerTodos()
        {
            if (!File.Exists(_nombreArchivo))
            {
                return new List<Expediente>();
            }

            List<Expediente> lista = new List<Expediente>();
            string[] lineas = File.ReadAllLines(_nombreArchivo);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                string[] datos = linea.Split('|');

                Guid id = Guid.Parse(datos[0]);
                Caratula caratulaObj = new Caratula(datos[1]); 
                DateTime fechaCreacion = DateTime.Parse(datos[2]);
                DateTime fechaModif = DateTime.Parse(datos[3]);
                Guid usuarioId = Guid.Parse(datos[4]);
                EstadoExpediente estado = (EstadoExpediente)Enum.Parse(typeof(EstadoExpediente), datos[5]);
                Expediente expedienteReconstruido = Expediente.Reconstruir(id, caratulaObj, fechaCreacion, fechaModif, usuarioId, estado);
                lista.Add(expedienteReconstruido);
            }

            return lista;
        }

        public Expediente? ObtenerPorId(Guid id)
        {
            foreach (var expediente in ObtenerTodos())
            {
                if (expediente.Id == id)
                {
                    return expediente;
                }
            }
            return null;
        }

        public void Modificar(Expediente expedienteModificado)
        {
            List<Expediente> todos = new List<Expediente>();
            foreach (var e in ObtenerTodos())
            {
                todos.Add(e);
            }

            int indice = -1;
            for (int i = 0; i < todos.Count; i++)
            {
                if (todos[i].Id == expedienteModificado.Id)
                {
                    indice = i;
                    break;
                }
            }

            if (indice == -1)
            {
                throw new RepositorioException($"No se encontró el expediente con ID {expedienteModificado.Id} para modificar.");
            }

            todos[indice] = expedienteModificado;
            GuardarTodo(todos);
        }

        public void Eliminar(Guid id)
        {
            List<Expediente> todos = new List<Expediente>();
            foreach (var e in ObtenerTodos())
            {
                todos.Add(e);
            }

            Expediente? elementoAEliminar = null;
            foreach (var e in todos)
            {
                if (e.Id == id)
                {
                    elementoAEliminar = e;
                    break;
                }
            }

            if (elementoAEliminar == null)
            {
                throw new RepositorioException($"No se encontró el expediente con ID {id} para eliminar.");
            }

            todos.Remove(elementoAEliminar);
            GuardarTodo(todos);
        }

        private void GuardarTodo(List<Expediente> lista)
        {
            List<string> lineas = new List<string>();
            foreach (var e in lista)
            {
                lineas.Add($"{e.Id}|{e.Caratula.Valor}|{e.FechaCreacion:O}|{e.FechaUltimaModificacion:O}|{e.UsuarioUltimoCambio}|{e.Estado}");
            }
            File.WriteAllLines(_nombreArchivo, lineas);
        }
    }
}