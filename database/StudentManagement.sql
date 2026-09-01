IF DB_ID(N'StudentManagementDb') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE StudentManagementDb;');
END;
GO

USE StudentManagementDb;
GO

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        Id INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_Students PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Age INT NOT NULL
            CONSTRAINT CK_Students_Age CHECK (Age BETWEEN 1 AND 150),
        Grade INT NOT NULL
            CONSTRAINT CK_Students_Grade CHECK (Grade BETWEEN 0 AND 100)
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetAllStudents
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Age, Grade
    FROM dbo.Students
    ORDER BY Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetPassedStudents
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Age, Grade
    FROM dbo.Students
    WHERE Grade >= 50
    ORDER BY Grade DESC, Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetAverageGrade
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COALESCE(AVG(CAST(Grade AS FLOAT)), 0)
    FROM dbo.Students;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_GetStudentById
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Age, Grade
    FROM dbo.Students
    WHERE Id = @StudentId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_AddStudent
    @Name NVARCHAR(100),
    @Age INT,
    @Grade INT,
    @NewStudentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Students (Name, Age, Grade)
    VALUES (@Name, @Age, @Grade);

    SET @NewStudentId = CONVERT(INT, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_UpdateStudent
    @StudentId INT,
    @Name NVARCHAR(100),
    @Age INT,
    @Grade INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Students
    SET Name = @Name,
        Age = @Age,
        Grade = @Grade
    WHERE Id = @StudentId;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_DeleteStudent
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Students
    WHERE Id = @StudentId;

    SELECT @@ROWCOUNT;
END;
GO
