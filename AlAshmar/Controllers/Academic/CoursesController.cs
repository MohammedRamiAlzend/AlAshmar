using AlAshmar.Application.UseCases.Courses.CreateCourse;
using AlAshmar.Application.UseCases.Courses.DeleteCourse;
using AlAshmar.Application.UseCases.Courses.GetAllCourses;
using AlAshmar.Application.UseCases.Courses.GetCourseById;
using AlAshmar.Application.UseCases.Courses.GetCoursesBySemester;
using AlAshmar.Application.UseCases.Courses.UpdateCourse;

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
        return result.ToActionResult();
    }

    [HttpGet("by-semester/{semesterId:guid}")]
    public async Task<IActionResult> GetBySemester(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetCoursesBySemesterQuery(semesterId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetCourseByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCourseDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new CreateCourseCommand(dto.EventName, dto.SemesterId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCourseDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCourseCommand(id, dto.EventName), cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteCourseCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}
