using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Forms;

namespace AlAshmar.Application.Services.Domain;

public interface IFormService : IAdvancedCrudService<Form, FormDto, Guid>
{

    Task<Result<FormDto>> GetByAccessTokenAsync(Guid accessToken, CancellationToken cancellationToken = default);
    Task<Result<FormDto>> CreateAsync(CreateFormDto dto, CancellationToken cancellationToken = default);
    Task<Result<FormDto>> UpdateAsync(Guid id, UpdateFormDto dto, CancellationToken cancellationToken = default);
}

public class FormService : CrudServiceBase<Form, FormDto, Guid>, IFormService
{
    public FormService(IRepositoryBase<Form, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }

    public override async Task<Result<FormDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(f => f.Id == id, FormIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var entity = result.Value.FirstOrDefault();
        if (entity is null)
            return ApplicationErrors.ResourceNotFound;

        return _mapper.Map<FormDto>(entity);
    }

    public override async Task<Result<List<FormDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(transform: FormIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormDto>).ToList();
    }

    public override async Task<Result<PagedList<FormDto>>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetPagedAsync(page, pageSize, transform: FormIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var items = result.Value.Items.Select(_mapper.Map<FormDto>).ToList();
        return PagedList<FormDto>.Create(items, result.Value.TotalItems, page, pageSize);
    }

    public override async Task<Result<List<FormDto>>> FindAsync(Expression<Func<Form, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(predicate, FormIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormDto>).ToList();
    }

    public async Task<Result<FormDto>> CreateAsync(CreateFormDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Form>(dto);
        var result = await _repository.AddAsync(entity);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<FormDto>(entity);
    }

    public async Task<Result<FormDto>> UpdateAsync(Guid id, UpdateFormDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing.Value == null)
            return ApplicationErrors.ResourceNotFound;

        _mapper.Map(dto, existing.Value);

        var result = await _repository.UpdateAsync(existing.Value);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<FormDto>(existing.Value);
    }

    public async Task<Result<FormDto>> GetByAccessTokenAsync(Guid accessToken, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(f => f.AccessToken == accessToken, FormIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var form = result.Value.FirstOrDefault();
        if (form is null)
            return ApplicationErrors.FormNotFound;

        return _mapper.Map<FormDto>(form);
    }
}

public interface IFormQuestionService : IAdvancedCrudService<FormQuestion, FormQuestionDto, Guid>
{
    Task<Result<FormQuestionDto>> CreateAsync(CreateFormQuestionDto dto, CancellationToken cancellationToken = default);
    Task<Result<FormQuestionDto>> UpdateAsync(Guid id, UpdateFormQuestionDto dto, CancellationToken cancellationToken = default);
}
public class FormQuestionService : CrudServiceBase<FormQuestion, FormQuestionDto, Guid>, IFormQuestionService
{
    public FormQuestionService(IRepositoryBase<FormQuestion, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }

    public override async Task<Result<FormQuestionDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(fq => fq.Id == id, FormQuestionIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var entity = result.Value.FirstOrDefault();
        if (entity is null)
            return ApplicationErrors.ResourceNotFound;

        return _mapper.Map<FormQuestionDto>(entity);
    }

    public override async Task<Result<List<FormQuestionDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(transform: FormQuestionIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormQuestionDto>).ToList();
    }

    public override async Task<Result<PagedList<FormQuestionDto>>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetPagedAsync(page, pageSize, transform: FormQuestionIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var items = result.Value.Items.Select(_mapper.Map<FormQuestionDto>).ToList();
        return PagedList<FormQuestionDto>.Create(items, result.Value.TotalItems, page, pageSize);
    }

    public override async Task<Result<List<FormQuestionDto>>> FindAsync(Expression<Func<FormQuestion, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(predicate, FormQuestionIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormQuestionDto>).ToList();
    }

    public async Task<Result<FormQuestionDto>> CreateAsync(CreateFormQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<FormQuestion>(dto);
        var result = await _repository.AddAsync(entity);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<FormQuestionDto>(entity);
    }

    public async Task<Result<FormQuestionDto>> UpdateAsync(Guid id, UpdateFormQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing.Value == null)
            return ApplicationErrors.ResourceNotFound;

        _mapper.Map(dto, existing.Value);

        var result = await _repository.UpdateAsync(existing.Value);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<FormQuestionDto>(existing.Value);
    }
}

