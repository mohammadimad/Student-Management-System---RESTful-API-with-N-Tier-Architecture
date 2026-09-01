using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentDataAccessLayer;

public sealed class SqlStudentRepository : IStudentRepository
{
    private readonly string _connectionString;

    public SqlStudentRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("A database connection string is required.", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public Task<IReadOnlyList<StudentRecord>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        ReadStudentsAsync("SP_GetAllStudents", cancellationToken);

    public Task<IReadOnlyList<StudentRecord>> GetPassedAsync(
        CancellationToken cancellationToken = default) =>
        ReadStudentsAsync("SP_GetPassedStudents", cancellationToken);

    public async Task<double> GetAverageGradeAsync(
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = CreateStoredProcedureCommand("SP_GetAverageGrade", connection);

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);

        return result is null or DBNull ? 0 : Convert.ToDouble(result);
    }

    public async Task<StudentRecord?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = CreateStoredProcedureCommand("SP_GetStudentById", connection);
        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        return await reader.ReadAsync(cancellationToken) ? MapStudent(reader) : null;
    }

    public async Task<int> AddAsync(
        StudentRecord student,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = CreateStoredProcedureCommand("SP_AddStudent", connection);

        AddStudentParameters(command, student);
        var idParameter = command.Parameters.Add("@NewStudentId", SqlDbType.Int);
        idParameter.Direction = ParameterDirection.Output;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);

        return idParameter.Value is int id
            ? id
            : throw new DataException("The database did not return the new student ID.");
    }

    public async Task<bool> UpdateAsync(
        StudentRecord student,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = CreateStoredProcedureCommand("SP_UpdateStudent", connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = student.Id;
        AddStudentParameters(command, student);

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt32(result) == 1;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = CreateStoredProcedureCommand("SP_DeleteStudent", connection);
        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt32(result) == 1;
    }

    private async Task<IReadOnlyList<StudentRecord>> ReadStudentsAsync(
        string storedProcedure,
        CancellationToken cancellationToken)
    {
        var students = new List<StudentRecord>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = CreateStoredProcedureCommand(storedProcedure, connection);

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            students.Add(MapStudent(reader));
        }

        return students;
    }

    private static SqlCommand CreateStoredProcedureCommand(
        string storedProcedure,
        SqlConnection connection) =>
        new(storedProcedure, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

    private static void AddStudentParameters(SqlCommand command, StudentRecord student)
    {
        command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = student.Name;
        command.Parameters.Add("@Age", SqlDbType.Int).Value = student.Age;
        command.Parameters.Add("@Grade", SqlDbType.Int).Value = student.Grade;
    }

    private static StudentRecord MapStudent(SqlDataReader reader) =>
        new(
            reader.GetInt32(reader.GetOrdinal("Id")),
            reader.GetString(reader.GetOrdinal("Name")),
            reader.GetInt32(reader.GetOrdinal("Age")),
            reader.GetInt32(reader.GetOrdinal("Grade")));
}
