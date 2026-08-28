# TASKe

TASKe is a task management web application built as a full-stack project.

The project has a React frontend, an ASP.NET backend API, and a SQL Server database. Docker Compose is used to run the different parts of the application together.

## Features

* User registration and login
* Create, update, and delete tasks
* View and manage tasks through a web interface
* REST API for communication between the frontend and backend
* SQL Server database for storing application data
* Docker Compose setup for running the application

## Tech stack

### Frontend

* React
* JavaScript
* HTML/CSS

### Backend

* ASP.NET Core
* C#
* REST API
* Entity Framework Core

### Database

* Microsoft SQL Server

### Development

* Docker
* Docker Compose

## Project structure

```text
TASKe/
├── frontend/        # React application
├── backend/         # ASP.NET Core API
├── database/        # Database and migration files
├── docker-compose.yml
└── README.md
```

The exact folder structure may vary depending on the current version of the project.

## How it works

The React application handles the user interface and sends requests to the ASP.NET API.

The API handles authentication, task operations, and communication with the database. Entity Framework Core is used to work with the SQL Server database.

The basic flow is:

```text
React frontend
      |
      | HTTP requests
      v
ASP.NET Core API
      |
      | Entity Framework Core
      v
SQL Server
```

## Running the project

### Requirements

You need:

* Docker
* Docker Compose

Clone the repository:

```bash
git clone https://github.com/AyoubAZ-0/TASKe.git
cd TASKe
```

Start the application:

```bash
docker compose up --build
```

Once the containers are running, open the frontend using the port configured in `docker-compose.yml`.

To stop the application:

```bash
docker compose down
```

## Development

If you want to work on the frontend or backend separately, you can run them using their respective development tools instead of Docker.

For the backend, restore the .NET dependencies and run the ASP.NET application.

For the frontend, install the npm dependencies and start the React development server.

```bash
npm install
npm start
```

The API configuration used by the frontend should point to the address where the ASP.NET API is running.

## Database

TASKe uses Microsoft SQL Server to store application data.

The backend uses Entity Framework Core to access the database and manage the database schema.

When running the project with Docker Compose, the database runs in its own container.

## What I learned

I built TASKe to get more experience with full-stack development.

The project helped me understand how a React frontend communicates with an ASP.NET API, how to structure a backend using controllers, services, and models, and how to connect an application to SQL Server with Entity Framework Core.

I also learned more about Docker and Docker Compose by setting up the frontend, backend, and database so they could run together.

## License

This project is for learning and development purposes.
