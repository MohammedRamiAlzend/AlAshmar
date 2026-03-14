using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Domain.Entities.Forms;

namespace AlAshmar.Application.Services.Domain;

// ==================== FORM DOMAIN SERVICES ====================

public interface IFormService : IAdvancedCrudService<Form, FormDto, Guid>
{
    /// <summary>Returns the form identified by its magic-link access token.</summary>
    Task<Result<FormDto>> GetByAccessTokenAsync(Guid accessToken, CancellationToken cancellationToken = default);
}

public class FormService : CrudServiceBase<Form, FormDto, Guid>, IFormService
{
    public FormService(IRepositoryBase<Form, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }

    public async Task<Result<FormDto>> GetByAccessTokenAsync(Guid accessToken, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(f => f.AccessToken == accessToken);
        if (result.IsError)
            return result.Errors;

        var form = result.Value.FirstOrDefault();
        if (form is null)
            return ApplicationErrors.FormNotFound;

        return _mapper.Map<FormDto>(form);
    }
}

public interface IFormQuestionService : IAdvancedCrudService<FormQuestion, FormQuestionDto, Guid> { }
public class FormQuestionService : CrudServiceBase<FormQuestion, FormQuestionDto, Guid>, IFormQuestionService
{
    public FormQuestionService(IRepositoryBase<FormQuestion, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IFormQuestionOptionService : IAdvancedCrudService<FormQuestionOption, FormQuestionOptionDto, Guid> { }
public class FormQuestionOptionService : CrudServiceBase<FormQuestionOption, FormQuestionOptionDto, Guid>, IFormQuestionOptionService
{
    public FormQuestionOptionService(IRepositoryBase<FormQuestionOption, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IFormResponseService : IAdvancedCrudService<FormResponse, FormResponseDto, Guid>
{
    /// <summary>
    /// Persists a full form submission (all answers) in a single operation and
    /// auto-grades the response when the form is a quiz.
    /// </summary>
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

    public async Task<Result<FormResponseDto>> SubmitAsync(SubmitFormResponseDto dto, CancellationToken cancellationToken = default)
    {
        // Validate form exists and is active
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

        // Build the response entity
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

            // Auto-grade for quiz forms
            if (form.FormType == FormType.Quiz && question is not null && question.Points.HasValue)
            {
                bool isCorrect = false;

                if (question.QuestionType is QuestionType.ShortText or QuestionType.LongText)
                {
                    // Text answers are not auto-graded
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

            // Link selected options
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
}
