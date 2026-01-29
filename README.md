# 🧠 SegundoCerebro

<div align="center">

![.NET 9.0 SDK](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?style=for-the-badge&logo=blazor)
![PostgreSQL 15+](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

*Tu asistente personal inteligente para la gestión completa de vida*

[🚀 Demo en Vivo](#) • [📖 Documentación](#documentación) • [🐛 Reportar Bug](../../issues) • [💡 Solicitar Feature](../../issues)

</div>

---

## 📋 Tabla de Contenidos

- [Acerca del Proyecto](#-acerca-del-proyecto)
- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Tecnologías](#-tecnologías)
- [Instalación](#-instalación)
- [Uso](#-uso)
- [Roadmap](#-roadmap)
- [API Reference](#-api-reference)
- [Contribuir](#-contribuir)
- [Licencia](#-licencia)
- [Contacto](#-contacto)

---

## 🎯 Acerca del Proyecto

**SegundoCerebro** es una aplicación integral de gestión personal que actúa como tu "segundo cerebro" digital. Combina gestión financiera, organización de tareas, seguimiento de hábitos, y análisis de productividad en una sola plataforma potente y fácil de usar.

### 🌟 Visión
Convertirse en el ecosistema definitivo para la organización personal, donde cada aspecto de tu vida productiva esté conectado e inteligentemente optimizado.

### 🎯 Misión
Empoderar a las personas para que tomen el control total de su vida financiera, productiva y personal a través de herramientas intuitivas y análisis inteligentes.

---

## ✨ Características

### 💰 **Módulo Financiero** (v1.0 - Completado ✅)
- 📊 **Dashboard Financiero**: Vista completa de tu situación económica
- 🏦 **Gestión de Cuentas**: Múltiples tipos (efectivo, bancarias, inversiones)
- 💸 **Transacciones Completas**: Sistema CRUD completo con filtros avanzados
- 📈 **Presupuestos Dinámicos**: Creación y seguimiento de presupuestos mensuales
- 📊 **Reportes Avanzados**: Análisis de tendencias y proyecciones
- 🏷️ **Categorías Personalizables**: Sistema flexible de clasificación
- 🔍 **Filtros Inteligentes**: Por fecha, cuenta, categoría y tipo
- 💱 **Gestión Multi-Cuenta**: Transferencias y balances consolidados

### 🎯 **Módulo de Productividad** (v2.0 - Planificado)
- ✅ **Gestión de Tareas**: Sistema GTD completo
- 📅 **Planificación de Proyectos**: Metodologías ágiles integradas
- ⏰ **Time Tracking**: Seguimiento detallado del tiempo
- 🎯 **Objetivos SMART**: Definición y seguimiento de metas

### 🔄 **Módulo de Hábitos** (v3.0 - Planificado)
- 📊 **Habit Tracker**: Seguimiento diario de hábitos
- 🎮 **Gamificación**: Sistema de puntos y logros
- 📈 **Análisis de Patrones**: IA para identificar patrones de comportamiento

### 🤖 **Inteligencia Artificial** (v4.0 - Futuro)
- 🧠 **Asistente Personal**: Chatbot inteligente integrado
- 💡 **Recomendaciones**: Sugerencias personalizadas basadas en datos
- 🔍 **Análisis Predictivo**: Predicciones financieras y de productividad

---

## 🏗️ Arquitectura

SegundoCerebro utiliza **Clean Architecture** con CQRS (Command Query Responsibility Segregation) para garantizar escalabilidad, mantenibilidad y testabilidad.

```
📁 SegundoCerebro/
├── 📂 src/
│   ├── 📂 Core/
│   │   ├── 📂 SegundoCerebro.Domain/          # Entidades de negocio
│   │   └── 📂 SegundoCerebro.Application/     # Lógica de aplicación (CQRS)
│   ├── 📂 Infrastructure/
│   │   └── 📂 SegundoCerebro.Infrastructure/  # Acceso a datos y servicios externos
│   └── 📂 Presentation/
│       ├── 📂 SegundoCerebro.WebAPI/          # API REST
│       ├── 📂 SegundoCerebro.BlazorWasm/      # Frontend Web
│       └── 📂 SegundoCerebro.Maui/            # App Móvil (Futuro)
└── 📂 tests/                                  # Tests automatizados
```

### 🔧 Patrones Implementados
- **Clean Architecture**: Separación clara de responsabilidades
- **CQRS + MediatR**: Separación de comandos y consultas
- **Repository Pattern**: Abstracción del acceso a datos
- **Unit of Work**: Gestión de transacciones
- **Dependency Injection**: Inversión de dependencias

---

## 🛠️ Tecnologías

### Backend
- **Framework**: .NET 9.0
- **Lenguaje**: C# 12
- **ORM**: Entity Framework Core 9.0
- **Base de Datos**: PostgreSQL 15+
- **Mediator**: MediatR
- **Validación**: FluentValidation
- **Mapping**: AutoMapper
- **Testing**: xUnit, FluentAssertions

### Frontend
- **Framework**: Blazor WebAssembly
- **UI Components**: MudBlazor
- **Storage**: Blazored LocalStorage
- **HTTP Client**: System.Net.Http
- **Validación**: FluentValidation

### DevOps & Herramientas
- **Containerization**: Docker & Docker Compose
- **CI/CD**: GitHub Actions
- **Monitoring**: Serilog + Seq
- **Documentation**: Swagger/OpenAPI
- **IDE**: Visual Studio Code / Visual Studio 2022

---

## 🚀 Instalación

### Prerrequisitos
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Node.js 18+](https://nodejs.org/) (opcional, para herramientas de desarrollo)

### 1. Clonar el Repositorio
```bash
git clone https://github.com/Jorgx02/SegundoCerebro.git
cd SegundoCerebro
```

### 2. Configurar Base de Datos
```bash
# Crear base de datos PostgreSQL
createdb segundo_cerebro_db

# Configurar connection string en appsettings.Development.json
```

### 3. Restaurar Dependencias
```bash
dotnet restore
```

### 4. Ejecutar Migraciones
```bash
cd src/Presentation/SegundoCerebro.WebAPI
dotnet ef database update
```

### 5. Ejecutar la Aplicación
```bash
# Terminal 1: API Backend
cd src/Presentation/SegundoCerebro.WebAPI
dotnet run

# Terminal 2: Frontend Blazor
cd src/Presentation/SegundoCerebro.BlazorWasm
dotnet run
```

### 6. Acceder a la Aplicación
- **Frontend**: http://localhost:5173
- **API**: http://localhost:7099
- **Swagger**: http://localhost:7099/swagger

---

## 📱 Uso

### Gestión Financiera Básica

1. **Crear Cuenta Bancaria**
   ```
   Navegar a "Accounts" → "New Account"
   Completar información de la cuenta
   Guardar y activar
   ```

2. **Registrar Transacciones**
   ```
   Ir a "Transactions" → "New Transaction"
   Seleccionar tipo (Ingreso/Gasto)
   Elegir cuenta y categoría
   Añadir descripción y monto
   ```

3. **Filtrar y Buscar**
   ```
   Usar filtros por:
   - Rango de fechas
   - Tipo de transacción
   - Cuenta específica
   - Categoría
   ```

4. **Crear Presupuesto**
   ```
   Acceder a "Budgets" → "Create Budget"
   Definir límites por categoría
   Establecer período
   ```

5. **Ver Reportes**
   ```
   Dashboard principal muestra:
   - Balance total actualizado
   - Transacciones recientes
   - Gastos mensuales por categoría
   - Tendencias y proyecciones
   - Estado de presupuestos
   ```

---

## 🗺️ Roadmap

### 📍 **Fase 1: Fundación Financiera** (Q1 2026) ✅ **COMPLETADA**
- [x] Arquitectura Clean + CQRS ✅
- [x] Gestión de cuentas bancarias ✅
- [x] Sistema de transacciones completo ✅
- [x] CRUD de transacciones con filtros ✅
- [x] Categorización de gastos/ingresos ✅
- [x] Presupuestos básicos ✅
- [x] Dashboard financiero funcional ✅
- [x] API REST completa (20+ endpoints) ✅
- [x] Frontend Blazor responsivo ✅
- [x] Base de datos PostgreSQL configurada ✅
- [x] **Sistema completamente operativo** ✅

### 🚧 **Fase 2: Productividad Personal** (Q1 2026) - **PRÓXIMO**
- [ ] Gestión de tareas (GTD)
- [ ] Proyectos y objetivos
- [ ] Time tracking
- [ ] Calendario integrado
- [ ] Recordatorios y notificaciones
- [ ] Sincronización multi-dispositivo

### 🎯 **Fase 3: Hábitos y Bienestar** (Q2 2026)
- [ ] Habit tracker diario
- [ ] Métricas de bienestar
- [ ] Gamificación (puntos, logros)
- [ ] Análisis de patrones
- [ ] Integración con dispositivos wearables
- [ ] Reportes de progreso

### 🤖 **Fase 4: Inteligencia Artificial** (Q3 2026)
- [ ] Chatbot asistente personal
- [ ] Análisis predictivo financiero
- [ ] Recomendaciones automáticas
- [ ] OCR para facturas/recibos
- [ ] Categorización automática inteligente
- [ ] Alertas proactivas

### 📱 **Fase 5: Ecosistema Móvil** (Q3 2026)
- [ ] App nativa con .NET MAUI
- [ ] Sincronización offline-first
- [ ] Notificaciones push
- [ ] Widgets nativos
- [ ] Integración con servicios del sistema
- [ ] Apple/Google Wallet integration

### 🌐 **Fase 6: Plataforma Social** (Q4 2026)
- [ ] Sharing de objetivos
- [ ] Competencias familiares/amigos
- [ ] Comunidad de usuarios
- [ ] Marketplace de templates
- [ ] Coaches y expertos integrados
- [ ] API pública para desarrolladores

---

## 📊 Métricas del Proyecto

### Estado Actual (v1.0) ✅ **APLICACIÓN OPERATIVA**
- ✅ **Backend**: 100% funcional y compilando
- ✅ **Base de datos**: PostgreSQL configurada y operativa
- ✅ **API REST**: 20+ endpoints funcionando
- ✅ **Frontend completo**: Todas las páginas principales implementadas
- ✅ **Sistema de Cuentas**: CRUD completo y funcional
- ✅ **Sistema de Transacciones**: CRUD completo con filtros avanzados
- ✅ **UI/UX**: Layout profesional y responsivo con MudBlazor
- ✅ **Navegación**: Sistema de menús y enrutado completo
- 🚧 **Tests**: En desarrollo
- 🚧 **Documentación**: En progreso

### Funcionalidades Implementadas
- **👤 Gestión de Cuentas**: Crear, editar, eliminar, listar
- **💸 Gestión de Transacciones**: CRUD completo con filtros
- **🏷️ Sistema de Categorías**: Categorización inteligente
- **🔍 Filtros Avanzados**: Por fecha, tipo, cuenta, categoría
- **📊 Dashboard**: Vista general de finanzas
- **🎨 UI Profesional**: Diseño moderno y responsivo
- **🔄 Navegación**: Menú lateral y enrutado funcional

### Estadísticas de Código
```
- Líneas de código: ~12,000+
- Proyectos: 5
- Entidades de dominio: 6+
- Endpoints API: 20+
- Páginas frontend: 8+
- Componentes UI: 15+
- Tests unitarios: En desarrollo
- Cobertura: TBD
- Dependencias: ~30
```

---

## 🔌 API Reference

### Accounts
```http
GET    /api/accounts              # Obtener todas las cuentas
GET    /api/accounts/{id}         # Obtener cuenta por ID
POST   /api/accounts              # Crear nueva cuenta
PUT    /api/accounts/{id}         # Actualizar cuenta
DELETE /api/accounts/{id}         # Eliminar cuenta
GET    /api/accounts/{id}/balance # Obtener balance de cuenta
```

### Transactions
```http
GET    /api/transactions              # Obtener todas las transacciones
GET    /api/transactions/{id}         # Obtener transacción por ID
POST   /api/transactions              # Crear nueva transacción
PUT    /api/transactions/{id}         # Actualizar transacción
DELETE /api/transactions/{id}         # Eliminar transacción
GET    /api/transactions/account/{id} # Transacciones por cuenta
GET    /api/transactions/category/{id}# Transacciones por categoría
GET    /api/transactions/daterange    # Transacciones por rango de fechas
```

### Categories
```http
GET    /api/categories            # Obtener todas las categorías
GET    /api/categories/{id}       # Obtener categoría por ID
POST   /api/categories            # Crear nueva categoría
PUT    /api/categories/{id}       # Actualizar categoría
DELETE /api/categories/{id}       # Eliminar categoría
```

### Budgets
```http
GET    /api/budgets               # Obtener todos los presupuestos
GET    /api/budgets/{id}          # Obtener presupuesto por ID
POST   /api/budgets               # Crear nuevo presupuesto
PUT    /api/budgets/{id}          # Actualizar presupuesto
DELETE /api/budgets/{id}          # Eliminar presupuesto
```

### Reports
```http
GET    /api/reports/financial-summary  # Resumen financiero
GET    /api/reports/monthly-breakdown  # Desglose mensual
GET    /api/reports/category-analysis  # Análisis por categorías
```

**📋 Documentación completa**: [Swagger UI](http://localhost:7099/swagger)

---

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Aquí tienes cómo puedes ayudar:

### 1. Fork del Proyecto
```bash
git clone https://github.com/Jorgx02/SegundoCerebro.git
cd SegundoCerebro
```

### 2. Crear Branch de Feature
```bash
git checkout -b feature/nueva-caracteristica
```

### 3. Commit Changes
```bash
git commit -m "Add: nueva característica increíble"
```

### 4. Push a Branch
```bash
git push origin feature/nueva-caracteristica
```

### 5. Abrir Pull Request

### 📋 Guidelines de Contribución
- Seguir Clean Code principles
- Escribir tests para nuevas funcionalidades
- Actualizar documentación relevante
- Usar Conventional Commits
- Respetar la arquitectura establecida

### 🐛 Reportar Bugs
Usa nuestro [template de issues](../../issues/new) e incluye:
- Descripción detallada del problema
- Pasos para reproducir
- Comportamiento esperado vs actual
- Screenshots (si aplica)
- Información del entorno

---

## 📄 Licencia

Distribuido bajo la Licencia MIT. Ver `LICENSE` para más información.

---

## 📞 Contacto

**Jorge** - [@tu-twitter](https://twitter.com/tu-usuario) - hidalgofernandezjorge@gmail.com

**Link del Proyecto**: [https://github.com/Jorgx02/SegundoCerebro](https://github.com/Jorgx02/SegundoCerebro)

---

## 🙏 Agradecimientos

- [MudBlazor](https://mudblazor.com/) - Componentes UI
- [MediatR](https://github.com/jbogard/MediatR) - Patrón Mediator
- [FluentValidation](https://fluentvalidation.net/) - Validaciones
- [Entity Framework Core](https://docs.microsoft.com/ef/) - ORM
- [AutoMapper](https://automapper.org/) - Object mapping

---

<div align="center">

**[⬆ Volver arriba](#-segundocerebro)**

Made with ❤️ by [Jorge](https://github.com/Jorgx02)

</div>