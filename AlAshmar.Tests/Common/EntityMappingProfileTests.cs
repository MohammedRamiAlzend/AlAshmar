using AlAshmar.Application.Common;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Forms;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace AlAshmar.Tests.Common;

public class EntityMappingProfileTests
{
    private readonly IMapper _mapper;

    public EntityMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EntityMappingProfile>(), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void UpdateFormDto_To_Form_IsMapped()
    {
        var dto = new UpdateFormDto(
            Title: "Updated title",
            Description: "Updated description",
            FormType: FormType.Quiz,
            Audience: AudienceType.Both,
            TimerMinutes: 30,
            IsActive: false,
            AllowMultipleResponses: true,
            StartsAt: DateTime.UtcNow.AddDays(-1),
            EndsAt: DateTime.UtcNow.AddDays(3),
            HalaqaId: Guid.NewGuid(),
            CourseId: Guid.NewGuid(),
            PrimaryColor: "#101010",
            BackgroundColor: "#f0f0f0",
            FontFamily: "Noto Kufi Arabic"
        );

        var entity = _mapper.Map<Form>(dto);

        Assert.Equal(dto.Title, entity.Title);
        Assert.Equal(dto.FormType, entity.FormType);
        Assert.Equal(dto.AllowMultipleResponses, entity.AllowMultipleResponses);
    }

    [Fact]
    public void CreateFormDto_To_Form_IsMapped()
    {
        var dto = new CreateFormDto(
            Title: "Weekly Survey",
            Description: "Collect feedback",
            FormType: FormType.Normal,
            Audience: AudienceType.Students,
            TimerMinutes: 15,
            IsActive: true,
            AllowMultipleResponses: false,
            StartsAt: null,
            EndsAt: null,
            CreatedByManagerId: Guid.NewGuid(),
            CreatedByTeacherId: null,
            HalaqaId: null,
            CourseId: null,
            PrimaryColor: "#123456",
            BackgroundColor: "#ffffff",
            FontFamily: "Cairo"
        );

        var entity = _mapper.Map<Form>(dto);

        Assert.Equal(dto.Title, entity.Title);
        Assert.Equal(dto.Description, entity.Description);
        Assert.Equal(dto.FormType, entity.FormType);
    }

    [Fact]
    public void Form_To_FormDto_IsMapped()
    {
        var form = new Form
        {
            Id = Guid.NewGuid(),
            Title = "Quiz 1",
            Description = "Basics",
            FormType = FormType.Quiz,
            Audience = AudienceType.Students,
            AccessToken = Guid.NewGuid(),
            TimerMinutes = 20,
            IsActive = true,
            AllowMultipleResponses = false,
            StartsAt = DateTime.UtcNow,
            EndsAt = DateTime.UtcNow.AddDays(1),
            PrimaryColor = "#000",
            BackgroundColor = "#fff",
            FontFamily = "Cairo",
            Questions =
            [
                new FormQuestion
                {
                    Id = Guid.NewGuid(),
                    FormId = Guid.NewGuid(),
                    Text = "Q1",
                    QuestionType = QuestionType.MultipleChoice,
                    Options =
                    [
                        new FormQuestionOption
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = Guid.NewGuid(),
                            Text = "A",
                            Order = 1,
                            IsCorrect = true
                        }
                    ]
                }
            ]
        };

        var dto = _mapper.Map<FormDto>(form);

        Assert.Equal(form.Id, dto.Id);
        Assert.Equal(form.Title, dto.Title);
        Assert.Single(dto.Questions);
    }

    [Fact]
    public void CreateFormQuestionDto_To_FormQuestion_IsMapped()
    {
        var dto = new CreateFormQuestionDto(
            FormId: Guid.NewGuid(),
            Text: "How was class today?",
            Description: "Pick one answer",
            QuestionType: QuestionType.MultipleChoice,
            Order: 1,
            IsRequired: true,
            Points: 5,
            ColumnSpan: 12,
            LabelColor: "#111111",
            FontSize: 14,
            FontFamily: "Cairo",
            Options:
            [
                new CreateFormQuestionOptionDto(
                    QuestionId: Guid.NewGuid(),
                    Text: "Great",
                    Order: 1,
                    IsCorrect: true
                )
            ]
        );

        var entity = _mapper.Map<FormQuestion>(dto);

        Assert.Equal(dto.FormId, entity.FormId);
        Assert.Equal(dto.Text, entity.Text);
        Assert.Equal(dto.QuestionType, entity.QuestionType);
    }

    [Fact]
    public void CreateFormQuestionOptionDto_To_FormQuestionOption_IsMapped()
    {
        var dto = new CreateFormQuestionOptionDto(
            QuestionId: Guid.NewGuid(),
            Text: "Option A",
            Order: 1,
            IsCorrect: true
        );

        var entity = _mapper.Map<FormQuestionOption>(dto);

        Assert.Equal(dto.QuestionId, entity.QuestionId);
        Assert.Equal(dto.Text, entity.Text);
        Assert.Equal(dto.IsCorrect, entity.IsCorrect);
    }
}
