# TASKe

A full-stack task management application built with ASP.NET Core, React, and SQL Server.

## Features

- **Authentication & Roles**: User registration and login with role-based permissions (`Admin` and `User`).
- **Task Management**: Create, assign, and track tasks through life-cycle states (`NotStarted` -> `Ongoing` -> `Done`).
- **Role Isolation**: Administrators can assign tasks to team members and oversee all progress; standard users manage only their assigned workload.
- **RESTful API**: ASP.NET Core Web API with Entity Framework Core and SQL Server persistence.
- **Unit Testing**: xUnit test suite covering services, controllers, and state transitions using in-memory database execution.
- **Docker Compose**: Containerized multi-service configuration for local orchestration.

## Tech Stack

- **Backend**: ASP.NET Core 9.0, C#, Entity Framework Core
- **Frontend**: React, JavaScript, Vite, CSS
- **Database**: Microsoft SQL Server 2022
- **Testing**: xUnit, EF Core InMemory Provider
- **DevOps**: Docker, Docker Compose

## Project Structure

```text
TASKe/
├── TASKe/
│   ├── TASKe/               # ASP.NET Core Web API project
│   │   ├── Controllers/     # API controllers
│   │   ├── Data/            # EF Core DbContext & migrations
│   │   ├── Models/          # Domain entities & DTOs
│   │   └── Services/        # Business logic & state rules
│   ├── TASKe.Tests/         # xUnit test suite
│   └── TASKe.slnx           # Solution file
├── task-ui/                 # React frontend application
├── docker-compose.yml       # Multi-container orchestration
└── README.md
```

## Getting Started

### Using Docker Compose (Recommended)

To run the complete stack (API, Frontend, SQL Server):

```bash
docker compose up --build
```

- **Frontend**: `http://localhost:3000`
- **Backend API**: `http://localhost:5000`
- **SQL Server**: `localhost:1434`

To stop:

```bash
docker compose down
```

### Running Locally for Development

#### Backend (.NET 9.0)

```bash
cd TASKe/TASKe
dotnet run
```

Run unit tests:

```bash
cd TASKe
dotnet test
```

#### Frontend (React + Vite)

```bash
cd task-ui
npm install
npm run dev
```

## License

This project is licensed under the MIT License.
