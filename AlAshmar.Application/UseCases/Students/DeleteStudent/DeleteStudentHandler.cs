using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.DeleteStudent;

public record DeleteStudentCommand(Guid Id) : ICommand<Result<Success>>;

public class DeleteStudentHandler : ICommandHandler<DeleteStudentCommand, Result<Success>>
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
            return new Error("404", "Student not found", ErrorKind.NotFound);

        var result = await _repository.RemoveAsync(s => s.Id == command.Id);
        if (result.IsError)
            return result.Errors;
        
        return Result.Success;
    }
}
