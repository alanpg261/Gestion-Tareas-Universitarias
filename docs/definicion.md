# Gestión de Tareas Universitarias

Documento de definición del proyecto — Programación Web (IF2003), grupo 603

## 1. Descripción general

Gestión de Tareas Universitarias es una plataforma web orientada a estudiantes universitarios que permite organizar, administrar y realizar seguimiento a las actividades académicas de manera centralizada.

La plataforma facilita la gestión de tareas, proyectos, talleres, exposiciones y evaluaciones mediante la creación de materias, asignación de fechas límite y actualización de estados de avance.

Los estudiantes podrán acceder mediante inicio de sesión, registrar sus actividades académicas, clasificarlas por asignatura y consultar su progreso desde un panel principal. Además, contarán con herramientas visuales para identificar tareas próximas a vencer, tareas vencidas y actividades completadas.

La solución busca reducir olvidos, mejorar la planificación académica y proporcionar una visión clara de las responsabilidades pendientes de cada estudiante.

### Conceptos del dominio

**Usuario**
- Persona registrada en el sistema.

**Materia**
- Asignatura asociada al estudiante.
- Ejemplos: Programación Web, Bases de Datos, Ingeniería de Software.

**Tarea**
- Actividad asignada al estudiante.
- Ejemplos: Taller, Proyecto, Exposición, Quiz.

**Estado**
- Situación actual de la tarea.
- Posibles estados: Pendiente, En progreso, Completada, Vencida.

**Fecha límite**
- Fecha máxima para completar una tarea.

## 2. Problema

Los estudiantes universitarios suelen manejar múltiples asignaturas, talleres, proyectos, exposiciones y actividades académicas al mismo tiempo. En muchos casos utilizan agendas físicas, grupos de mensajería o simplemente la memoria para recordar fechas de entrega, lo que puede generar olvidos, retrasos y bajo rendimiento académico.

Actualmente no existe una herramienta centralizada y sencilla que permita a los estudiantes organizar sus tareas académicas, realizar seguimiento a su progreso y recibir recordatorios oportunos.

Por esta razón se propone desarrollar una aplicación web que permita gestionar tareas universitarias de manera eficiente.

## 3. Objetivos

### Objetivo general

Desarrollar una aplicación web para la gestión de tareas universitarias que permita a los estudiantes organizar sus actividades académicas, realizar seguimiento a su progreso y administrar eficientemente sus fechas de entrega.

### Objetivos específicos

- Permitir la creación y almacenamiento de tareas asociadas a materias académicas.
- Reducir el riesgo de olvido de actividades mediante alertas visuales de vencimiento.
- Facilitar la consulta de tareas pendientes desde una única interfaz.
- Permitir la clasificación y filtrado de tareas por materia y estado.
- Proporcionar estadísticas básicas sobre el progreso académico del estudiante.

### Métricas de éxito

- Crear una tarea en menos de 30 segundos.
- Consultar tareas pendientes en menos de 5 segundos.
- Mostrar correctamente el 100% de las tareas asociadas a una materia.
- Permitir identificar tareas próximas a vencer desde el dashboard principal.
- Registrar correctamente cambios de estado de las tareas.

## 4. Stakeholders, actores y roles

### Stakeholders

**Primarios**
- Estudiantes universitarios.

**Secundarios**
- Docentes.
- Coordinadores académicos.
- Instituciones educativas.

### Login

Los estudiantes y administradores acceden mediante correo electrónico y contraseña.

- El estudiante solamente puede gestionar su propia información.
- El administrador puede acceder a funciones de gestión y supervisión del sistema.

### Actores y roles

**Estudiante**

Puede:
- Crear tareas.
- Editar tareas.
- Eliminar tareas.
- Marcar tareas como completadas.
- Consultar tareas pendientes.
- Visualizar tareas por materia.

**Administrador**

Puede:
- Gestionar usuarios.
- Consultar estadísticas globales.
- Administrar materias del sistema.
- Supervisar el funcionamiento de la plataforma.
- Gestionar configuraciones generales.

## 5. Alcance

### Incluye

- Registro e inicio de sesión con roles (Estudiante y Administrador).
- Gestión de materias (CRUD completo).
- Creación, edición y eliminación de tareas con asignación de fechas límite.
- Estados de tarea: Pendiente, En progreso, Completada y Vencida (con identificación visual).
- Clasificación por asignaturas y filtrado por estado, materia y búsqueda de texto.
- Alertas visuales de vencimiento y de proximidad a la fecha límite (72 horas).
- Dashboard principal con indicadores, próximas a vencer y tareas recientes.
- Estadísticas de productividad por materia.
- Panel administrativo: gestión de usuarios (bloqueo/desbloqueo) y estadísticas globales.
- API REST propia documentada con OpenAPI, consumida desde el frontend con `fetch`.
- Publicación en internet con despliegue continuo desde GitHub (ver sección 15).

