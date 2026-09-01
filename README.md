# Student Management System API

A complete educational RESTful API built with ASP.NET Core and a three-tier architecture. It manages student records through a clean HTTP layer, business services, and an asynchronous ADO.NET repository backed by SQL Server stored procedures.

## Project Status

The API is implemented and builds successfully on .NET 8 with **0 warnings and 0 errors**. It includes CRUD endpoints, validation, Swagger documentation, secure configuration, SQL database setup, consistent status codes, and sample HTTP requests.

## Features

- Create, retrieve, update, and delete students.
- Retrieve students with passing grades (`Grade >= 50`).
- Calculate the average grade.
- Validate names, ages, and grades automatically.
- Return standard HTTP status codes, including `201`, `204`, `400`, and `404`.
- Access SQL Server asynchronously through parameterized stored procedures.
- Separate the API, business, and data-access responsibilities.
- Configure dependencies with built-in Dependency Injection.
- Return standardized problem details for unhandled errors.
- Explore and test endpoints through Swagger UI.
- Check application availability through `/health`.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- C#
- SQL Server
- ADO.NET with `Microsoft.Data.SqlClient`
- Swagger / OpenAPI

## Architecture

```text
StudentApi.sln
|-- MyFirstRestAPI_Porject/          # Presentation/API layer
|   |-- Contracts/                   # Validated request and response contracts
|   |-- Controllers/                 # REST endpoints
|   |-- Properties/launchSettings.json
|   `-- Program.cs                   # Application startup and DI configuration
|-- StudentAPIBusinessLayer/         # Business layer
|   |-- IStudentService.cs
|   |-- StudentDto.cs
|   `-- StudentService.cs
|-- StudentDataAccessLayer/          # Data-access layer
|   |-- IStudentRepository.cs
|   |-- SqlStudentRepository.cs
|   `-- StudentRecord.cs
`-- database/
    `-- StudentManagement.sql        # Database, table, and stored procedures
```

The dependency flow is:

```text
HTTP Request -> StudentsController -> IStudentService -> IStudentRepository -> SQL Server
```

Interfaces keep the layers loosely coupled, while ASP.NET Core resolves their implementations through Dependency Injection.

## API Endpoints

| Method | Endpoint | Description | Success |
|---|---|---|---|
| `GET` | `/health` | Check API availability | `200 OK` |
| `GET` | `/api/students` | Return all students | `200 OK` |
| `GET` | `/api/students/{id}` | Return one student | `200 OK` |
| `GET` | `/api/students/passed` | Return students with grades of 50 or higher | `200 OK` |
| `GET` | `/api/students/average-grade` | Return the average student grade | `200 OK` |
| `POST` | `/api/students` | Create a student | `201 Created` |
| `PUT` | `/api/students/{id}` | Update a student | `200 OK` |
| `DELETE` | `/api/students/{id}` | Delete a student | `204 No Content` |

Missing records return `404 Not Found`. Invalid request bodies return `400 Bad Request` with validation details.

### Request body

```json
{
  "name": "Mohammad Abdelfattah",
  "age": 23,
  "grade": 95
}
```

Validation rules:

- `name`: required, 2-100 characters.
- `age`: between 1 and 150.
- `grade`: between 0 and 100.

## Local Setup

### Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server Express
- SQL Server Management Studio or the `sqlcmd` command-line tool

### 1. Clone the repository

```bash
git clone https://github.com/mohammadimad/Student-Management-System---RESTful-API-with-N-Tier-Architecture.git
cd Student-Management-System---RESTful-API-with-N-Tier-Architecture
```

### 2. Create the database

Open `database/StudentManagement.sql` in SQL Server Management Studio and execute it, or run:

```powershell
sqlcmd -S . -E -i database/StudentManagement.sql
```

The script creates:

- `StudentManagementDb`
- The `Students` table with data constraints
- All stored procedures required by the repository

### 3. Configure the connection

Development uses Windows authentication by default:

```text
Server=.;Database=StudentManagementDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Adjust `appsettings.Development.json` for your local SQL Server instance. For credentials or deployment environments, use .NET user secrets or environment variables instead of committing them:

```powershell
dotnet user-secrets set "ConnectionStrings:StudentDatabase" "YOUR_CONNECTION_STRING" --project MyFirstRestAPI_Porject/StudentApi.csproj
```

Environment-variable alternative:

```text
ConnectionStrings__StudentDatabase=YOUR_CONNECTION_STRING
```

### 4. Restore and build

```bash
dotnet restore StudentApi.sln
dotnet build StudentApi.sln --no-restore
```

### 5. Run the API

```bash
dotnet run --project MyFirstRestAPI_Porject/StudentApi.csproj
```

Open [http://localhost:5215/swagger](http://localhost:5215/swagger) to explore the API. Ready-to-run examples are also available in `MyFirstRestAPI_Porject/StudentAPi.http`.

## Database Operations

The data layer uses these stored procedures:

| Stored procedure | Purpose |
|---|---|
| `SP_GetAllStudents` | Return all students |
| `SP_GetPassedStudents` | Return students with grades of 50 or higher |
| `SP_GetAverageGrade` | Calculate the average grade |
| `SP_GetStudentById` | Return one student by ID |
| `SP_AddStudent` | Insert a student and return its generated ID |
| `SP_UpdateStudent` | Update a student and return the affected-row count |
| `SP_DeleteStudent` | Delete a student and return the affected-row count |

## Build Verification

Verified with:

```bash
dotnet restore StudentApi.sln
dotnet build StudentApi.sln --configuration Release --no-restore
dotnet list StudentApi.sln package --vulnerable --include-transitive
```

Current result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
No vulnerable packages found from the configured NuGet sources.
```

The application startup, health endpoint, automatic request validation, Swagger document, and all registered routes were also smoke-tested locally. Database-backed operations require a running SQL Server instance initialized with the included script.

## Security Notes

- No database usernames or passwords are stored in the source code.
- Keep production connection strings in environment variables or a secret manager.
- The provided development connection uses Windows authentication and is intended only for local use.
- Add authentication and role-based authorization before exposing administrative operations publicly.
- Use HTTPS and production-grade logging and monitoring when deploying.

## Author

[Mohammad Imad Abdelfattah](https://github.com/mohammadimad)
