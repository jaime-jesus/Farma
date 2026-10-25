# 💊 PharmaCare Suite .NET — Sistema de Control y Gestión Farmacéutica

[![.NET 8](https://img.shields.io/badge/.NET-8.0%20(LTS)-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor Server](https://img.shields.io/badge/Blazor-Interactive%20Server-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4)](https://learn.microsoft.com/ef/core/)
[![SQLite](https://img.shields.io/badge/Database-SQLite%20(Zero--Config)-003B57?logo=sqlite&logoColor=white)](https://www.sqlite.org/)
[![Architecture](https://img.shields.io/badge/Architecture-Layered%20%2B%20SOLID-success)](https://learn.microsoft.com/dotnet/architecture/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

**PharmaCare Suite .NET** es una aplicación web empresarial moderna y demostrativa desarrollada con **C# y .NET 8**, diseñada bajo una **arquitectura desacoplada en capas**, principios **SOLID** y el patrón **Result**. 

Resuelve una necesidad crítica del sector farmacéutico: el control riguroso de inventario, trazabilidad de lotes, alertas sanitarias de vencimiento y emisión oficial de reportes valorizados listos para imprimir en **PDF**.

---

## 🎯 Características Principales

1. **🔐 Pantalla de Bienvenida y Autenticación:**
   - Control de acceso para farmacéuticos y directores técnicos.
   - Atajo de credenciales demo para evaluadores de GitHub (`admin` / `admin123`).

2. **🎛️ Ventana Principal con Hub Central de Operaciones:**
   - **Métricas KPI en vivo:** Total de catálogo, valor total valorizado, productos próximos a vencer y stock crítico.
   - **4 Botones de Acción Centrales:**
     * 📦 **Ingreso / Gestión:** Modal interactivo para dar de alta o editar medicamentos con validación de datos en tiempo real (CRUD).
     * 📄 **Reportes en PDF:** Generador de reportes farmacéuticos con vista previa y botón de impresión optimizado para PDF (incluye membrete oficial, resumen valorizado y firmas de auditoría).
     * ⚠️ **Alertas Sanitarias:** Filtro preventivo de lotes por vencer (<=60 días) y stock bajo para evitar quiebres de existencias.
     * 🛒 **Orden de Compra / Reposición:** Emisión directa de informes de reabastecimiento urgente.

3. **📊 Catálogo Interactivo en Tiempo Real:**
   - Búsqueda instantánea por nombre comercial, principio activo, número de lote o código SKU.
   - Filtros dinámicos por categoría terapéutica y laboratorio farmacéutico.
   - Indicadores de estado visual: 🟢 Óptimo, 🟡 Próximo a vencer, 🔴 Stock crítico.

4. **⚡ Experiencia "Zero-Setup" (Clone & Run):**
   - Base de datos **SQLite local portátil (`pharmacare.db`)**.
   - Se crea automáticamente y se precarga con **14 productos farmacéuticos de muestra realistas** (*Seed Data*) al ejecutar el proyecto por primera vez. **No requiere instalar SQL Server ni motores externos.**

---

## 🏗️ Arquitectura y Patrones de Diseño

El proyecto sigue una **Arquitectura en Capas Limpia**, asegurando que la interfaz de usuario nunca toque directamente la base de datos:

```text
┌────────────────────────────────────────────────────────┐
│             Capa de Presentación (Blazor UI)           │
│  - Home.razor (Login + Dashboard Central)              │
│  - ProductModal.razor (CRUD Modal)                     │
│  - ReportModal.razor (PDF Export & Print)              │
│  - AlertsModal.razor & DeleteConfirmModal.razor        │
└──────────────────────────┬─────────────────────────────┘
                           │ (Uso de DTOs y ViewModels)
                           ▼
┌────────────────────────────────────────────────────────┐
│        Capa de Negocio / Servicios (Interfaces)        │
│  - IProductService / ProductService                    │
│  - IReportService / ReportService                      │
│  - IAuthService / AuthService                          │
│  - Patrón Result<T> (Manejo de errores sin excepciones)│
└──────────────────────────┬─────────────────────────────┘
                           │ (Inyección de Dependencias)
                           ▼
┌────────────────────────────────────────────────────────┐
│             Capa de Acceso a Datos (Persistencia)      │
│  - AppDbContext (Entity Framework Core 8)              │
│  - DbInitializer (Carga automática de datos semilla)   │
│  - SQLite (Data Source=pharmacare.db)                  │
└────────────────────────────────────────────────────────┘
```

### Principios SOLID Aplicados:
* **Single Responsibility (S):** Los componentes solo presentan información; la lógica de negocio y validación reside en los servicios (`ProductService`).
* **Open/Closed (O):** Nuevos tipos de reportes pueden agregarse sin alterar la estructura central de la base de datos.
* **Dependency Inversion (D):** Todos los componentes dependen de abstracciones (`IProductService`, `IReportService`, `IAuthService`) registradas mediante el contenedor de Inyección de Dependencias nativo de ASP.NET Core.

---

## 📂 Estructura del Repositorio

```text
PharmaCareBlazor/
├── Common/
│   └── Result.cs                   # Implementación del Result Pattern (Result y Result<T>)
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor        # Barra superior, perfil y estado de SQLite
│   │   └── MainLayout.razor.css
│   ├── Pages/
│   │   └── Home.razor              # Pantalla de Bienvenida, Login y Dashboard
│   └── Shared/
│       ├── ProductModal.razor      # Modal de Ingreso / Edición con DataAnnotations
│       ├── ReportModal.razor       # Modal de Reportes con CSS @media print para PDF
│       ├── AlertsModal.razor       # Modal de Alertas Sanitarias de Vencimiento y Stock
│       └── DeleteConfirmModal.razor# Modal seguro de confirmación de eliminación
├── Data/
│   ├── AppDbContext.cs             # DbContext con Fluent API y mapeo SQLite
│   └── DbInitializer.cs            # Datos semilla de medicamentos de demostración
├── DTOs/
│   └── ProductDtos.cs              # ProductCreateUpdateDto, ProductDto, MetricsDto, ReportFilterDto
├── Models/
│   └── Product.cs                  # Entidad central del dominio con propiedades calculadas
├── Properties/
│   └── launchSettings.json         # Perfiles de ejecución HTTP/HTTPS
├── Services/
│   ├── Interfaces/                 # IProductService, IReportService, IAuthService
│   └── Implementations/            # ProductService, ReportService, AuthService
├── wwwroot/
│   └── app.css                     # Sistema de diseño moderno (Clinical Dark Theme & @media print)
├── appsettings.json                # Cadena de conexión a SQLite
└── Program.cs                      # Registro de dependencias y arranque del ciclo de vida
```

---

## 🚀 Cómo Ejecutar el Proyecto en tu Máquina

### Requisitos Previos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior instalado.

### Pasos:

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/tu-usuario/PharmaCareBlazor.git
   cd PharmaCareBlazor
   ```

2. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```

3. **Abrir en el navegador:**
   Ingresa a: [http://localhost:5240](http://localhost:5240)

4. **Credenciales de Acceso:**
   * **Usuario:** `admin`
   * **Contraseña:** `admin123`
   *(O haz clic directamente en el botón de autorrelleno en la pantalla de bienvenida)*.

---

## 🖨️ Exportación a PDF

El sistema implementa una hoja de estilos especializada para impresión (`@media print`):
* Al pulsar **"Imprimir / Guardar en PDF"** en el modal de reportes, el navegador activa el diálogo de impresión con un formato de reporte médico con membrete, métricas calculadas y líneas de firma, ocultando automáticamente el dashboard, botones y barras de navegación.

---

## 👨‍💻 Autor
Proyecto diseñado y desarrollado como demostración técnica de capacidades en **C#, .NET 8, Blazor y Arquitecturas Limpias**.
