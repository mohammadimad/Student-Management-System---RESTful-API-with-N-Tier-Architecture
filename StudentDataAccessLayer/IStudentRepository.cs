namespace StudentDataAccessLayer;

public interface IStudentRepository
{
    Task<IReadOnlyList<StudentRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentRecord>> GetPassedAsync(CancellationToken cancellationToken = default);
    Task<double> GetAverageGradeAsync(CancellationToken cancellationToken = default);
    Task<StudentRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> AddAsync(StudentRecord student, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(StudentRecord student, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
