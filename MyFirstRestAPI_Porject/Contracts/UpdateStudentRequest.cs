using System.ComponentModel.DataAnnotations;

namespace StudentApi.Contracts;

public sealed class UpdateStudentRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [Range(1, 150)]
    public int Age { get; init; }

    [Range(0, 100)]
    public int Grade { get; init; }
}
