using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Application.Repos;
using MediatR;

namespace AlAshmar.Application.UseCases.Teachers.DeleteTeacher;

public record DeleteTeacherCommand(Guid Id) : ICommand<Result<Success>>;

public class DeleteTeacherHandler : IRequestHandler<DeleteTeacherCommand, Result<Success>>
{
    private readonly IRepositoryBase<Teacher, Guid> _repository;

    public DeleteTeacherHandler(IRepositoryBase<Teacher, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<Success>> Handle(DeleteTeacherCommand command, CancellationToken cancellationToken = default)
    {
        var teacher = await _repository.GetByIdAsync(command.Id);
        if (teacher.Value == null)
            return ApplicationErrors.TeacherNotFound;

        var result = await _repository.RemoveAsync(t => t.Id == command.Id);
        if (result.IsError)
            return result.Errors;
        
        return Result.Success;
    }
}
