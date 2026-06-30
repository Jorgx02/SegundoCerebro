# Registro de Decisiones de Arquitectura (ADR) - SegundoCerebro

Este documento registra las decisiones arquitectónicas importantes tomadas durante el desarrollo del proyecto. Servirá como base para redactar las secciones de "Evolución de la Arquitectura" y "Toma de Decisiones" en la memoria del TFG.

---

## ADR 001: Refactorización del Dominio - Separación de Cuentas y Tarjetas (En Implementación)

**Fecha:** (Registro inicial - Preparación para Módulo 1 Avanzado). **Inicio de implementación:** [Fecha actual]

### Contexto y Problema

En el diseño inicial del dominio, el enum `AccountType` incluía el valor `CreditCard` como un tipo de cuenta más (junto con `Checking`, `Savings`, `Investment`, `Cash`).
Durante la planificación para integrar la pasarela de pagos **Stripe (Sandbox)**, se identificó que este modelo simplificado no refleja la realidad financiera:

1. Una tarjeta no es una cuenta en sí misma, sino un **método de acceso/pago** vinculado a una cuenta bancaria real.
2. Una misma cuenta corriente (`Checking`) puede tener asociadas múltiples tarjetas (ej. una física y otra virtual).
3. Mantener `CreditCard` como tipo de cuenta impediría modelar correctamente la validación de tarjetas de Stripe contra cuentas bancarias existentes.

### Decisión

Se decide aplicar una refactorización basada en Domain-Driven Design (DDD):

1. **Eliminar** `CreditCard` del enumerado `AccountType`.
2. **Crear** una nueva entidad `Card` que represente una tarjeta física/virtual (con propiedades como `Last4Digits`, `Brand`, `StripePaymentMethodId`).
3. **Establecer** una relación de 1 a N entre `Account` y `Card`.
4. **Aplicar reglas de negocio:** Solo las cuentas de tipo `Checking` podrán tener tarjetas asociadas (rechazando cuentas de ahorro, inversión o efectivo).

### Archivos Afectados (Planificados)

- `src/Core/SegundoCerebro.Domain/Enums/AccountType.cs` (Modificación)
- `src/Core/SegundoCerebro.Domain/Entities/Card.cs` (Nueva creación)
- `src/Core/SegundoCerebro.Domain/Entities/Account.cs` (Modificación: Añadir navegación `ICollection<Card>`)
- `src/Infrastructure/SegundoCerebro.Infrastructure/Data/ApplicationDbContext.cs` (Modificación: Configuración de EF Core)
- Capa Application: DTOs, Validadores y Handlers relacionados con Cuentas.
- Capa Presentation (Frontend): `CreateAccountDialog.razor`, `EditAccountDialog.razor`, y `Accounts.razor`.

### Consecuencias

- **Positivas:** El modelo de datos es mucho más realista, robusto y escalable. Prepara el terreno perfecto para la futura integración con el SDK de Stripe.
- **Negativas:** Requiere refactorizar código existente y lanzar una nueva migración en Entity Framework Core.

### Relación con el TFG

Esta evolución demuestra capacidad de análisis crítico sobre el propio código y aplicación práctica de reingeniería para satisfacer nuevos requisitos (pasarelas de pago).

---

## ADR 002: Adopción de Clean Architecture y Patrón CQRS (Retrospectiva)

**Fecha:** Fase 1 - Inicio del proyecto

### Contexto y Problema

SegundoCerebro está concebido como una aplicación integral y escalable que agrupará múltiples módulos (Finanzas, Productividad, Hábitos). Utilizar una arquitectura tradicional en capas o monolítica fuertemente acoplada provocaría un "código espagueti", dificultando la introducción de nuevas funcionalidades y la realización de pruebas unitarias.

### Decisión

1. Adoptar **Clean Architecture**, invirtiendo el flujo de dependencias para que el núcleo (`Domain`) no dependa de ningún framework externo.
2. Implementar el patrón **CQRS (Command Query Responsibility Segregation)** utilizando la librería MediatR en la capa `Application`.