### No incluye

- Integración con plataformas universitarias externas.
- Videollamadas.
- Chat entre usuarios.
- Calificaciones automáticas.
- Aplicación móvil.
- Sincronización con calendarios externos.

## 6. Funcionalidades

### Estudiante

- Registrarse.
- Iniciar sesión.
- Crear tareas.
- Editar tareas.
- Eliminar tareas.
- Consultar tareas.
- Marcar tareas como completadas.
- Gestionar materias.
- Consultar estadísticas personales.

### Administrador

- Gestionar usuarios.
- Consultar estadísticas generales.
- Supervisar el uso de la plataforma.
- Gestionar catálogos del sistema.

## 7. Requerimientos funcionales

| ID | Requerimiento | Rol | Prioridad | Estado | Nota |
|------|------|------|------|------|------|
| RF-01 | El sistema debe permitir el registro de usuarios. | Invitado | Alta | Implementado | Registro con ASP.NET Core Identity. |
| RF-02 | El sistema debe permitir iniciar sesión mediante correo y contraseña. | Todos | Alta | Implementado | Correo + contraseña con Identity; sesión por cookie. |
| RF-03 | El sistema debe permitir crear tareas. | Estudiante | Alta | Implementado | Con materia, fecha límite y estado. |
| RF-04 | El sistema debe permitir editar tareas existentes. | Estudiante | Alta | Implementado | Solo tareas propias (404 si no lo es). |
| RF-05 | El sistema debe permitir eliminar tareas. | Estudiante | Alta | Implementado | Solo tareas propias. |
| RF-06 | El sistema debe permitir marcar tareas como completadas. | Estudiante | Alta | Implementado | Modificado: el bool `Completada` se reemplazó por el enum `EstadoTarea` (Pendiente, EnProgreso, Completada, Vencida) según el modelo del documento. Se completa desde la vista y desde la API (`POST /api/tareas/{id}/completar`). |
| RF-07 | El sistema debe permitir gestionar materias. | Estudiante | Alta | Implementado | CRUD completo; eliminar una materia elimina en cascada sus tareas. |
| RF-08 | El sistema debe mostrar tareas filtradas por estado. | Estudiante | Media | Implementado | Pills de filtro; "Vencida" se calcula por fecha límite. |
| RF-09 | El sistema debe mostrar tareas filtradas por materia. | Estudiante | Media | Implementado | Select de materias propias + búsqueda por texto. |
| RF-10 | El sistema debe mostrar estadísticas de productividad. | Estudiante | Media | Implementado | Pantalla Estadísticas y tarjetas del dashboard (tasa de finalización y carga por materia). |
| RF-11 | El sistema debe permitir al administrador administrar usuarios. | Administrador | Alta | Implementado | Modificado: se acotó a listar usuarios con su rol y conteos, y bloquear/desbloquear (lockout de Identity). La edición de perfiles quedó fuera del alcance. |
| RF-12 | El sistema debe permitir al administrador consultar estadísticas generales. | Administrador | Media | Implementado | Tarjetas globales en el panel administrativo. |

## 8. Requerimientos no funcionales

| ID | Categoría | Requerimiento | Estado | Nota |
|------|------|------|------|------|
| RNF-01 | Rendimiento | Las páginas principales deben cargar en menos de 3 segundos. | Implementado | Cumple por diseño (páginas ligeras, consultas LINQ simples); no se hicieron pruebas de carga formales. |
| RNF-02 | Rendimiento | Las consultas de tareas deben responder en menos de 2 segundos. | Implementado | Ídem; volumen académico bajo según supuestos. |
| RNF-03 | Seguridad | Las contraseñas deben almacenarse cifradas. | Implementado | Identity las almacena con hash PBKDF2. |
| RNF-04 | Seguridad | Cada usuario solo podrá acceder a su propia información. | Implementado | `UsuarioId` en Materia; todos los controladores y la API filtran por el usuario autenticado. |
| RNF-05 | Usabilidad | La plataforma debe ser utilizable desde dispositivos móviles con ancho mínimo de 360px. | Implementado | Bootstrap 5 con rejilla responsive; el sidebar se apila en pantallas pequeñas. |
| RNF-06 | Compatibilidad | Debe funcionar correctamente en Chrome, Edge y Firefox. | Implementado | Tecnologías estándar (HTML5, fetch, Bootstrap 5). |
| RNF-07 | Disponibilidad | El sistema debe estar disponible al menos el 95% del tiempo. | Pendiente | Depende del despliegue en Azure; se evalúa sobre la dirección publicada. |
| RNF-08 | Mantenibilidad | El código debe seguir arquitectura MVC durante el desarrollo. | Implementado | Modelos, vistas y controladores separados; lógica de datos en controladores/servicios inyectados, no en las vistas. |

