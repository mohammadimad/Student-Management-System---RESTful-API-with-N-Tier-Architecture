using Microsoft.AspNetCore.Mvc;
using StudentApi.Contracts;
using StudentAPIBusinessLayer;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/students")]
[Produces("application/json")]
public sealed class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StudentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var students = await _studentService.GetAllAsync(cancellationToken);
        return Ok(students);
    }

    [HttpGet("passed")]
    [ProducesResponseType(typeof(IReadOnlyList<StudentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetPassed(
        CancellationToken cancellationToken)
    {
        var students = await _studentService.GetPassedAsync(cancellationToken);
        return Ok(students);
    }

    [HttpGet("average-grade")]
    [ProducesResponseType(typeof(AverageGradeResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AverageGradeResponse>> GetAverageGrade(
        CancellationToken cancellationToken)
    {
        var average = await _studentService.GetAverageGradeAsync(cancellationToken);
        return Ok(new AverageGradeResponse(average));
    }

    [HttpGet("{id:int:min(1)}", Name = nameof(GetById))]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var student = await _studentService.GetByIdAsync(id, cancellationToken);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StudentDto>> Create(
        CreateStudentRequest request,
        CancellationToken cancellationToken)
    {
        var student = await _studentService.CreateAsync(
            new StudentInput(request.Name, request.Age, request.Grade),
            cancellationToken);

        return CreatedAtRoute(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDto>> Update(
        int id,
        UpdateStudentRequest request,
        CancellationToken cancellationToken)
    {
        var student = await _studentService.UpdateAsync(
            id,
            new StudentInput(request.Name, request.Age, request.Grade),
            cancellationToken);

        return student is null ? NotFound() : Ok(student);
    }

    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _studentService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
