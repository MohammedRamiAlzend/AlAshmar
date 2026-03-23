global using AlAshmar.Application.UseCases.Courses.CreateCourse;
global using AlAshmar.Application.UseCases.Courses.DeleteCourse;
global using AlAshmar.Application.UseCases.Courses.GetAllCourses;
global using AlAshmar.Application.UseCases.Courses.GetCourseById;
global using AlAshmar.Application.UseCases.Courses.GetCoursesBySemester;
global using AlAshmar.Application.UseCases.Courses.UpdateCourse;
global using AlAshmar.Domain.DTOs.Domain;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using MediatR;

namespace AlAshmar.Controllers.Academic;

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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllCoursesQuery(), cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("by-semester/{semesterId:guid}")]
    public async Task<IActionResult> GetBySemester(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetCoursesBySemesterQuery(semesterId), cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetCourseByIdQuery(id), cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCourseDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new CreateCourseCommand(dto.EventName, dto.SemesterId), cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCourseDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCourseCommand(id, dto.EventName), cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteCourseCommand(id), cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return NoContent();
    }
}