### Consecuencias

- **Positivas:** Altísima mantenibilidad. La lógica de negocio está completamente aislada y es 100% testeable. Cada caso de uso tiene su propio archivo (Single Responsibility Principle).
- **Negativas:** Mayor curva de aprendizaje y verbosidad inicial (requiere crear clases separadas para Commands, Queries, Handlers y Validators).

---

## ADR 003: Multi-tenancy y Aislamiento de Datos por Usuario (Retrospectiva)

**Fecha:** Fase 1 - Configuración de Base de Datos

### Contexto y Problema

El sistema será utilizado por múltiples usuarios. Bajo ninguna circunstancia un usuario debe ser capaz de consultar, modificar o eliminar las finanzas o tareas de otro usuario, incluso si intenta forzar los identificadores (ID) en la API.

### Decisión

1. Añadir la propiedad `UserId` a todas las entidades base del dominio.
2. Configurar **Global Query Filters** (Filtros de consulta globales) directamente en el DbContext de Entity Framework Core (`a => a.UserId == _currentUserService.UserId`).
3. Sobrescribir el método `SaveChangesAsync()` para asignar automáticamente el `UserId` en cada inserción nueva.

### Consecuencias

- **Positivas:** Seguridad por diseño. Los desarrolladores no necesitan acordarse de añadir `.Where(x => x.UserId == id)` en cada consulta LINQ en los repositorios, eliminando el error humano.
- **Negativas:** Obliga a tener siempre un contexto HTTP válido (`IHttpContextAccessor`) para inyectar el usuario actual en la capa de datos.

---

## ADR 004: Duplicación Controlada de DTOs en Blazor WebAssembly (Retrospectiva)

**Fecha:** Fase 1 y 2 - Integración del Frontend

### Contexto y Problema

El cliente Blazor WebAssembly necesita conocer la estructura de los datos que envía y recibe de la API (Modelos y Enumerados). Sin embargo, referenciar el proyecto `SegundoCerebro.Domain` o `Application` directamente desde Blazor acoplaría el Frontend al Backend, forzando a descargar DLLs con lógica de servidor en el navegador del cliente.

### Decisión

1. No compartir proyectos DLL entre el Frontend y el Backend.
2. Crear una réplica controlada de los DTOs y Enumerados en las carpetas `Models/` y `Models/Enums/` del proyecto `BlazorWasm`.

### Consecuencias

- **Positivas:** Cumplimiento estricto del desacoplamiento. El tamaño de descarga de la aplicación Blazor se mantiene pequeño y no se expone lógica sensible.
- **Negativas:** Ligera repetición de código (Trade-off aceptable). Si se añade un campo a un DTO en el backend, debe replicarse manualmente en el modelo equivalente del frontend.

---

## ADR 005: Integridad Referencial - Soft Delete y Borrado de Proyectos (Retrospectiva)

**Fecha:** Fase 1 y 2 - Lógica de Negocio

### Contexto y Problema

Cuando un usuario decide eliminar una estructura contenedora (como una Cuenta Bancaria o un Proyecto GTD), surgen conflictos con los elementos hijos (Transacciones o Tareas). Si se permiten borrados en cascada, el historial financiero del usuario o su registro de tareas desaparecería, rompiendo auditorías y estadísticas pasadas.

### Decisión

Se establecieron dos políticas claras a nivel de base de datos (`OnModelCreating`):

1. **Cuentas y Finanzas:** Uso de _Soft Delete_ (Desactivación Lógica). Las cuentas no se borran, se marcan como inhabilitadas (`IsActive = false`). Las relaciones con `Transaction` se configuran con `DeleteBehavior.Restrict`.
2. **Productividad:** Las Tareas (`TodoItems`) vinculadas a un `Project` se configuran con `DeleteBehavior.Cascade`. Si un proyecto se elimina, todas sus tareas asociadas se eliminan con él. Esto mantiene la integridad contextual de las tareas; una tarea sin su proyecto pierde su propósito original.

