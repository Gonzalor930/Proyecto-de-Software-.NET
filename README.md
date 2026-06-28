# Sistema de Gestión de Expedientes (SGE) — Trabajo Práctico 2

**Asignatura:** Seminario de Lenguajes (Opción .NET) — 1º Semestre 2026

### Integrantes del Grupo
    Alexis Veloso 21102/3
    Thomas Carabelli 26183/4
    Gonzalo Romano 25626/6

---

## Descripción de la Arquitectura

La solución SGE respeta la **Regla de Dependencia** de la Arquitectura Limpia y se compone de cuatro proyectos:

1. **SGE.Dominio** — Núcleo de negocio con entidades (`Expediente`, `Tramite`, `Usuario`), objetos de valor inmutables y el enumerativo `Permiso`.
2. **SGE.Aplicacion** — Orquestación mediante Casos de Uso aislados que se comunican exclusivamente con DTOs e interactúan con repositorios e `IUnidadDeTrabajo`.
3. **SGE.Infraestructura** — Persistencia real con EF Core y SQLite, implementando las interfaces de repositorio y la unidad de trabajo. Incluye el `HashService` (SHA-256) y el `AutorizacionService` definitivo.
4. **SGE.WebApi** — Punto de entrada (Composition Root). Expone los endpoints HTTP organizados por módulo, configura JWT y Scalar.

---

## Credenciales de los Usuarios Semilla

Estos usuarios se crean automáticamente al inicializar la base de datos. No es necesario registrarlos.

| Rol | Correo | Contraseña | Permisos |
|---|---|---|---|
| Administrador | `admin@sge.com` | `admin123` | Acceso total |
| Usuario Básico | `basico@sge.com` | `1234` | Sin permisos de mutación (solo lectura) |
| Usuario Avanzado | `avanzado@sge.com` | `1234` | `ExpedienteAlta`, `ExpedienteModificacion`, `TramiteAlta`, `TramiteModificacion` |

> Las contraseñas nunca se almacenan en texto plano. Se aplica SHA-256 antes de persistirlas.

---

## Valores de los Enumerativos

Los campos `etiqueta` (trámites), `nuevoEstado` (expedientes) y `permisosDeseados` (usuarios) se envían como enteros.

**Permiso**

| Valor | Nombre |
|---|---|
| 0 | ExpedienteAlta |
| 1 | ExpedienteBaja |
| 2 | ExpedienteModificacion |
| 3 | TramiteAlta |
| 4 | TramiteBaja |
| 5 | TramiteModificacion |

> **Regla de implicancia:** El permiso `ExpedienteBaja` (1) otorga implícitamente `TramiteBaja` (4).

---

## Instrucciones de Ejecución

Posicionarse en la carpeta raíz de la solución y ejecutar:

```bash
dotnet restore
dotnet build
dotnet run --project SGE.WebApi
```

La API queda disponible en `http://localhost:5000`.
La interfaz gráfica de Scalar estará en `http://localhost:5000/scalar/v1`.

---

## Guía de Prueba de Endpoints desde Scalar

### Paso 1 — Obtener un Token JWT

Todo endpoint protegido requiere un token. Empezar siempre por aquí.

**`POST /api/usuarios/login`** — Sin token.

```json
{
  "correo": "admin@sge.com",
  "contrasenaPlana": "admin123"
}
```

La respuesta contiene el token JWT. En Scalar, hacer clic en el ícono del candado (**Authorize**) y pegarlo con el prefijo `Bearer`:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

### Paso 2 — Módulo de Usuarios

#### Registrar un nuevo usuario
**`POST /api/usuarios/registro`** — Sin token.

```json
{
  "nombre": "Usuario Nuevo",
  "correo": "nuevo@sge.com",
  "contrasenaPlana": "password123"
}
```

El usuario se registra sin permisos de mutación por defecto (`EsAdministrador: false`).

---

#### Modificar mis propios datos
**`PUT /api/usuarios/mis-datos`** — Requiere token. El `UserId` se extrae del token; no va en el body.

