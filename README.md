# Proyecto-de-Software-.NET
Primer trabajo de la asignatura Seminario de Lenguajes, opcion .NET

Estructura del Proyecto:
SGE/ (Carpeta Raíz)
│
├── SGE.sln (Archivo de solución que agrupa los 4 proyectos) 
│
├── SGE.Dominio/ (Class Library) [cite: 26, 53]
│   ├── Expedientes/ [cite: 33, 47]
│   │   ├── Expediente.cs (Entidad) [cite: 58]
│   │   ├── EstadoExpediente.cs (Enum: RecienIniciado, ParaResolver, etc.) [cite: 66]
│   │   └── Caratula.cs (Value Object - record class) [cite: 62, 86]
│   ├── Tramites/ [cite: 33, 47]
│   │   ├── Tramite.cs (Entidad) [cite: 72]
│   │   ├── EtiquetaTramite.cs (Enum: Resolucion, PaseAEstudio, etc.) [cite: 75]
│   │   └── ContenidoTramite.cs (Value Object - record class) [cite: 76, 87]
│   └── Comun/ [cite: 47]
│       └── DominioException.cs [cite: 47, 54]
│
├── SGE.Aplicacion/ (Class Library - Depende de Dominio) [cite: 27, 110]
│   ├── Expedientes/ [cite: 50]
│   │   ├── AgregarExpedienteUseCase.cs [cite: 130, 133]
│   │   ├── ModificarCaratulaExpedienteUseCase.cs [cite: 134, 135]
│   │   ├── CambiarEstadoExpedienteUseCase.cs [cite: 134, 135]
│   │   ├── BajaExpedienteUseCase.cs [cite: 133, 136]
│   │   ├── ListarExpedientesUseCase.cs [cite: 150]
│   │   ├── IExpedienteRepository.cs (Interfaz) [cite: 115]
│   │   └── ExpedienteDTOs.cs (Records para Request y Response) [cite: 130]
│   ├── Tramites/ [cite: 50]
│   │   ├── AgregarTramiteUseCase.cs [cite: 137]
│   │   ├── BajaTramiteUseCase.cs [cite: 137]
│   │   ├── ModificarTramiteUseCase.cs [cite: 137]
│   │   ├── ListarTramitesPorExpedienteUseCase.cs [cite: 151]
│   │   ├── ITramiteRepository.cs (Interfaz) [cite: 116]
│   │   └── TramiteDTOs.cs (Records para Request y Response) [cite: 130]
│   ├── Servicios/
│   │   └── ActualizacionEstadoExpedienteService.cs (Servicio de Aplicación) [cite: 140]
│   └── Autorizacion/ [cite: 50]
│       ├── IAutorizacionService.cs (Interfaz) [cite: 117]
│       ├── Permiso.cs (Enum: ExpedienteAlta, TramiteBaja, etc.) [cite: 153]
│       └── AutorizacionException.cs [cite: 155]
│
├── SGE.Infraestructura/ (Class Library - Depende de Aplicacion y Dominio) [cite: 28, 156]
│   ├── Persistencia/
│   │   ├── ExpedienteTxtRepository.cs (Implementación en archivos .txt) [cite: 158]
│   │   └── TramiteTxtRepository.cs (Implementación en archivos .txt) [cite: 158]
│   ├── Servicios/
│   │   └── AutorizacionProvisionalService.cs [cite: 164]
│   └── Comun/
│       └── RepositorioException.cs [cite: 120]
│
└── SGE.Consola/ (Console App - Depende de todos) [cite: 29, 168]
    └── Program.cs (Composition Root: donde se instancia todo y corre el flujo) [cite: 169, 170]
