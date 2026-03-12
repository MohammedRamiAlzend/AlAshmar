using AlAshmar.Application.UseCases.Courses.CreateCourse;
using AlAshmar.Application.UseCases.Courses.DeleteCourse;
using AlAshmar.Application.UseCases.Courses.GetAllCourses;
using AlAshmar.Application.UseCases.Courses.GetCourseById;
using AlAshmar.Application.UseCases.Courses.GetCoursesBySemester;
using AlAshmar.Application.UseCases.Courses.UpdateCourse;
using AlAshmar.Application.DTOs.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Academic;

/// <summary>
/// Controller for Course (course/program) CRUD operations.
/// Each Semester has many Courses; each Course has many Halaqas.
/// </summary>
[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ISender _sender;

    public CoursesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get all courses.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllCoursesQuery(), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get all courses in a specific semester.
    /// </summary>
    [HttpGet("by-semester/{semesterId:guid}")]
    public async Task<IActionResult> GetBySemester(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetCoursesBySemesterQuery(semesterId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a course by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetCourseByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new course.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCourseDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new CreateCourseCommand(dto.EventName, dto.SemesterId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing course.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCourseDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCourseCommand(id, dto.EventName), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a course by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteCourseCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}