public interface IFormQuestionOptionService : IAdvancedCrudService<FormQuestionOption, FormQuestionOptionDto, Guid>
{
    Task<Result<FormQuestionOptionDto>> CreateAsync(CreateFormQuestionOptionDto dto, CancellationToken cancellationToken = default);
    Task<Result<FormQuestionOptionDto>> UpdateAsync(Guid id, UpdateFormQuestionOptionDto dto, CancellationToken cancellationToken = default);
}
public class FormQuestionOptionService : CrudServiceBase<FormQuestionOption, FormQuestionOptionDto, Guid>, IFormQuestionOptionService
{
    public FormQuestionOptionService(IRepositoryBase<FormQuestionOption, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }

    public async Task<Result<FormQuestionOptionDto>> CreateAsync(CreateFormQuestionOptionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<FormQuestionOption>(dto);
        var result = await _repository.AddAsync(entity);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<FormQuestionOptionDto>(entity);
    }

    public async Task<Result<FormQuestionOptionDto>> UpdateAsync(Guid id, UpdateFormQuestionOptionDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing.Value == null)
            return ApplicationErrors.ResourceNotFound;

        _mapper.Map(dto, existing.Value);

        var result = await _repository.UpdateAsync(existing.Value);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<FormQuestionOptionDto>(existing.Value);
    }
}

public interface IFormResponseService : IAdvancedCrudService<FormResponse, FormResponseDto, Guid>
{

    Task<Result<FormResponseDto>> SubmitAsync(SubmitFormResponseDto dto, CancellationToken cancellationToken = default);
}

public class FormResponseService : CrudServiceBase<FormResponse, FormResponseDto, Guid>, IFormResponseService
{
    private readonly IRepositoryBase<Form, Guid> _formRepository;
    private readonly IRepositoryBase<FormQuestion, Guid> _questionRepository;
    private readonly IRepositoryBase<FormAnswer, Guid> _answerRepository;

    public FormResponseService(
        IRepositoryBase<FormResponse, Guid> repository,
        IRepositoryBase<Form, Guid> formRepository,
        IRepositoryBase<FormQuestion, Guid> questionRepository,
        IRepositoryBase<FormAnswer, Guid> answerRepository,
        IMapper mapper)
        : base(repository, mapper)
    {
        _formRepository = formRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
    }

    public override async Task<Result<FormResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(fr => fr.Id == id, FormResponseIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var entity = result.Value.FirstOrDefault();
        if (entity is null)
            return ApplicationErrors.ResourceNotFound;

        return _mapper.Map<FormResponseDto>(entity);
    }

