using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlAshmar.Domain.Entities.Forms;

namespace AlAshmar.Infrastructure.Persistence.Configurations;

public class FormConfiguration : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {
        builder.ToTable("Forms");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(f => f.Description)
            .HasMaxLength(2000);

        builder.Property(f => f.FormType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(f => f.Audience)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(f => f.AccessToken)
            .IsRequired();

        builder.HasIndex(f => f.AccessToken)
            .IsUnique();

        builder.Property(f => f.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(f => f.AllowMultipleResponses)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(f => f.CreatedByManager)
            .WithMany()
            .HasForeignKey(f => f.CreatedByManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(f => f.CreatedByTeacher)
            .WithMany()
            .HasForeignKey(f => f.CreatedByTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(f => f.Halaqa)
            .WithMany()
            .HasForeignKey(f => f.HalaqaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(f => f.Course)
            .WithMany()
            .HasForeignKey(f => f.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(f => f.Questions)
            .WithOne(q => q.Form)
            .HasForeignKey(q => q.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Responses)
            .WithOne(r => r.Form)
            .HasForeignKey(r => r.FormId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => f.CreatedByManagerId);
        builder.HasIndex(f => f.CreatedByTeacherId);
        builder.HasIndex(f => f.HalaqaId);
        builder.HasIndex(f => f.CourseId);
    }
}

public class FormQuestionConfiguration : IEntityTypeConfiguration<FormQuestion>
{
    public void Configure(EntityTypeBuilder<FormQuestion> builder)
    {
        builder.ToTable("FormQuestions");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(q => q.Description)
            .HasMaxLength(2000);

        builder.Property(q => q.QuestionType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(q => q.Order)
            .IsRequired();

        builder.Property(q => q.IsRequired)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(q => q.Form)
            .WithMany(f => f.Questions)
            .HasForeignKey(q => q.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => q.FormId);
    }
}

public class FormQuestionOptionConfiguration : IEntityTypeConfiguration<FormQuestionOption>
{
    public void Configure(EntityTypeBuilder<FormQuestionOption> builder)
    {
        builder.ToTable("FormQuestionOptions");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(o => o.Order)
            .IsRequired();

        builder.Property(o => o.IsCorrect)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.QuestionId);
    }
}

public class FormResponseConfiguration : IEntityTypeConfiguration<FormResponse>
{
    public void Configure(EntityTypeBuilder<FormResponse> builder)
    {
        builder.ToTable("FormResponses");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.SubmittedAt)
            .IsRequired();

        builder.Property(r => r.IsCompleted)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(r => r.Form)
            .WithMany(f => f.Responses)
            .HasForeignKey(r => r.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.RespondedByStudent)
            .WithMany()
            .HasForeignKey(r => r.RespondedByStudentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.RespondedByTeacher)
            .WithMany()
            .HasForeignKey(r => r.RespondedByTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(r => r.Answers)
            .WithOne(a => a.Response)
            .HasForeignKey(a => a.ResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.FormId);
        builder.HasIndex(r => r.RespondedByStudentId);
        builder.HasIndex(r => r.RespondedByTeacherId);
    }
}

public class FormAnswerConfiguration : IEntityTypeConfiguration<FormAnswer>
{
    public void Configure(EntityTypeBuilder<FormAnswer> builder)
    {
        builder.ToTable("FormAnswers");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.TextAnswer)
            .HasMaxLength(4000);

        builder.HasOne(a => a.Response)
            .WithMany(r => r.Answers)
            .HasForeignKey(a => a.ResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.SelectedOptions)
            .WithOne(so => so.FormAnswer)
            .HasForeignKey(so => so.FormAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.ResponseId);
        builder.HasIndex(a => a.QuestionId);
    }
}

public class FormAnswerSelectedOptionConfiguration : IEntityTypeConfiguration<FormAnswerSelectedOption>
{
    public void Configure(EntityTypeBuilder<FormAnswerSelectedOption> builder)
    {
        builder.ToTable("FormAnswerSelectedOptions");

        builder.HasKey(so => new { so.FormAnswerId, so.FormQuestionOptionId });

        builder.HasOne(so => so.FormAnswer)
            .WithMany(a => a.SelectedOptions)
            .HasForeignKey(so => so.FormAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(so => so.FormQuestionOption)
            .WithMany()
            .HasForeignKey(so => so.FormQuestionOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(so => so.FormAnswerId);
    }
}