## 9. Reglas de negocio

- **RN-01.** Un estudiante debe iniciar sesión para gestionar tareas.
- **RN-02.** Toda tarea debe pertenecer a una materia.
- **RN-03.** Toda tarea debe tener un título.
- **RN-04.** Toda tarea debe tener una fecha límite.
- **RN-05.** Una tarea completada no puede estar en estado pendiente.
- **RN-06.** Las tareas vencidas deben identificarse visualmente.
- **RN-07.** Un estudiante solo puede visualizar sus propias tareas.

## 10. Modelo de datos

### Usuario
- id (Identity)
- correo
- contraseña (hash con PBKDF2 vía ASP.NET Core Identity)
- rol (Estudiante o Administrador)

### Materia
- id
- nombre
- descripcion
- usuario_id (obligatorio, FK al usuario propietario)

### Tarea
- id
- titulo
- descripcion
- fecha_limite
- estado (enum: Pendiente, EnProgreso, Completada, Vencida)
- materia_id (obligatorio, FK)

### Relaciones

- Un usuario tiene muchas materias.
- Una materia pertenece a un usuario.
- Una materia tiene muchas tareas (borrado en cascada).
- Una tarea pertenece a una materia.

## 11. Pantallas y flujo

### Pantalla 1: Inicio de sesión

**Rol:** Estudiante y Administrador

**Propósito:** Permitir la autenticación de los usuarios mediante correo electrónico y contraseña para acceder a las funcionalidades correspondientes a su rol.

---

### Pantalla 2: Dashboard

**Rol:** Estudiante

**Propósito:** Mostrar un resumen de las tareas pendientes, completadas y próximas a vencer, además de accesos rápidos a las funcionalidades principales.

---

### Pantalla 3: Gestión de tareas

**Rol:** Estudiante

**Propósito:** Permitir la creación, edición, eliminación y consulta de tareas académicas asociadas a las materias registradas.

---

### Pantalla 4: Gestión de materias

**Rol:** Estudiante

**Propósito:** Permitir crear, editar y administrar las materias utilizadas para clasificar las tareas.

---

### Pantalla 5: Estadísticas

**Rol:** Estudiante

**Propósito:** Mostrar indicadores sobre tareas completadas, pendientes y vencidas para facilitar el seguimiento académico.

---

### Pantalla 6: Panel administrativo

**Rol:** Administrador

**Propósito:** Gestionar usuarios registrados y consultar estadísticas generales del sistema.

---

### Flujo del estudiante

Inicio de sesión → Dashboard → Gestión de tareas → Crear/Editar tarea → Dashboard

Inicio de sesión → Dashboard → Gestión de materias → Dashboard

Inicio de sesión → Dashboard → Estadísticas → Dashboard

### Flujo del administrador

Inicio de sesión → Panel administrativo → Gestión de usuarios

Inicio de sesión → Panel administrativo → Estadísticas generales

```mermaid
flowchart TD

    A[Login] --> B[Dashboard]

    B --> C[Tareas]
    B --> D[Materias]
    B --> E[Calendario]
    B --> F[Estadísticas]
    B --> G[Perfil]

    C --> C1[Lista de Tareas]
    C1 --> C2[Crear Tarea]
    C1 --> C3[Editar Tarea]
    C1 --> C4[Eliminar Tarea]
    C1 --> C5[Marcar Completada]

    D --> D1[Lista de Materias]
    D1 --> D2[Crear Materia]
    D1 --> D3[Editar Materia]
    D1 --> D4[Eliminar Materia]

    E --> E1[Calendario Mensual]
    E1 --> C2

    F --> F1[Tareas Pendientes]
    F --> F2[Tareas Completadas]
    F --> F3[Tareas Vencidas]

    G --> G1[Ver Perfil]
    G1 --> G2[Editar Perfil]
```

## 12. Mockup

