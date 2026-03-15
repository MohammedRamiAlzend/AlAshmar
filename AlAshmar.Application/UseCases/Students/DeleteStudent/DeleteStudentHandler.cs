using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Application.UseCases.Students.DeleteStudent;

public record DeleteStudentCommand(Guid Id) : IRequest<Result<Success>>;

public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, Result<Success>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public DeleteStudentHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<Success>> Handle(DeleteStudentCommand command, CancellationToken cancellationToken = default)
    {
        var student = await _repository.GetByIdAsync(command.Id);
        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

        var result = await _repository.RemoveAsync(s => s.Id == command.Id);
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }
}