### Consecuencias

- **Positivas:** Mantiene intacto el historial financiero inmutable y asegura que no queden tareas "huérfanas" sin contexto en el sistema.
- **Negativas:** La interfaz de usuario tiene que saber diferenciar visualmente y gestionar qué elementos están "Inactivos" para no saturar las vistas.

---

## ADR 008: Inmutabilidad de Proyectos Completados y Lógica de Finalización

**Fecha:** Fase 2 - Implementación del Módulo de Productividad

### Contexto y Problema

Un proyecto completado es un registro histórico del trabajo realizado. Permitir su modificación o eliminación posterior podría corromper las métricas de productividad y el historial de logros. Además, el estado "Completado" debe ser un reflejo fiel de que todo el trabajo asociado ha concluido.

### Decisión

1.  **Inmutabilidad al Borrar:** Un `Project` con estado `Completed` no puede ser eliminado. La lógica de negocio en el `DeleteProjectCommandHandler` debe impedirlo explícitamente.
2.  **Condición de Finalización:** Un `Project` no puede ser marcado como `Completed` si alguna de sus `TodoItems` asociadas no está también en estado `Completed`. Esta validación se implementará en el `UpdateProjectCommandHandler`.

### Consecuencias

- **Positivas:** Se garantiza la integridad y el valor histórico de los datos de productividad. El estado del sistema es más coherente y fiable.
- **Negativas:** Añade complejidad a la lógica de negocio en los manejadores de comandos, que ahora deben consultar el estado de las entidades relacionadas antes de realizar una operación.

---

## ADR 006: Implementación de Logging Estructurado con Serilog y Seq

**Fecha:** Fase 2 - Configuración de Telemetría

### Contexto y Problema

A medida que el sistema crece en complejidad (múltiples módulos, usuarios aislados e integraciones de terceros previstas), depender de la consola estándar de .NET para leer errores de texto plano se vuelve insostenible. Se necesita un sistema de monitoreo que permita buscar, filtrar y analizar el comportamiento de la aplicación y su rendimiento (ej. filtrar errores por `UserId`).

### Decisión

1. Reemplazar el logger por defecto de Microsoft por **Serilog** para generar logs estructurados (JSON).
2. Utilizar **Seq** como sumidero (sink) y visor centralizado de logs, ejecutado localmente vía Docker.
3. Crear un **Pipeline Behavior en MediatR** (`LoggingBehavior`) en la capa de Aplicación para interceptar todos los Commands y Queries, registrando automáticamente el nombre del caso de uso, el tiempo de ejecución y posibles excepciones.

### Consecuencias

- **Positivas:** Observabilidad total del sistema sin acoplar código de infraestructura (logging) en la lógica de negocio. Trazabilidad absoluta de tiempos de respuesta.
- **Negativas:** Se añade una nueva dependencia de infraestructura externa (Seq) para el entorno de desarrollo y futura producción.

---

## ADR 007: Estándar de Documentación de Código (XML Comments)

**Fecha:** Fase 2 - Mejora de Calidad del Código

### Contexto y Problema

A medida que el código base crece, la lógica de negocio y las responsabilidades de cada clase pueden volverse ambiguas para nuevos desarrolladores o revisiones futuras. La falta de comentarios técnicos reduce la mantenibilidad y dificulta la generación automática de documentación.

### Decisión

1. Adoptar el uso de **Comentarios de Documentación XML (`///`)** para todas las entidades, interfaces, comandos, consultas y manejadores principales.
2. Los comentarios deben explicar el _porqué_ y la _responsabilidad_ de la clase/propiedad, no obviedades sobre la sintaxis.

### Consecuencias

- **Positivas:** Mejora sustancial en la legibilidad, activación de IntelliSense descriptivo en el IDE, y base sólida para generar documentación técnica para la memoria del TFG (y futura exportación a Swagger/OpenAPI).

---

## ADR 009: Diseño e Implementación del Módulo de Productividad (GTD)

