# EzyClassroomz

A modern classroom management application with React TypeScript frontend and ASP.NET Core Web API backend.

## Project Structure

```
EzyClassroomz/
├── backend/
│   └── EzyClassroomz.Api/      # ASP.NET Core Web API
├── frontend/                    # React + TypeScript + Vite
└── EzyClassroomz.sln           # .NET Solution file
```

## Prerequisites

- .NET 8.0 SDK or later
- Node.js 18+ and npm
- Visual Studio 2022 or VS Code (recommended)

## Getting Started

### Backend (API)

1. Navigate to the backend directory:
   ```bash
   cd backend/EzyClassroomz.Api
   ```

2. Restore dependencies and run:
   ```bash
   dotnet restore
   dotnet run
   ```

3. The API will be available at:
   - HTTP: http://localhost:5000
   - HTTPS: https://localhost:5001
   - Swagger UI: http://localhost:5000/swagger

### Frontend

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Run the development server:
   ```bash
   npm run dev
   ```

4. The app will be available at: http://localhost:5173

## Available Endpoints

### API
- `GET /api/health` - Health check endpoint

## Technology Stack

### Frontend
- React 18
- TypeScript
- Vite
- CSS3

### Backend
- ASP.NET Core 8.0
- Swagger/OpenAPI
- CORS enabled for local development

## Development Notes

- The frontend is configured to proxy API requests to the backend
- CORS is configured to allow requests from http://localhost:5173
- Both projects run independently but communicate via HTTP