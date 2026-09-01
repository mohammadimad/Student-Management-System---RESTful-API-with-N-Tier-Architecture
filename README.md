# Student Management System API

An educational .NET 8 project that applies an N-tier architecture to student management. It separates the API host, business rules, and SQL Server data access into independent projects and uses stored procedures for database operations.

## Project Status

The solution builds successfully, and the Business Logic Layer (BLL) and Data Access Layer (DAL) implement the student operations described below. However, the current repository does not include a student controller, route definitions, or the SQL schema and stored-procedure scripts. Those pieces must be added before the application exposes a complete REST API.

The database connection is also hard-coded in the current DAL and should be moved to configuration or .NET user secrets before the project is shared or deployed.

## Implemented Capabilities

- Retrieve all students.
- Find a student by ID.
- Add a new student.
- Update an existing student.
- Delete a student.
- Return students with passing grades.
- Calculate the average student grade.
- Transfer data through a dedicated `StudentDTO`.
- Track whether a business object is in add or update mode.
- Execute database operations through SQL Server stored procedures.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- C#
- SQL Server
- ADO.NET with `Microsoft.Data.SqlClient`
- Swagger / OpenAPI

## N-Tier Architecture

```text
StudentApi.sln
|-- MyFirstRestAPI_Porject/     # Presentation/API host
|   |-- Program.cs
|   `-- StudentApi.csproj
|-- StudentAPIBusinessLayer/    # Business rules and object state
|   `-- Student.cs
`-- StudentDataAccessLayer/     # DTOs and stored-procedure access
    `-- StudentData.cs
```

### Presentation Layer

Configures ASP.NET Core controllers and Swagger. A student controller still needs to be committed to expose the business operations over HTTP.

### Business Logic Layer

The `Student` class coordinates add and update operations, exposes query helpers, and keeps the persistence details out of the API layer.

### Data Access Layer

The `StudentData` class uses ADO.NET and parameterized stored-procedure calls to communicate with SQL Server.

## Required Database Operations

The data layer expects the following stored procedures:

| Stored procedure | Purpose |
|---|---|
| `SP_GetAllStudents` | Return all students |
| `SP_GetPassedStudents` | Return students with passing grades |
| `SP_GetAverageGrade` | Calculate the average grade |
| `SP_GetStudentById` | Return one student by ID |
| `SP_AddStudent` | Insert a student and return the generated ID |
| `SP_UpdateStudent` | Update a student |
| `SP_DeleteStudent` | Delete a student |

The expected student fields are:

```text
Id      integer, primary key
Name    string
Age     integer
Grade   integer
```

## Local Setup

### Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server Express
- The required table and stored procedures listed above

### 1. Clone the repository

```bash
git clone https://github.com/mohammadimad/Student-Management-System---RESTful-API-with-N-Tier-Architecture.git
cd Student-Management-System---RESTful-API-with-N-Tier-Architecture
```

### 2. Restore and build

```bash
dotnet restore StudentApi.sln
dotnet build StudentApi.sln
```

### 3. Configure the database securely

Move the SQL Server connection string out of `StudentData.cs`. Store it in `appsettings.Development.json`, environment variables, or .NET user secrets, and inject it through configuration.

Do not commit real database usernames or passwords.

### 4. Complete the API layer

Add a controller that calls the Business Logic Layer, then run:

```bash
dotnet run --project MyFirstRestAPI_Porject/StudentApi.csproj
```

Swagger is enabled in the development environment and will be available under `/swagger` after HTTP endpoints are added.

## Build Verification

The solution was restored and built successfully with the .NET SDK. The current compiler output contains two nullable-reference warnings where lookup operations may return no student.

## Recommended Next Steps

- Add and document the student controller and REST routes.
- Include SQL scripts for the table and stored procedures.
- Move the connection string into secure configuration.
- Add request validation and consistent error responses.
- Add unit tests for the business layer.
- Add integration tests for the API and data flows.
- Remove tracked `bin` and `obj` directories and add a standard .NET `.gitignore`.

## Learning Goals

This project demonstrates:

- Separation of concerns across three layers.
- Business-object state management.
- ADO.NET data access with stored procedures.
- DTO-based data transfer.
- Dependency flow between API, business, and data projects.

## Author

[Mohammad Imad Abdelfattah](https://github.com/mohammadimad)