**Fecha:** Fase 2 - Implementación del Módulo de Productividad

### Contexto y Problema

La aplicación necesita un módulo robusto para la gestión de tareas y proyectos, siguiendo principios de metodologías como GTD (Getting Things Done). Esto requiere no solo un CRUD básico, sino también la gestión de estados, relaciones entre proyectos y tareas, y reglas de negocio específicas para mantener la coherencia de los datos de productividad.

### Decisión

Se decide implementar un módulo completo de `Projects` y `TodoItems` siguiendo la arquitectura Clean/CQRS existente:

1.  **Entidades del Dominio:** Se crean las entidades `Project` y `TodoItem` con sus respectivos `Enums` para los estados (`ProjectStatus`, `TodoItemStatus`).
2.  **Relación y Cascada:** Se establece una relación de uno a muchos entre `Project` y `TodoItem`. Se configura `DeleteBehavior.Cascade` (ver ADR 005) para que al eliminar un proyecto, todas sus tareas asociadas se eliminen, ya que una tarea sin su proyecto pierde su contexto.
3.  **CQRS Completo:** Se implementan todos los `Commands` (Create, Update, Delete) y `Queries` (GetAll, GetById, GetByProject) necesarios para ambas entidades en la capa de `Application`.
4.  **Reglas de Negocio (ADR 008):** Se implementa la lógica en los `CommandHandlers` para:
    - Impedir la eliminación de un proyecto con estado `Completed`.
    - Impedir marcar un proyecto como `Completed` si tiene tareas pendientes.
5.  **Frontend en Blazor:** Se crean tres páginas principales para la interacción del usuario:
    - `Projects.razor`: Una tabla para listar, filtrar, crear, editar y eliminar proyectos.
    - `ProjectDetails.razor`: Una vista detallada de un proyecto que lista todas sus tareas asociadas y permite el CRUD completo sobre ellas.
    - `TodoItems.razor`: Una vista global que muestra todas las tareas de todos los proyectos, agrupadas por el nombre del proyecto para una visión general.
    - `KanbanBoard.razor`: Un tablero Kanban interactivo que permite visualizar y cambiar el estado de las tareas mediante drag-and-drop. Se utiliza el componente `MudDropContainer` para una gestión robusta del estado.
    - `Calendar.razor`: Una vista de calendario mensual que muestra todas las tareas con fecha de vencimiento (`DueDate`), proporcionando una perspectiva temporal para la planificación.
6.  **Actualización Optimista:** En la página de detalles del proyecto, el `CheckBox` para completar tareas implementa una actualización optimista de la UI. El estado visual cambia instantáneamente, y la llamada a la API se realiza en segundo plano, mejorando la percepción de velocidad para el usuario.

### Consecuencias

- **Positivas:** El módulo de productividad está completamente integrado en la arquitectura existente, es robusto y ofrece una experiencia de usuario fluida y visualmente rica gracias al tablero Kanban. Las reglas de negocio garantizan la integridad de los datos.
- **Positivas:** El módulo de productividad está completamente integrado en la arquitectura existente, es robusto y ofrece una experiencia de usuario fluida y visualmente rica gracias al tablero Kanban y la vista de calendario. Las reglas de negocio garantizan la integridad de los datos.
- **Negativas:** Aumenta la complejidad del dominio y el número de endpoints y componentes de la UI a mantener.

### Relación con el TFG

Esta implementación constituye el segundo gran módulo funcional de la aplicación, demostrando la escalabilidad de la arquitectura elegida y la capacidad de construir funcionalidades complejas sobre ella.

---

## ADR 010: Implementación del Seguimiento de Tiempo (Time Tracking)

**Fecha:** Fase 2 - Evolución del Módulo de Productividad

### Contexto y Problema

Para proporcionar una herramienta de productividad completa, no basta con gestionar tareas; es crucial poder medir el tiempo invertido en ellas. Esto permite a los usuarios analizar su rendimiento, facturar horas de trabajo y entender mejor en qué invierten su tiempo. El módulo de productividad necesitaba una forma integrada y no intrusiva de registrar estos tiempos.

