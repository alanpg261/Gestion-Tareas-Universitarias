# AGENTS.md — Gestión de Tareas Universitarias

Guía para agentes de IA que trabajen en este repositorio. Todo el contenido refleja el estado real del código; no se deben dar por implementadas funcionalidades que solo existen en los documentos de diseño.

## 1. Descripción general

Aplicación web para que estudiantes universitarios organicen sus actividades académicas: crean **materias** (asignaturas) y les asignan **tareas** con fecha límite y estado de completado. Proyecto con fines educativos del curso Programación Web (IF2003), grupo 603.

Estado actual de la implementación:

- **Implementado:** autenticación con ASP.NET Core Identity (registro, login, gestión de cuenta), CRUD completo de `Materia` y CRUD completo de `Tarea`, páginas estáticas (Home, Privacy, Error).
- **Aún no implementado** (existe en `docs/definicion.md` pero no en el código): aislamiento de datos por usuario (regla RN-07 / requisito RNF-04), dashboard principal, estadísticas, panel de administrador, roles (Estudiante/Administrador), alertas de tareas por vencer, filtros por estado o materia, y el campo `Estado` de la tarea (hoy solo existe el bool `Completada`).
- `ApplicationUser : IdentityUser` es una subclase vacía; ni `Materia` ni `Tarea` tienen referencia al usuario propietario, y los controladores no filtran por usuario. Cualquier usuario autenticado (o no, ver sección Seguridad) ve y edita todos los registros.

## 2. Stack tecnológico

- **.NET 10** (`net10.0`), C# con `Nullable` e `ImplicitUsings` habilitados. SDK usado en el entorno: 10.0.302.
- **ASP.NET Core MVC** con vistas Razor (`AddControllersWithViews`) + **Razor Pages** para la UI de Identity.
- **ASP.NET Core Identity** (paquetes `Microsoft.AspNetCore.Identity.*` 10.0.9), UI scaffoldada en `Areas/Identity`.
- **Entity Framework Core 10.0.9** con proveedor **SQL Server**; `Microsoft.VisualStudio.Web.CodeGeneration.Design` 10.0.3 (scaffolding de Visual Studio).
- Front-end: **Bootstrap 5**, **jQuery** + jQuery Validate (unobtrusive), todo servido local desde `wwwroot/lib/`. No hay npm ni bundler.
- Sin archivo de solución (`.sln`); el único proyecto es `GestionTareasUniversitarias/GestionTareasUniversitarias.csproj`.

## 3. Estructura del código

```
GestionTareasUniversitarias/
├── Program.cs                  # Punto de entrada: DbContext, MVC, Identity, pipeline HTTP
├── appsettings.json            # Cadena de conexión "DefaultConnection" (SQL Server Express)
├── Controllers/                # Controladores MVC
│   ├── HomeController.cs       # Index, Privacy, Error (file-scoped namespace)
│   ├── MateriasController.cs   # CRUD de Materia (block namespace, estilo scaffold VS)
│   └── TareasController.cs     # CRUD de Tarea con SelectList de Materia
├── Models/                     # Tarea, Materia, ApplicationUser, ErrorViewModel
├── Data/
│   ├── AppDbContext.cs         # IdentityDbContext<ApplicationUser> + DbSet<Materia/Tarea>
│   └── AppDbContextFactory.cs  # IDesignTimeDbContextFactory (cadena de conexión duplicada)
├── Migrations/                 # InitialCreate y AddIdentity (esquema AspNet*)
├── Areas/Identity/Pages/       # UI de Identity generada por scaffolding (no editar a mano)
├── Views/                      # Vistas Razor por controlador + Shared (_Layout, _LoginPartial)
└── wwwroot/                    # css/js propios + lib/ (bootstrap, jquery, validación)
```

Modelo de datos (configurado por convención EF, sin Fluent API):

- `Materia`: Id, Nombre (obligatorio, máx. 100), Descripcion (máx. 300), colección `Tareas`.
- `Tarea`: Id, Titulo (obligatorio, máx. 100), Descripcion (máx. 500), FechaLimite (obligatoria), Completada (bool), MateriaId (obligatorio) → `Materia` con **borrado en cascada** (eliminar una materia elimina sus tareas).
- Validación con **DataAnnotations** en las propiedades; nombres de tipos y propiedades en **español** (`Titulo`, `Descripcion`, `FechaLimite`, `Completada`).

## 4. Comandos de compilación y ejecución

Todos los comandos se ejecutan desde `GestionTareasUniversitarias/` (el directorio del `.csproj`):

```bash
dotnet restore                      # Restaurar paquetes NuGet
dotnet build                        # Compilar (Debug por defecto)
dotnet run                          # Ejecutar; perfiles en Properties/launchSettings.json
dotnet run --launch-profile https   # https://localhost:7218 y http://localhost:5220
```

