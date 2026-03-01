using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Students.GetMemorizationProgress;

public class GetMemorizationProgressHandler(
    IRepositoryBase<StudentHadith, Guid> hadithRepository,
    IRepositoryBase<StudentQuraanPage, Guid> quranRepository)
    : IQueryHandler<GetMemorizationProgressQuery, Result<StudentMemorizationProgressDto>>
{
    public async Task<Result<StudentMemorizationProgressDto>> Handle(GetMemorizationProgressQuery query, CancellationToken cancellationToken = default)
    {
        var hadiths = await hadithRepository.GetAllAsync(
            h => h.StudentId == query.StudentId,
            q => q.Include(h => h.Hadith).ThenInclude(h => h.Book));

        var quranPages = await quranRepository.GetAllAsync(
            q => q.StudentId == query.StudentId);

        if (hadiths.IsError) return hadiths.Errors;
        if (quranPages.IsError) return quranPages.Errors;

        var hadithDtos = hadiths.Value
            .Select(h => new StudentHadithDto(
                h.Id, h.HadithId, h.StudentId, h.TeacherId, h.ClassId,
                h.MemorizedAt, h.Status, h.Notes)).ToList();

        var quranDtos = quranPages.Value
            .Select(q => new StudentQuraanPageDto(
                q.Id, q.PageNumber, q.StudentId, q.TeacherId, q.ClassId,
                q.MemorizedAt, q.Status, q.Notes)).ToList();

        return new StudentMemorizationProgressDto(
            query.StudentId,
            hadithDtos,
            quranDtos,
            hadithDtos.Count(h => h.Status == "Memorized"),
            quranDtos.Count(q => q.Status == "Memorized")
        );
    }
}