### Decisión

Se decide implementar un sistema de seguimiento de tiempo a nivel de `TodoItem`:

1.  **Nueva Entidad `TimeLog`:** Se crea una entidad `TimeLog` para almacenar cada intervalo de tiempo trabajado. Contiene `StartTime`, `EndTime?`, y una relación de muchos a uno con `TodoItem`.
2.  **Lógica de Negocio en CQRS:** Se implementan los comandos `StartTimeLogCommand` y `StopTimeLogCommand`. La lógica de negocio en sus manejadores asegura que no se pueda iniciar un nuevo cronómetro si ya hay uno activo para la misma tarea.
3.  **Cálculos en AutoMapper:** Para optimizar las consultas del frontend, se enriquece el `TodoItemDto` con dos propiedades calculadas a través de AutoMapper:
    - `TotalTimeTracked`: Suma la duración de todos los `TimeLog` completados para una tarea.
    - `IsCurrentlyTracking`: Un booleano que indica si existe un `TimeLog` activo (con `EndTime` nulo).
4.  **Integración en la UI:** Se añade un componente de cronómetro (botón Play/Stop y contador) directamente en las tarjetas de tarea del `KanbanBoard.razor` y en las listas de `ProjectDetails.razor`, ofreciendo una experiencia de usuario consistente.

### Consecuencias

- **Positivas:** El módulo de productividad se vuelve significativamente más potente. La integración directa en la UI existente hace que la funcionalidad sea muy accesible. La arquitectura permite futuras ampliaciones, como reportes de tiempo.
- **Negativas:** Aumenta la carga de consultas a la base de datos, ya que cada carga de tareas ahora debe incluir sus `TimeLogs` asociados (`.Include(t => t.TimeLogs)`).

---

## ADR 011: Implementación del Módulo de Hábitos y Bienestar (v3.0)

**Fecha:** Fase 3 - Implementación del Módulo de Hábitos

### Contexto y Problema

Para cumplir con la visión de ser una aplicación de gestión de vida integral, se necesita un módulo que permita a los usuarios definir, seguir y analizar sus hábitos personales. Esto no solo complementa la productividad (Módulo 2), sino que también introduce un componente de bienestar y autodisciplina, clave para el desarrollo personal.

### Decisión

Se decide implementar un módulo completo de seguimiento de hábitos (`Habits`) siguiendo la arquitectura Clean/CQRS existente:

1.  **Entidades del Dominio:** Se crean dos nuevas entidades:
    - `Habit`: Representa el hábito en sí (nombre, descripción, frecuencia, etc.).
    - `HabitLog`: Almacena un registro de cumplimiento para un hábito en una fecha concreta. Se establece una restricción única en la base de datos para el par `(HabitId, Date)` para evitar duplicados.
2.  **CQRS Completo:** Se implementan todos los `Commands` (Create, Update, Delete, ToggleCompletion) y `Queries` (GetHabitsForTracker) necesarios en la capa de `Application`.
3.  **Lógica de Rachas (Streaks):** Para añadir un elemento de gamificación y motivación, se implementa una lógica de negocio en el backend (`GetHabitsForTrackerQueryHandler`) que calcula dos métricas clave en cada carga:
    - `CurrentStreak`: El número de días consecutivos que el hábito se ha completado hasta hoy (o ayer).
    - `LongestStreak`: La racha más larga de días consecutivos completados en todo el historial del hábito.
4.  **Frontend Interactivo en Blazor:** Se crea una página `Habits.razor` que funciona como un tracker semanal:
    - Utiliza un `MudTable` para mostrar los hábitos en filas y los días de la semana en columnas.
    - Permite la navegación entre semanas ("Semana anterior", "Siguiente semana", "Hoy").
    - Un `MudCheckBox` en cada celda permite marcar/desmarcar el cumplimiento de un hábito para un día específico, llamando al endpoint `toggle` de la API.