No hay scripts de build, contenedores, CI/CD ni procesos de despliegue definidos; el despliegue no está configurado.

## 5. Base de datos y migraciones

- Motor: **SQL Server Express** local (`Server=localhost\SQLEXPRESS`), autenticación Windows (`Trusted_Connection=True`), base de datos `GestionTareasUniversitariasDb` (`appsettings.json` → `ConnectionStrings:DefaultConnection`).
- La cadena de conexión está **duplicada** en `Data/AppDbContextFactory.cs` (necesaria para herramientas de diseño de EF). Al cambiarla, actualizar ambos lugares.
- Aplicar/crear migraciones (requiere la herramienta global `dotnet ef`):

```bash
dotnet ef database update         # Aplicar migraciones pendientes (crea la BD si no existe)
dotnet ef migrations add <Nombre> # Nueva migración tras cambiar el modelo
dotnet ef migrations list         # Listar migraciones
```

- Migraciones existentes: `InitialCreate` (tablas Materias/Tareas) y `AddIdentity` (tablas AspNet*). Son código generado; no editar a mano.
- La base de datos no se crea automáticamente al arrancar (no hay `EnsureCreated` ni `Migrate()` en `Program.cs`); hay que ejecutar `dotnet ef database update`.

## 6. Convenciones de código

- **Idioma de la documentación y del dominio: español.** Los identificadores de negocio usan términos en español; no anglicizarlos al añadir miembros al modelo (p. ej. seguir usando `Titulo`, `FechaLimite`).
- Los modelos y `Program.cs` usan **file-scoped namespaces**; los controladores generados usan block namespaces. Respetar el estilo de cada archivo.
- Controladores: inyección de `AppDbContext` por constructor en campo `_context`, acciones `async Task<IActionResult>`, lista blanca de binding con `[Bind("...")]`, `[ValidateAntiForgeryToken]` en los POST, patrón `DeleteConfirmed` con `[HttpPost, ActionName("Delete")]`, comprobaciones `id == null` → `NotFound()`.
- Vistas Razor: `ViewData["Title"]`, tag helpers (`asp-action`, `asp-route-id`), clases de Bootstrap, validación con `_ValidationScriptsPartial`.
- Comentarios: los existentes son los del scaffolding de Visual Studio (en inglés); no propagarlos a código nuevo —el proyecto no exige comentarios.

## 7. Pruebas

**No existe infraestructura de pruebas:** no hay proyectos de test (xUnit/NUnit/MSTest), ni tests manuales automatizados, ni cobertura. La verificación actual es compilar (`dotnet build`) y probar la aplicación manualmente con `dotnet run`. Si se añade un proyecto de pruebas, crearlo con el mismo target (`net10.0`) y siguiendo la estructura estándar de .NET.

## 8. Seguridad

- Contraseñas gestionadas por Identity (hash con PBKDF2); `RequireConfirmedAccount = false` en `Program.cs:20` (no se exige confirmación de correo).
- HTTPS forzado (`UseHttpsRedirection`), HSTS y página de error genérica solo fuera de Development.
- Los POST llevan `[ValidateAntiForgeryToken]` y el binding restringido con `[Bind]` (mitiga overposting).
- **Limitación conocida:** las acciones de `MateriasController` y `TareasController` no tienen `[Authorize]`, por lo que el CRUD es accesible sin autenticar, y nada aísla los datos por usuario. Esto contradice las reglas RN-01/RN-07 y el requisito RNF-04 del documento de definición — es trabajo pendiente deliberado, no un bug a "arreglar" sin coordinar con el diseño (implica añadir `UsuarioId` al modelo).
- `AllowedHosts: "*"` en `appsettings.json`; restringir en producción.

## 9. Documentación del proyecto

En `docs/` (todo en español): `definicion.md` (definición oficial del proyecto: requerimientos, reglas de negocio RN-01…RN-07, modelo de datos, historias de usuario y mockups), `srs.md` (especificación anterior), `mockup/*.png` (pantallas) y `presentacion.pptx`. `README.md` en la raíz es solo el título del repo. Consultar `docs/definicion.md` antes de decidir alcance de una funcionalidad.

## 10. Estado del repositorio y notas operativas

- **No hay `.gitignore`** y los directorios `bin/` y `obj/` (461 archivos de salida de compilación) están **commiteados** en el repositorio. No agregar más artefactos de build; idealmente el equipo debería introducir un `.gitignore` estándar de .NET y dejar de trackearlos.
- El repositorio es una copia de trabajo limpia sobre `main` con `origin/main`.
- `Areas/Identity` y `Migrations` son código generado por scaffolding: regenerar con Visual Studio/`dotnet aspnet-codegenerator` en lugar de editarlos a mano.
