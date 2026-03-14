using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;




public sealed class CourseIncludes : IEntityIncludes<Course>
{
    private readonly IReadOnlyList<Func<IQueryable<Course>, IQueryable<Course>>> _steps;

    private CourseIncludes(IEnumerable<Func<IQueryable<Course>, IQueryable<Course>>> steps)
    {
        _steps = steps.ToList();
    }




    public static readonly CourseIncludes None = new([]);


    public static readonly CourseIncludes Basic = None
        .WithSemester();


    public static readonly CourseIncludes Full = None
        .WithSemester()
        .WithHalaqas();




    public CourseIncludes WithSemester() =>
        Add(q => q.Include(d => d.Semester));


    public CourseIncludes WithHalaqas() =>
        Add(q => q.Include(d => d.Halaqas));




    public Func<IQueryable<Course>, IQueryable<Course>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));



    private CourseIncludes Add(Func<IQueryable<Course>, IQueryable<Course>> step) =>
        new(_steps.Append(step));
}