    public override async Task<Result<List<FormResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(transform: FormResponseIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormResponseDto>).ToList();
    }

    public override async Task<Result<PagedList<FormResponseDto>>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetPagedAsync(page, pageSize, transform: FormResponseIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var items = result.Value.Items.Select(_mapper.Map<FormResponseDto>).ToList();
        return PagedList<FormResponseDto>.Create(items, result.Value.TotalItems, page, pageSize);
    }

    public override async Task<Result<List<FormResponseDto>>> FindAsync(Expression<Func<FormResponse, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(predicate, FormResponseIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormResponseDto>).ToList();
    }

    public async Task<Result<FormResponseDto>> SubmitAsync(SubmitFormResponseDto dto, CancellationToken cancellationToken = default)
    {

        var formResult = await _formRepository.GetByIdAsync(dto.FormId);
        if (formResult.IsError || formResult.Value is null)
            return ApplicationErrors.FormNotFound;

        var form = formResult.Value;

        if (!form.IsActive)
            return ApplicationErrors.FormNotActive;

        var now = DateTime.UtcNow;
        if (form.StartsAt.HasValue && now < form.StartsAt.Value)
            return ApplicationErrors.FormNotStarted;

        if (form.EndsAt.HasValue && now > form.EndsAt.Value)
            return ApplicationErrors.FormAlreadyEnded;

        var response = new FormResponse
        {
            Id = Guid.NewGuid(),
            FormId = dto.FormId,
            RespondedByStudentId = dto.RespondedByStudentId,
            RespondedByTeacherId = dto.RespondedByTeacherId,
            SubmittedAt = now,
            TimeSpentSeconds = dto.TimeSpentSeconds,
            IsCompleted = true
        };

        int totalScore = 0;

        foreach (var answerDto in dto.Answers)
        {
            var questionResult = await _questionRepository.GetByIdAsync(answerDto.QuestionId);
            var question = questionResult.Value;

            var answer = new FormAnswer
            {
                Id = Guid.NewGuid(),
                ResponseId = response.Id,
                QuestionId = answerDto.QuestionId,
                TextAnswer = answerDto.TextAnswer
            };
            if (form.FormType == FormType.Quiz && question is not null && question.Points.HasValue)
            {
                bool isCorrect = false;

                if (question.QuestionType is QuestionType.ShortText or QuestionType.LongText)
                {
                }
                else if (answerDto.SelectedOptionIds.Count > 0 && question.Options.Count > 0)
                {
                    var correctOptionIds = question.Options
                        .Where(o => o.IsCorrect)
                        .Select(o => o.Id)
                        .OrderBy(id => id)
                        .ToList();

                    var selectedIds = answerDto.SelectedOptionIds.OrderBy(id => id).ToList();
                    isCorrect = correctOptionIds.SequenceEqual(selectedIds);
                }

                answer.IsCorrect = isCorrect;
                answer.PointsAwarded = isCorrect ? question.Points.Value : 0;
                totalScore += answer.PointsAwarded.Value;
            }
            foreach (var optionId in answerDto.SelectedOptionIds)
            {
                answer.SelectedOptions.Add(new FormAnswerSelectedOption
                {
                    FormAnswerId = answer.Id,
                    FormQuestionOptionId = optionId
                });
            }

            response.Answers.Add(answer);
        }

        if (form.FormType == FormType.Quiz)
            response.TotalScore = totalScore;

        var saveResult = await _repository.AddAsync(response);
        if (saveResult.IsError)
            return saveResult.Errors;

        return _mapper.Map<FormResponseDto>(response);
    }
}

public interface IFormAnswerService : IAdvancedCrudService<FormAnswer, FormAnswerDto, Guid> { }
public class FormAnswerService : CrudServiceBase<FormAnswer, FormAnswerDto, Guid>, IFormAnswerService
{
    public FormAnswerService(IRepositoryBase<FormAnswer, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }

    public override async Task<Result<FormAnswerDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(fa => fa.Id == id, FormAnswerIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var entity = result.Value.FirstOrDefault();
        if (entity is null)
            return ApplicationErrors.ResourceNotFound;

        return _mapper.Map<FormAnswerDto>(entity);
    }

    public override async Task<Result<List<FormAnswerDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(transform: FormAnswerIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormAnswerDto>).ToList();
    }

    public override async Task<Result<PagedList<FormAnswerDto>>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetPagedAsync(page, pageSize, transform: FormAnswerIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        var items = result.Value.Items.Select(_mapper.Map<FormAnswerDto>).ToList();
        return PagedList<FormAnswerDto>.Create(items, result.Value.TotalItems, page, pageSize);
    }

    public override async Task<Result<List<FormAnswerDto>>> FindAsync(Expression<Func<FormAnswer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(predicate, FormAnswerIncludes.Instance.Apply());
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<FormAnswerDto>).ToList();
    }
}
