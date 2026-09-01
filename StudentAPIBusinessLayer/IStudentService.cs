namespace StudentAPIBusinessLayer;

public interface IStudentService
{
    Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentDto>> GetPassedAsync(CancellationToken cancellationToken = default);
    Task<double> GetAverageGradeAsync(CancellationToken cancellationToken = default);
    Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StudentDto> CreateAsync(StudentInput input, CancellationToken cancellationToken = default);
    Task<StudentDto?> UpdateAsync(int id, StudentInput input, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