### Login

![Mockup login](./mockup/mockup-login.png)

Pantalla de autenticación utilizada por estudiantes y administradores. Permite ingresar mediante correo electrónico y contraseña para acceder a las funcionalidades correspondientes a cada rol.

---

### Dashboard

![Mockup dashboard](./mockup/mockup-dashboard.png)

Pantalla principal de la plataforma. Muestra un resumen general de tareas pendientes, tareas completadas y accesos rápidos a las funcionalidades más utilizadas.

---

### Tareas

![Mockup tareas](./mockup/mockup-tareas.png)

Permite consultar todas las tareas registradas por el usuario. Desde esta pantalla se pueden visualizar detalles y acceder a las opciones de edición o eliminación.

---

### Crear tarea

![Mockup crear tarea](./mockup/mockup-crear-tarea.png)

Formulario para registrar una nueva tarea académica. Incluye campos para ingresar título, descripción, fecha límite y materia asociada.

---

### Editar tarea

![Mockup editar tarea](./mockup/mockup-editar-tarea.png)

Permite modificar la información de una tarea previamente registrada y actualizar sus datos según las necesidades del usuario.

---

### Lista de materias

![Mockup materias](./mockup/mockup-materias.png)

Pantalla destinada a la administración de materias. Permite visualizar todas las materias registradas por el estudiante.

---

### Crear materia

![Mockup crear materia](./mockup/mockup-crear-materia.png)

Formulario para registrar una nueva materia dentro de la plataforma y utilizarla posteriormente para clasificar tareas académicas.

---

### Editar materia

![Mockup editar materia](./mockup/mockup-editar-materia.png)

Permite actualizar la información de una materia previamente creada.

---

### Flujo principal del estudiante

Login → Dashboard → Tareas → Crear tarea

Login → Dashboard → Tareas → Editar tarea

Login → Dashboard → Materias → Crear materia

Login → Dashboard → Materias → Editar materia

## 13. Historias de usuario, casos de uso, restricciones y supuestos

### Historias de usuario

**HU-01**
Como estudiante, quiero crear una tarea para organizar mis actividades académicas y cumplir oportunamente con mis responsabilidades.

**HU-02**
Como estudiante, quiero editar una tarea para corregir o actualizar información cuando sea necesario.

**HU-03**
Como estudiante, quiero eliminar una tarea para mantener mi lista de actividades organizada y actualizada.

**HU-04**
Como estudiante, quiero marcar una tarea como completada para llevar seguimiento de mi progreso académico.

**HU-05**
Como estudiante, quiero asociar tareas a materias para clasificar y organizar mejor mis actividades.

**HU-06**
Como estudiante, quiero visualizar mis tareas pendientes para identificar rápidamente las actividades que debo realizar.

**HU-07**
Como estudiante, quiero recibir alertas visuales sobre tareas próximas a vencer para evitar incumplir fechas de entrega.

**HU-08**
Como estudiante, quiero iniciar sesión con mis credenciales para acceder de forma segura a mi información académica.

**HU-09**
Como administrador, quiero gestionar los usuarios registrados para supervisar el uso correcto de la plataforma.

**HU-10**
Como administrador, quiero consultar estadísticas generales del sistema para conocer el estado y uso de la plataforma.

### Casos de uso

#### UC-01 Iniciar sesión

**Actor:** Estudiante

**Precondición:**
- El usuario debe estar registrado en la plataforma.

**Flujo principal:**
1. El estudiante accede a la pantalla de inicio de sesión.
2. Ingresa su correo electrónico y contraseña.
3. El sistema valida las credenciales.
4. El sistema identifica el rol del usuario.
5. El sistema concede el acceso y redirige al dashboard principal.

**Excepciones:**
- Si el correo no existe, el sistema muestra un mensaje de error.
- Si la contraseña es incorrecta, el sistema rechaza el acceso e informa al usuario.

---

#### UC-02 Crear tarea

**Actor:** Estudiante

**Precondición:**
- El estudiante debe tener una sesión iniciada.
- Debe existir al menos una materia registrada.

**Flujo principal:**
1. El estudiante selecciona la opción "Nueva tarea".
2. Ingresa el título de la tarea.
3. Ingresa una descripción opcional.
4. Selecciona la materia asociada.
5. Define la fecha límite.
6. Guarda la información.
7. El sistema registra la tarea y la muestra en la lista de tareas.

**Excepciones:**
- Si el título está vacío, el sistema muestra un mensaje de error.
- Si no se selecciona una materia, el sistema impide guardar la tarea.

