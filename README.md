# Documento Explicativo - Sistema para la Gestión de Expedientes (SGE) 

**Asignatura:** Seminario de Lenguajes (Opción .NET) - 1º Semestre 2026  

### Integrantes del Grupo
    Alexis Veloso 21102/3
    Thomas Carabelli 26183/4
    Gonzalo Romano 25626/6
---

## Descripción de la Arquitectura
La solución SGE ha sido diseñada respetando la **Regla de Dependencia** de la Arquitectura Limpia. Se compone de cuatro proyectos:
1. **SGE.Dominio:** Núcleo de negocio con entidades (`Expediente`, `Tramite`) y objetos de valor inmutables.
2. **SGE.Aplicacion:** Orquestación mediante Casos de Uso aislados que se comunican exclusivamente con DTOs.
3. **SGE.Infraestructura:** Persistencia provisional mediante archivos de texto plano, implementando las interfaces de repositorio de la capa de aplicación.
4. **SGE.Consola:** Punto de entrada para probar la aplicación.

---

## Guía de Pruebas (Program.cs)

Para corroborar el correcto funcionamiento del sistema, toda la instanciación de dependencias y la ejecución de Casos de Uso se ha centralizado en el archivo `Program.cs` del proyecto **SGE.Consola**.

A continuación, se detallan los ejemplos de código implementados en la consola y sus respectivas salidas, demostrando tanto el "Camino Feliz" como el manejo de errores de dominio e infraestructura.

### 1. Configuración Inicial
Al inicio del `Program.cs`, se instancian los repositorios y servicios necesarios para inyectarlos en los Casos de Uso:

```csharp
// --- COMPOSITION ROOT ---
Guid usuarioActualId = Guid.NewGuid(); // Simulamos un usuario logueado

// 1. Instanciación de Infraestructura
IExpedienteRepository expedienteRepo = new ExpedienteTxtRepository();
ITramiteRepository tramiteRepo = new TramiteTxtRepository();
IAutorizacionService authService = new AutorizacionProvisionalService();
ActualizacionEstadoExpedienteService estadoService = new ActualizacionEstadoExpedienteService(expedienteRepo, tramiteRepo);

// 2. Instanciación de Casos de Uso
var altaExpedienteUC = new AgregarExpedienteUseCase(expedienteRepo, authService);
var bajaExpedienteUC = new BajaExpedienteUseCase(expedienteRepo, tramiteRepo, authService);

```

### 2. Prueba 1: Alta de Expediente

Se prueba la creación de un expediente válido y su persistencia.

**Código en Program.cs:**

```csharp
Console.WriteLine("=== PRUEBA 1: ALTA DE EXPEDIENTE ===");
try
{
    var requestAlta = new AgregarExpedienteRequest(usuarioActualId, "Solicitud de Inscripción a Beca");
    var responseAlta = altaExpedienteUC.Ejecutar(requestAlta);
    
    Console.WriteLine($"[ÉXITO] Expediente creado correctamente.");
    Console.WriteLine($"[INFO] ID asignado: {responseAlta.ExpedienteId}");
}
catch (Exception ex)
{
    Console.WriteLine($"[ERROR INESPERADO] {ex.Message}");
}

```

**Salida por Consola:**

```text
=== PRUEBA 1: ALTA DE EXPEDIENTE ===
[ÉXITO] Expediente creado correctamente.
[INFO] ID asignado: a1b2c3d4-e5f6-7890-abcd-1234567890ab

```

### 3. Prueba 2: Excepciones del Dominio

Se prueba que la capa de dominio rechace la creacion de una entidad con datos invalidos

**Código en Program.cs:**

```csharp
Console.WriteLine("\n=== PRUEBA 2: ALTA CON CARÁTULA INVÁLIDA ===");
try
{
    // Intento de alta con texto vacío para disparar la validacion del Value Object
    var requestInvalido = new AgregarExpedienteRequest(usuarioActualId, "   ");
    altaExpedienteUC.Ejecutar(requestInvalido);
}
catch (DominioException ex)
{
    Console.WriteLine($"[DOMINIO RECHAZADO] {ex.Message}");
}

```

**Salida por Consola:**

```text
=== PRUEBA 2: ALTA CON CARATULA INVÁLIDA ===
[DOMINIO RECHAZADO] La caratula no puede ser nula o vacia

```

### 4. Prueba 3: Excepciones de Infraestructura

Se demuestra como el repositorio captura intentos de operar sobre datos inexistentes sin devolver un `false`.

**Código en Program.cs:**

```csharp
Console.WriteLine("\n=== PRUEBA 3: INTENTO DE BAJA DE EXPEDIENTE INEXISTENTE ===");
try
{
    Guid idInventado = Guid.NewGuid();
    var requestBaja = new BajaExpedienteRequest(usuarioActualId, idInventado);
    bajaExpedienteUC.Ejecutar(requestBaja);
}
catch (RepositorioException ex)
{
    Console.WriteLine($"[INFRAESTRUCTURA FALLA] {ex.Message}");
}

```

**Salida por Consola:**

```text
=== PRUEBA 3: INTENTO DE BAJA DE EXPEDIENTE INEXISTENTE ===
[INFRAESTRUCTURA FALLA] No se encontró el expediente con ID 550e8400-e29b-41d4-a716-446655440000 para eliminar.

```

---

## Instrucciones de Ejecución General

Para compilar y correr estas pruebas desde la terminal, posicionarse en la carpeta raíz (`SGE`) y ejecutar el siguiente comando:

```bash
dotnet run --project SGE.Consola


Los datos persistidos durante las pruebas podrán verificarse abriendo los archivos `expedientes.txt` y `tramites.txt` que se generarán automáticamente en el directorio de salida del proyecto de consola.
