# ProductManager

Prueba tecnica para Redlabs: aplicacion web para la gestion de productos con autenticacinn JWT, operaciones CRUD y generación de reportes en PDF.

## Tecnologías

- **Backend**: .NET 8 Web API, Entity Framework Core 8, SQL Server, JWT, QuestPDF
- **Frontend**: Next.js 15, TypeScript, Tailwind CSS
- **Infraestructura**: Docker Compose (SQL Server)

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) con Docker Compose

## Cómo ejecutar el proyecto

### 1. Levantar la base de datos

Desde la raíz del proyecto:

```bash
docker compose up
```

Esto inicia SQL Server en el puerto `1433`.

### 2. Ejecutar el backend

En otra terminal:

```bash
cd backend
dotnet run --project src/ProductManager.Api
```

### 3. Ejecutar el frontend

En otra terminal:

```bash
cd frontend
npm install
npm run dev
```

La aplicación quedará disponible en `http://localhost:3000`.

### 4. Usar la aplicación

1. Abrir `http://localhost:3000`
2. Registrarse

## Nota sobre credenciales y secretos

Los valores de conexión a base de datos y la clave JWT que aparecen en `appsettings.json` los puse quemados para que cualquier persona pueda ejecutar el proyecto facilmente después de clonarlo.

La URL de la API en el frontend está hardcodeada a `http://localhost:5214/api` para evitar depender de archivos `.env` adicionales.