---

#### UC-03 Modificar tarea

**Actor:** Estudiante

**Precondición:**
- El estudiante debe haber iniciado sesión.
- La tarea debe existir y pertenecer al usuario.

**Flujo principal:**
1. El estudiante selecciona una tarea existente.
2. El sistema muestra la información actual.
3. El estudiante modifica los campos deseados.
4. Guarda los cambios.
5. El sistema actualiza la información de la tarea.

**Excepciones:**
- Si la tarea no existe, el sistema informa el error.
- Si el usuario intenta editar una tarea que no le pertenece, el sistema deniega la acción.

---

#### UC-04 Marcar tarea como completada

**Actor:** Estudiante

**Precondición:**
- El estudiante debe haber iniciado sesión.
- La tarea debe existir y encontrarse activa.

**Flujo principal:**
1. El estudiante selecciona una tarea pendiente.
2. Marca la tarea como completada.
3. El sistema actualiza el estado de la tarea.
4. El sistema refleja el cambio en las estadísticas y listados correspondientes.

### Restricciones

**Técnicas**
- Aplicación web.
- Base de datos relacional.
- Acceso mediante navegador web.
- Desarrollo durante el semestre académico.

**Tiempo**
- Entrega dentro del período académico establecido por el curso.

**Alcance académico**
- Proyecto orientado a fines educativos.

### Supuestos

- Se asume que los estudiantes cuentan con acceso a internet y un dispositivo (computador, tablet o celular) con navegador web actualizado.
- Se asume que cada estudiante gestiona su información de forma individual, sin necesidad de compartir tareas o materias con otros usuarios.
- Se asume que el estudiante ingresará y actualizará sus tareas manualmente, sin integración automática con el sistema académico de la universidad.
- Se asume que el sistema se usará durante un periodo académico (semestre), sin necesidad de migrar datos entre periodos distintos.
- Se asume el uso de Bootstrap y Tailwind CSS como frameworks de estilos para la interfaz, según lo indicado por el docente del curso.
- Se asume que, al ser un proyecto con fines educativos, el volumen de usuarios y datos concurrentes será bajo, sin requerir consideraciones de escalabilidad a gran escala.

## 14. Arquitectura, base de datos y API

**Entorno:** ASP.NET Core MVC en .NET 10, Entity Framework Core 10 con SQL Server (local: SQL Server Express; producción: Azure SQL Database) y ASP.NET Core Identity para autenticación y roles.

**Carpetas:** `Controllers/` (controladores MVC), `Controllers/Api/` (API REST), `Models/` (modelos de dominio y modelos de vista), `Views/` (vistas Razor), `Data/` (DbContext e inicializador), `Migrations/` (una migración por cambio del modelo).

**Migraciones:** `InitialCreate` (Materias/Tareas), `AddIdentity` (tablas de Identity), `AddUsuarioAndEstado` (`UsuarioId` en Materia y enum `EstadoTarea` en Tarea).

**API REST** — documentación OpenAPI en `/openapi/v1.json` y UI interactiva Scalar en `/scalar/v1` (entorno de desarrollo). Consumida desde el frontend con `fetch` (`wwwroot/js/tareas.js`).

| Método | Ruta | Para qué sirve | Rol |
|------|------|------|------|
| GET | `/api/tareas` | Lista las tareas del estudiante autenticado. | Estudiante |
| GET | `/api/tareas/proximas-a-vencer` | Lista las tareas que vencen en las próximas 72 horas. | Estudiante |
| GET | `/api/tareas/estadisticas` | Devuelve conteos por estado y tasa de completadas. | Estudiante |
| POST | `/api/tareas/{id}/completar` | Marca como completada una tarea propia. | Estudiante |

## 15. Despliegue y configuración

| Elemento | Local | Producción |
|------|------|------|
| Dirección | `https://localhost:7218` | Dirección pública de Azure App Service *(pendiente de publicar, ver Fase 7 del plan de trabajo)* |
| Base de datos | SQL Server Express en el equipo del desarrollador | Azure SQL Database |
| Cadena de conexión | Secretos de usuario (`dotnet user-secrets`) | Configuración de la aplicación en Azure (`ConnectionStrings__DefaultConnection`) |
| Publicación | No aplica | Automática al integrar a `main` mediante GitHub Actions *(configurar en la Fase 7)* |

Los valores privados **nunca** están en el repositorio: `appsettings.json` no contiene cadenas de conexión y `appsettings.example.json` muestra solo los nombres de las variables.

