using StudentDataAccessLayer;

namespace StudentAPIBusinessLayer;

public sealed class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<StudentDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var students = await _repository.GetAllAsync(cancellationToken);
        return students.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<StudentDto>> GetPassedAsync(
        CancellationToken cancellationToken = default)
    {
        var students = await _repository.GetPassedAsync(cancellationToken);
        return students.Select(Map).ToList();
    }

    public Task<double> GetAverageGradeAsync(CancellationToken cancellationToken = default) =>
        _repository.GetAverageGradeAsync(cancellationToken);

    public async Task<StudentDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var student = await _repository.GetByIdAsync(id, cancellationToken);
        return student is null ? null : Map(student);
    }

    public async Task<StudentDto> CreateAsync(
        StudentInput input,
        CancellationToken cancellationToken = default)
    {
        var student = new StudentRecord(0, input.Name.Trim(), input.Age, input.Grade);
        var id = await _repository.AddAsync(student, cancellationToken);

        return Map(student with { Id = id });
    }

    public async Task<StudentDto?> UpdateAsync(
        int id,
        StudentInput input,
        CancellationToken cancellationToken = default)
    {
        var student = new StudentRecord(id, input.Name.Trim(), input.Age, input.Grade);
        var updated = await _repository.UpdateAsync(student, cancellationToken);

        return updated ? Map(student) : null;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.DeleteAsync(id, cancellationToken);

    private static StudentDto Map(StudentRecord student) =>
        new(student.Id, student.Name, student.Age, student.Grade);
}
