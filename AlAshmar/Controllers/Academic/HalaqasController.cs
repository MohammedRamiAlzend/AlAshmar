using AlAshmar.Application.UseCases.Halaqas.CreateHalaqa;
using AlAshmar.Application.UseCases.Halaqas.DeleteHalaqa;
using AlAshmar.Application.UseCases.Halaqas.GetAllHalaqas;
using AlAshmar.Application.UseCases.Halaqas.GetHalaqaById;
using AlAshmar.Application.UseCases.Halaqas.GetHalaqasByCourse;
using AlAshmar.Application.UseCases.Halaqas.UpdateHalaqa;

namespace AlAshmar.Controllers.Academic;





[ApiController]
[Route("api/halaqas")]
[Authorize]
public class HalaqasController : ControllerBase
{
    private readonly ISender _sender;

    public HalaqasController(ISender sender)
    {
        _sender = sender;
    }




    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllHalaqasQuery(), cancellationToken);
        return result.ToActionResult();
    }




    [HttpGet("by-course/{courseId:guid}")]
    public async Task<IActionResult> GetByCourse(
        [FromRoute] Guid courseId,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetHalaqasByCourseQuery(courseId), cancellationToken);
        return result.ToActionResult();
    }




    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetHalaqaByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }




    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateHalaqaDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new CreateHalaqaCommand(dto.ClassName, dto.CourseId), cancellationToken);
        return result.ToActionResult();
    }




    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateHalaqaDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateHalaqaCommand(id, dto.ClassName), cancellationToken);
        return result.ToActionResult();
    }




    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteHalaqaCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}
