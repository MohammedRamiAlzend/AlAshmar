using AlAshmar.Application.Services.Domain;
using AlAshmar.Domain.Entities.Forms;

namespace AlAshmar.Controllers.Forms;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormsController(IFormService formService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FormDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await formService.GetAllAsync(cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedList<FormDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await formService.GetPagedAsync(page, pageSize, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FormDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await formService.GetByIdAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpGet("access/{accessToken:guid}")]
    public async Task<ActionResult<FormDto>> GetByAccessToken(Guid accessToken, CancellationToken cancellationToken)
    {
        var result = await formService.GetByAccessTokenAsync(accessToken, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);

        var form = result.Value;

        if (form.FormType == FormType.Quiz && !(User.Identity?.IsAuthenticated ?? false))
            return Unauthorized();

        if (!form.IsActive)
            return BadRequest("Form is not active.");

        var now = DateTime.UtcNow;
        if (form.StartsAt.HasValue && now < form.StartsAt.Value)
            return BadRequest("Form has not started yet.");
        if (form.EndsAt.HasValue && now > form.EndsAt.Value)
            return BadRequest("Form has already ended.");

        return Ok(form);
    }

    [HttpPost]
    public async Task<ActionResult<FormDto>> Create([FromBody] CreateFormDto dto, CancellationToken cancellationToken)
    {
        var result = await formService.CreateAsync(dto, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FormDto>> Update(Guid id, [FromBody] UpdateFormDto dto, CancellationToken cancellationToken)
    {
        var result = await formService.UpdateAsync(id, dto, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await formService.DeleteAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormQuestionsController(IFormQuestionService service) : ControllerBase
{
    private readonly IFormQuestionService _service = service;

    [HttpGet]
    public async Task<ActionResult<List<FormQuestionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FormQuestionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpGet("by-form/{formId:guid}")]
    public async Task<ActionResult<List<FormQuestionDto>>> GetByForm(Guid formId, CancellationToken cancellationToken)
    {
        var result = await _service.FindAsync(q => q.FormId == formId, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<FormQuestionDto>> Create([FromBody] CreateFormQuestionDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FormQuestionDto>> Update(Guid id, [FromBody] UpdateFormQuestionDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormQuestionOptionsController : ControllerBase
{
    private readonly IFormQuestionOptionService _service;

    public FormQuestionOptionsController(IFormQuestionOptionService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FormQuestionOptionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpGet("by-question/{questionId:guid}")]
    public async Task<ActionResult<List<FormQuestionOptionDto>>> GetByQuestion(Guid questionId, CancellationToken cancellationToken)
    {
        var result = await _service.FindAsync(o => o.QuestionId == questionId, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<FormQuestionOptionDto>> Create([FromBody] CreateFormQuestionOptionDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FormQuestionOptionDto>> Update(Guid id, [FromBody] UpdateFormQuestionOptionDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormResponsesController : ControllerBase
{
    private readonly IFormResponseService _service;
    private readonly IFormService _formService;
    private readonly IMapper _mapper;

    public FormResponsesController(IFormResponseService service, IFormService formService, IMapper mapper)
    {
        _service = service;
        _formService = formService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FormResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpGet("by-form/{formId:guid}")]
    public async Task<ActionResult<List<FormResponseDto>>> GetByForm(Guid formId, CancellationToken cancellationToken)
    {
        var result = await _service.FindAsync(r => r.FormId == formId, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("submit")]
    public async Task<ActionResult<FormResponseDto>> Submit([FromBody] SubmitFormResponseDto dto, CancellationToken cancellationToken)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            var formResult = await _formService.GetByIdAsync(dto.FormId, cancellationToken);
            if (!formResult.IsError && formResult.Value?.FormType == FormType.Quiz)
                return Unauthorized();
        }

        var result = await _service.SubmitAsync(dto, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return NoContent();
    }
}