```json
{
  "nuevoNombre": "Nombre Actualizado",
  "nuevaContrasenaPlana": "nuevaPassword456"
}
```

---

#### Listar todos los usuarios *(solo administrador)*
**`GET /api/usuarios`** — Requiere token de administrador.

---

#### Modificar permisos de un usuario *(solo administrador)*
**`PUT /api/usuarios/{idUsuarioAModificar}/permisos`** — Requiere token de administrador.

Reemplazar `{idUsuarioAModificar}` con el `Id` (UUID) del usuario a modificar.

```json
{
  "permisosDeseados": [0, 2, 3, 5]
}
```

El array reemplaza la lista de permisos completa. Enviar `[]` para quitar todos los permisos.

---

#### Eliminar un usuario *(solo administrador)*
**`DELETE /api/usuarios/{idUsuarioAEliminar}`** — Requiere token de administrador.

Reemplazar `{idUsuarioAEliminar}` con el `Id` (UUID) del usuario a eliminar.

---

### Paso 3 — Módulo de Expedientes

#### Listar todos los expedientes
**`GET /api/expedientes`** — Requiere token.

---

#### Obtener un expediente con sus trámites
**`GET /api/expedientes/{id}`** — Requiere token.

Retorna el expediente junto con la colección completa de trámites asociados.

---

#### Crear un expediente
**`POST /api/expedientes`** — Requiere token + permiso `ExpedienteAlta` (0).

Autenticarse como `avanzado@sge.com` o `admin@sge.com`.

```json
{
  "detalleCaratula": "Solicitud de Beca Universitaria 2026"
}
```

---

#### Modificar la carátula de un expediente
**`PUT /api/expedientes/{id}/caratula`** — Requiere token + permiso `ExpedienteModificacion` (2).

```json
{
  "nuevaCaratula": "Solicitud de Beca Universitaria 2026 - Revisada"
}
```

---

#### Cambiar el estado de un expediente
**`PUT /api/expedientes/{id}/estado`** — Requiere token + permiso `ExpedienteModificacion` (2).

```json
{
  "nuevoEstado": 1
}
```

---

#### Dar de baja un expediente *(baja en cascada)*
**`DELETE /api/expedientes/{id}`** — Requiere token + permiso `ExpedienteBaja` (1).

Autenticarse como `admin@sge.com` (el Usuario Avanzado no tiene este permiso).
Elimina el expediente y todos sus trámites asociados.

---

### Paso 4 — Módulo de Trámites

#### Listar trámites de un expediente
**`GET /api/tramites/expediente/{idExpediente}`** — Requiere token.

---

#### Crear un trámite
**`POST /api/tramites`** — Requiere token + permiso `TramiteAlta` (3).

Autenticarse como `avanzado@sge.com` o `admin@sge.com`.

```json
{
  "expedienteId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "contenido": "Se adjunta documentación solicitada.",
  "etiqueta": 0
}
```

La creación de un trámite actualiza automáticamente el estado del expediente asociado.

---

#### Modificar un trámite
**`PUT /api/tramites/{id}`** — Requiere token + permiso `TramiteModificacion` (5).

```json
{
  "nuevoContenido": "Documentación actualizada con las correcciones solicitadas."
}
```

---

#### Eliminar un trámite
**`DELETE /api/tramites/{id}`** — Requiere token + permiso `TramiteBaja` (4).

Autenticarse como `admin@sge.com` (el Usuario Avanzado no tiene este permiso).

---

## Manejo de Errores

La API retorna respuestas estandarizadas en formato `ProblemDetails`:

| Excepción | Código HTTP | Cuándo ocurre |
|---|---|---|
| `DominioException` | `400 Bad Request` | Datos inválidos o reglas de negocio violadas |
| `AutorizacionException` | `403 Forbidden` | Sin permisos suficientes para la operación |
| `EntidadNoEncontradaException` | `404 Not Found` | El recurso solicitado no existe |
| Token ausente o inválido | `401 Unauthorized` | Endpoint protegido sin token válido |
