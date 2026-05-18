using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Infraestructura;

namespace SGE.Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("   SISTEMA DE GESTIÓN DE EXPEDIENTES (SGE) ");
            Console.WriteLine("===========================================");

            // COMPOSITION ROOT: Instanciación de dependencias
            // TODO: Cuando tu compañero suba la infraestructura, descomentá estas 4 líneas reales:
            // IExpedienteRepository expedienteRepo = new ExpedienteTxtRepository();
            // ITramiteRepository tramiteRepo = new TramiteTxtRepository();
            // IAutorizacionService authService = new AutorizacionProvisionalService();
            // ActualizacionEstadoExpedienteService actualizacionService = new ActualizacionEstadoExpedienteService(expedienteRepo, tramiteRepo);

            // OBJETOS SIMULADOS TEMPORALES (Borrar/comentar cuando uses las líneas de arriba)
            IExpedienteRepository expedienteRepo = null!;
            ITramiteRepository tramiteRepo = null!;
            IAutorizacionService authService = null!;
            ActualizacionEstadoExpedienteService actualizacionService = null!;

            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("\n--- MENÚ DE OPERACIONES ---");
                Console.WriteLine("1. Dar de Alta un Expediente");
                Console.WriteLine("2. Dar de Baja un Expediente (Y sus trámites asociados)");
                Console.WriteLine("3. Modificar Caratula de un Expediente");
                Console.WriteLine("4. Cambiar Estado de un Expediente (Manual)");
                Console.WriteLine("5. Agregar un Trámite a un Expediente");
                Console.WriteLine("6. Dar de Baja un Trámite");
                Console.WriteLine("7. Modificar Contenido de un Trámite");
                Console.WriteLine("0. Salir del Sistema");
                
                string opcion = PedirTextoOpcional("Seleccione una opción: ");

                switch (opcion)
                {
                    case "1":
                        AltaExpedienteInteractiva(expedienteRepo, authService);
                        break;
                    case "2":
                        BajaExpedienteInteractiva(expedienteRepo, tramiteRepo, authService);
                        break;
                    case "3":
                        ModificarCaratulaInteractiva(expedienteRepo, authService);
                        break;
                    case "4":
                        CambiarEstadoManualInteractiva(expedienteRepo, authService);
                        break;
                    case "5":
                        AgregarTramiteInteractiva(tramiteRepo, authService, actualizacionService);
                        break;
                    case "6":
                        BajaTramiteInteractiva(tramiteRepo, authService, actualizacionService);
                        break;
                    case "7":
                        ModificarTramiteInteractiva(tramiteRepo, authService, actualizacionService);
                        break;
                    case "0":
                        salir = true;
                        Console.WriteLine("Saliendo del sistema SGE");
                        break;
                    default:
                        Console.WriteLine("Opción incorrecta");
                        break;
                }
            }
        }

        static void AltaExpedienteInteractiva(IExpedienteRepository repo, IAutorizacionService auth)
        {
            Console.WriteLine("\nALTA DE EXPEDIENTE");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            string caratula = PedirTextoObligatorio("Ingrese el detalle de la carátula: ");

            AgregarExpedienteRequest request = new AgregarExpedienteRequest(idUsuario, caratula);
            AltaExpedienteUseCase casoDeUso = new AltaExpedienteUseCase(repo, auth);

            EjecutarSeguro(() => {
                AgregarExpedienteResponse response = casoDeUso.Result(request); // Nota: Si tu método se llama Ejecutar o Result, adaptalo acá. Vimos Ejecutar en los anteriores.
                Console.WriteLine($"Expediente creado. ID Asignado: {response.ExpedienteId}");
            });
        }

        static void BajaExpedienteInteractiva(IExpedienteRepository repo, ITramiteRepository tramiteRepo, IAutorizacionService auth)
        {
            Console.WriteLine("\nBAJA DE EXPEDIENTE");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            Guid idExpediente = PedirGuid("Ingrese el ID del Expediente a eliminar: ");

            BajaExpedienteRequest request = new BajaExpedienteRequest(idUsuario, idExpediente);
            BajaExpedienteUseCase casoDeUso = new BajaExpedienteUseCase(repo, tramiteRepo, auth);

            EjecutarSeguro(() => {
                BajaExpedienteResponse response = casoDeUso.Ejecutar(request);
                Console.WriteLine("El expediente y todos sus trámites fueron eliminados en cascada.");
            });
        }

        static void ModificarCaratulaInteractiva(IExpedienteRepository repo, IAutorizacionService auth)
        {
            Console.WriteLine("\nMODIFICAR CARÁTULA DE EXPEDIENTE");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            Guid idExpediente = PedirGuid("Ingrese el ID del Expediente: ");
            string nuevaCaratula = PedirTextoObligatorio("Ingrese el nuevo texto de la carátula: ");

            ModificarCaratulaExpedienteRequest request = new ModificarCaratulaExpedienteRequest(idUsuario, idExpediente, nuevaCaratula);
            ModificarCaratulaExpedienteUseCase casoDeUso = new ModificarCaratulaExpedienteUseCase(repo, auth);

            EjecutarSeguro(() => {
                ModificarCaratulaExpedienteResponse response = casoDeUso.Ejecutar(request);
                Console.WriteLine("La carátula del expediente fue actualizada correctamente.");
            });
        }

        static void CambiarEstadoManualInteractiva(IExpedienteRepository repo, IAutorizacionService auth)
        {
            Console.WriteLine("\nCAMBIAR ESTADO MANUAL DE EXPEDIENTE");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            Guid idExpediente = PedirGuid("Ingrese el ID del Expediente: ");
            
            Console.WriteLine("Estados válidos: 0=RecienIniciado, 1=ParaResolver, 2=ConResolucion, 3=EnNotificacion, 4=Finalizado");
            int nuevoEstado = PedirEnteroEnRango("Seleccione el número de estado: ", 0, 4);

            CambiarEstadoExpedienteRequest request = new CambiarEstadoExpedienteRequest(idUsuario, idExpediente, nuevoEstado);
            CambiarEstadoExpedienteUseCase casoDeUso = new CambiarEstadoExpedienteUseCase(repo, auth);

            EjecutarSeguro(() => {
                CambiarEstadoExpedienteResponse response = casoDeUso.Ejecutar(request);
                Console.WriteLine("El estado del expediente cambió manualmente.");
            });
        }

        // --- FLUJOS INTERACTIVOS DE TRÁMITES ---

        static void AgregarTramiteInteractiva(ITramiteRepository tramiteRepo, IAutorizacionService auth, ActualizacionEstadoExpedienteService actualizacionService)
        {
            Console.WriteLine("\nAGREGAR TRÁMITE A EXPEDIENTE");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            Guid idExpediente = PedirGuid("Ingrese el ID del Expediente al que pertenece: ");
            string contenido = PedirTextoObligatorio("Ingrese el contenido del trámite: ");
            
            Console.WriteLine("Etiquetas válidas: 0=EscritorioPresentado, 1=PaseAEstudio, 2=Despacho, 3=Resolucion, 4=Notificacion, 5=PaseAlArchivo");
            int etiqueta = PedirEnteroEnRango("Seleccione el número de etiqueta: ", 0, 5);

            AgregarTramiteRequest request = new AgregarTramiteRequest(idUsuario, idExpediente, contenido, etiqueta);
            AgregarTramiteUseCase casoDeUso = new AgregarTramiteUseCase(tramiteRepo, auth, actualizacionService);

            EjecutarSeguro(() => {
                AgregarTramiteResponse response = casoDeUso.Ejecutar(request);
                Console.WriteLine($"Éxito: Trámite creado e incorporado. ID: {response.TramiteId}");
                Console.WriteLine("El estado del expediente fue recalculado automáticamente por el sistema.");
            });
        }

        static void BajaTramiteInteractiva(ITramiteRepository tramiteRepo, IAutorizacionService auth, ActualizacionEstadoExpedienteService actualizacionService)
        {
            Console.WriteLine("\n[6] --- BAJA DE TRÁMITE ---");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            Guid idTramite = PedirGuid("Ingrese el ID del Trámite que desea eliminar: ");

            BajaTramiteRequest request = new BajaTramiteRequest(idUsuario, idTramite);
            BajaTramiteUseCase casoDeUso = new BajaTramiteUseCase(tramiteRepo, auth, actualizacionService);

            EjecutarSeguro(() => {
                BajaTramiteResponse response = casoDeUso.Ejecutar(request);
                Console.WriteLine("Éxito: Trámite eliminado de los registros.");
                Console.WriteLine("El estado del expediente fue recalculado automáticamente en base a los trámites restantes.");
            });
        }

        static void ModificarTramiteInteractiva(ITramiteRepository tramiteRepo, IAutorizacionService auth, ActualizacionEstadoExpedienteService actualizacionService)
        {
            Console.WriteLine("\n[7] --- MODIFICAR CONTENIDO DE TRÁMITE ---");
            Guid idUsuario = PedirGuid("Ingrese ID de Usuario (Guid): ");
            Guid idTramite = PedirGuid("Ingrese el ID del Trámite a modificar: ");
            string nuevoContenido = PedirTextoObligatorio("Ingrese el nuevo contenido del trámite: ");

            ModificarTramiteRequest request = new ModificarTramiteRequest(idUsuario, idTramite, nuevoContenido);
            ModificarTramiteUseCase casoDeUso = new ModificarTramiteUseCase(tramiteRepo, auth, actualizacionService);

            EjecutarSeguro(() => {
                ModificarTramiteResponse response = casoDeUso.Ejecutar(request);
                Console.WriteLine("El contenido del trámite fue actualizado.");
            });
        }

        static void EjecutarSeguro(Action accion)
        {
            try
            {
                accion();
            }
            catch (DominioException ex)
            {
                Console.WriteLine($"ERROR DE REGLA DE NEGOCIO: {ex.Message}");
            }
            catch (AutorizacionException ex)
            {
                Console.WriteLine($"ERROR DE PERMISOS: {ex.Message}");
            }
            catch (EntidadNoEncontradaException ex)
            {
                Console.WriteLine($"ERROR DE BÚSQUEDA: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR CRÍTICO NO CONTROLADO: {ex.Message}");
            }
        }

        //MÉTODOS HELPERS BLINDADOS A PRUEBA DE INPUTS INCORRECTOS

        static string PedirTextoObligatorio(string mensaje)
        {
            string input = "";
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(mensaje);
                input = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada inválida: El texto no puede estar en blanco. Intente de nuevo.");
                }
            }
            return input;
        }

        static string PedirTextoOpcional(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine() ?? "";
        }

        static Guid PedirGuid(string mensaje)
        {
            Guid resultado;
            while (true)
            {
                Console.Write(mensaje);
                string input = Console.ReadLine() ?? "";
                if (Guid.TryParse(input, out resultado))
                {
                    return resultado;
                }
                Console.WriteLine("Formato de ID erróneo. Debe ingresar un formato Guid válido de 32 dígitos (ej: 00000000-0000-0000-0000-000000000000).");
            }
        }

        static int PedirEnteroEnRango(string mensaje, int min, int max)
        {
            int resultado;
            while (true)
            {
                Console.Write(mensaje);
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out resultado) && resultado >= min && resultado <= max)
                {
                    return resultado;
                }
                Console.WriteLine($"Entrada incorrecta. Debe ingresar un número entero que esté entre {min} y {max}.");
            }
        }
    }
}