## 16. Aportes por integrante

| Integrante | Aportes principales | Evidencia |
|---------|---------|---------|
| *(nombre del integrante 1)* | *(por ejemplo: autenticación, roles y panel administrador)* | PR #, commits |
| *(nombre del integrante 2)* | | |
| *(nombre del integrante 3)* | | |

Esta tabla se contrasta con el historial real de GitHub (commits y Pull Requests de cada integrante) y debe coincidir con él.

## Historial de cambios

| Fecha | Cambio realizado | Autor |
|---------|---------|---------|
| 2026-08-22 | Creación inicial del repositorio y estructura base del proyecto. | Equipo |
| 2026-08-22 | Adición de secciones de servicios y contacto. | Equipo |
| 2026-08-22 | Creación del documento SRS inicial. | Equipo |
| 2026-08-29 | Incorporación de imágenes y actualización del documento SRS. | Equipo |
| 2026-09-05 | Reorganización de carpetas del proyecto. | Equipo |
| 2026-09-19 | Adición de carpeta de pantallas para la actividad. | Equipo |
| 2026-09-25 | Conversión del SRS a documento de definición según los requisitos del entregable. | Equipo |
| 2026-09-25 | Adición de requerimientos funcionales, no funcionales, mockups e historias de usuario. | Equipo |
| 2026-09-25 | Reorganización del documento en las 13 secciones oficiales del entregable. | Equipo |
| 2026-11-28 | Configuración por entornos: `.gitignore`, `bin/`/`obj/` fuera del repositorio y cadena de conexión movida a secretos de usuario; `appsettings.example.json`. | Equipo |
| 2026-11-28 | Roles Estudiante/Administrador con semilla en desarrollo, aislamiento de datos por usuario (`UsuarioId`), enum `EstadoTarea` y migración `AddUsuarioAndEstado`. Motivo: cumplir RN-07, RNF-04 y la condición de roles del entregable final. | Equipo |
| 2026-11-28 | Filtros por estado y materia, búsqueda de texto, badges de estado, listas vacías con aviso y página 404. Motivo: RF-08, RF-09, RN-06 y condición de páginas de estado del entregable. | Equipo |
| 2026-11-28 | Sistema visual oscuro con sidebar y tarjetas, según los mockups del documento. | Equipo |
| 2026-11-28 | Dashboard con indicadores, próximas a vencer y tareas recientes; pantalla de Estadísticas (RF-10). | Equipo |
| 2026-11-28 | Panel administrativo: gestión de usuarios (bloqueo/desbloqueo) y estadísticas globales (RF-11, RF-12); pantallas separadas por rol. | Equipo |
| 2026-11-28 | API REST de tareas con documentación OpenAPI (Scalar), consumida desde el frontend con `fetch` y manejo de errores. Motivo: condición de API propia del entregable final. | Equipo |
| 2026-11-28 | Documento actualizado a versión final: estados de requerimientos, secciones 14–16 y colección de Postman. | Equipo |

## Referencias

- Documentación oficial de Tailwind CSS, consultada para la implementación de la interfaz según lo solicitado por el docente del curso. Enlace: tailwindcss.com/docs
- Documentación oficial de Bootstrap, consultada para la implementación de la interfaz según lo solicitado por el docente del curso. Enlace: getbootstrap.com/docs

## Declaración de uso de inteligencia artificial

Se utilizó Microsoft Copilot como herramienta de apoyo durante la elaboración de este proyecto.

Su uso se concentró principalmente en:

- Revisión y mejora de la redacción del documento de definición.
- Organización de secciones para ajustarse a los requisitos del entregable.
- Sugerencias para la estructura de requerimientos funcionales y no funcionales.
- Generación de propuestas visuales utilizadas como base para los mockups de la plataforma.

Durante la fase final del proyecto se utilizó adicionalmente un agente de IA (Kimi Code) como apoyo en la implementación, la verificación y la documentación: configuración por entornos, roles y aislamiento de datos, interfaz de usuario, dashboard, panel administrativo, API REST y pruebas. Todas las sugerencias fueron revisadas, comprendidas y verificadas por los integrantes antes de integrarse; las pruebas funcionales se ejecutaron contra la aplicación real.

La idea del proyecto, la definición del problema, los objetivos, las funcionalidades, las decisiones de alcance y la estructura general de la aplicación fueron definidas por los integrantes del equipo.

Las sugerencias proporcionadas por las herramientas fueron revisadas, adaptadas y modificadas antes de incorporarse al documento final.