5.  **Estrategia de Actualización de UI:** Tras marcar o desmarcar un hábito, se realiza una recarga completa de los datos (`LoadHabits()`). Esta decisión, aunque menos performante que una actualización optimista, es crucial para garantizar que las rachas calculadas en el backend se reflejen de forma inmediata y precisa en la interfaz.

### Consecuencias

- **Positivas:** Se añade un módulo funcional de alto valor para el usuario, con una interfaz moderna e interactiva. La gamificación mediante rachas aumenta el potencial de engagement. La arquitectura demuestra de nuevo su capacidad para escalar y albergar módulos complejos.
- **Negativas:** El cálculo de la racha más larga (`LongestStreak`) podría volverse computacionalmente costoso si un usuario tiene un historial de varios años. La recarga completa de datos en cada interacción de `toggle` introduce una pequeña latencia, un trade-off consciente a favor de la consistencia de los datos.

### Relación con el TFG

Esta implementación constituye el tercer gran módulo funcional de la aplicación. Demuestra la capacidad de diseñar e implementar funcionalidades centradas en el usuario, incluyendo lógica de negocio no trivial (cálculo de rachas) y decisiones de diseño de UX/UI (tracker semanal, estrategia de recarga).

---

## ADR 012: Refinamiento de la Interfaz de Usuario del Módulo de Hábitos

**Fecha:** Fase 3 - Refinamiento del Módulo de Hábitos

### Contexto y Problema

La implementación inicial del tracker de hábitos era funcional, pero presentaba varias áreas de mejora en la experiencia de usuario (UX). Los formularios de creación/edición eran poco intuitivos (requerían entrada manual de iconos) y la visualización de los hábitos semanales era confusa, ya que mostraba una parrilla de siete días.

### Decisión

Se decide aplicar una serie de refinamientos de UX en los componentes de Blazor:

1.  **Formularios de Creación/Edición Mejorados:**
    - **Frecuencia:** El `MudSelect` para la frecuencia se convierte en un tipo que acepta nulos (`HabitFrequency?`) y se marca como `Required`, forzando al usuario a hacer una elección explícita en lugar de tener un valor por defecto.
    - **Iconos:** Se reemplaza el campo de texto por un `MudPopover` que contiene una cuadrícula de `MudIconButton`, ofreciendo una selección de iconos puramente visual e intuitiva.
    - **Color (Decisión de Eliminación):** Tras un intento de implementar un selector de color que presentó complejidades de UI (posicionamiento del popover), se tomó la decisión pragmática de eliminar por completo la personalización de colores. Se priorizó la simplicidad y estabilidad de la interfaz sobre una característica no esencial.
2.  **Visualización Diferenciada por Frecuencia:**
    - La tabla en `Habits.razor` ahora utiliza lógica condicional (`@if (context.Frequency == HabitFrequency.Daily)`).
    - **Hábitos Diarios:** Renderizan una celda con un `MudCheckBox` para cada uno de los 7 días de la semana.
    - **Hábitos Semanales:** Renderizan una única celda que abarca las 7 columnas (`<td colspan="7">`) con un único `MudCheckBox` centrado, clarificando que la acción se realiza una vez por semana.

### Consecuencias

- **Positivas:** La experiencia de usuario mejora significativamente. Los formularios son más intuitivos y el tracker es menos ambiguo. La decisión de eliminar la funcionalidad de color demuestra un enfoque pragmático, favoreciendo la simplicidad y la robustez.
- **Negativas:** El código Razor en `Habits.razor` gana en complejidad debido a la lógica de renderizado condicional, un trade-off aceptable por la mejora en la claridad de la interfaz.

### Relación con el TFG

Este ADR demuestra un proceso de desarrollo iterativo, centrado en la usabilidad y la experiencia de usuario. Documenta cómo se refina una funcionalidad basándose en su uso y cómo se toman decisiones pragmáticas (como eliminar una característica problemática) para mejorar la calidad general del producto.

---

_(Nuevas decisiones se añadirán a continuación...)_
