using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Courses.DeleteCourse;

public record DeleteCourseCommand(Guid Id) : ICommand<Result<Success>>;

public class DeleteCourseHandler(IRepositoryBase<Course, Guid> repository)
    : IRequestHandler<DeleteCourseCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(DeleteCourseCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return new Error("404", "Course not found", ErrorKind.NotFound);

        var deleteResult = await repository.RemoveAsync(d => d.Id == command.Id);
        if (deleteResult.IsError)
            return deleteResult.Errors;

        return Result.Success;
    }
}
