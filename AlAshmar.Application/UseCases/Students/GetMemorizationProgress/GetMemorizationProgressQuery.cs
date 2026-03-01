using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.GetMemorizationProgress;

/// <summary>
/// Query to get student's memorization progress (Hadith and Quran).
/// </summary>
public record GetMemorizationProgressQuery(Guid StudentId) : IQuery<Result<StudentMemorizationProgressDto>>;

/// <summary>
/// DTO for student memorization progress.
/// </summary>
public record StudentMemorizationProgressDto(
    Guid StudentId,
    List<StudentHadithDto> Hadiths,
    List<StudentQuraanPageDto> QuranPages,
    int TotalHadithsMemorized,
    int TotalQuranPagesMemorized
);
