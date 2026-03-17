using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Application.Services.Domain;

public interface IAllowableExtensionService : IAdvancedCrudService<AllowableExtension, AllowableExtensionDto, Guid> { }
public class AllowableExtensionService : CrudServiceBase<AllowableExtension, AllowableExtensionDto, Guid>, IAllowableExtensionService
{
    public AllowableExtensionService(IRepositoryBase<AllowableExtension, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IAttachmentService : IAdvancedCrudService<Attachment, AttachmentDto, Guid> { }
public class AttachmentService : CrudServiceBase<Attachment, AttachmentDto, Guid>, IAttachmentService
{
    public AttachmentService(IRepositoryBase<Attachment, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IContactInfoService : IAdvancedCrudService<ContactInfo, ContactInfoDto, Guid> { }
public class ContactInfoService : CrudServiceBase<ContactInfo, ContactInfoDto, Guid>, IContactInfoService
{
    public ContactInfoService(IRepositoryBase<ContactInfo, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IUserService : IAdvancedCrudService<User, UserDto, Guid> { }
public class UserService : CrudServiceBase<User, UserDto, Guid>, IUserService
{
    public UserService(IRepositoryBase<User, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IRoleService : IAdvancedCrudService<Role, RoleDto, Guid> { }
public class RoleService : CrudServiceBase<Role, RoleDto, Guid>, IRoleService
{
    public RoleService(IRepositoryBase<Role, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IManagerService : IAdvancedCrudService<Manager, ManagerDto, Guid> { }
public class ManagerService : CrudServiceBase<Manager, ManagerDto, Guid>, IManagerService
{
    public ManagerService(IRepositoryBase<Manager, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface ITeacherService : IAdvancedCrudService<Teacher, TeacherDto, Guid> { }
public class TeacherService : CrudServiceBase<Teacher, TeacherDto, Guid>, ITeacherService
{
    public TeacherService(IRepositoryBase<Teacher, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface ITeacherAttendanceService : IAdvancedCrudService<TeacherAttendance, TeacherAttendanceDto, Guid> { }
public class TeacherAttendanceService : CrudServiceBase<TeacherAttendance, TeacherAttendanceDto, Guid>, ITeacherAttendanceService
{
    public TeacherAttendanceService(IRepositoryBase<TeacherAttendance, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IClassTeacherEnrollmentService : IAdvancedCrudService<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, Guid> { }
public class ClassTeacherEnrollmentService : CrudServiceBase<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, Guid>, IClassTeacherEnrollmentService
{
    public ClassTeacherEnrollmentService(IRepositoryBase<ClassTeacherEnrollment, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IStudentAttendanceService : IAdvancedCrudService<StudentAttendance, StudentAttendanceDto, Guid> { }
public class StudentAttendanceService : CrudServiceBase<StudentAttendance, StudentAttendanceDto, Guid>, IStudentAttendanceService
{
    public StudentAttendanceService(IRepositoryBase<StudentAttendance, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IClassStudentEnrollmentService : IAdvancedCrudService<ClassStudentEnrollment, ClassEnrollmentWithStudentDto, Guid> { }
public class ClassStudentEnrollmentService : CrudServiceBase<ClassStudentEnrollment, ClassEnrollmentWithStudentDto, Guid>, IClassStudentEnrollmentService
{
    public ClassStudentEnrollmentService(IRepositoryBase<ClassStudentEnrollment, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IBookService : IAdvancedCrudService<Book, BookDto, Guid> { }
public class BookService : CrudServiceBase<Book, BookDto, Guid>, IBookService
{
    public BookService(IRepositoryBase<Book, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IHadithService : IAdvancedCrudService<Hadith, HadithDto, Guid> { }
public class HadithService : CrudServiceBase<Hadith, HadithDto, Guid>, IHadithService
{
    public HadithService(IRepositoryBase<Hadith, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface ISemesterService : IAdvancedCrudService<Semester, SemesterDto, Guid> { }
public class SemesterService : CrudServiceBase<Semester, SemesterDto, Guid>, ISemesterService
{
    public SemesterService(IRepositoryBase<Semester, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IPointCategoryService : IAdvancedCrudService<PointCategory, PointCategoryDto, Guid> { }
public class PointCategoryService : CrudServiceBase<PointCategory, PointCategoryDto, Guid>, IPointCategoryService
{
    public PointCategoryService(IRepositoryBase<PointCategory, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IPointService : IAdvancedCrudService<Point, PointDto, Guid> { }
public class PointService : CrudServiceBase<Point, PointDto, Guid>, IPointService
{
    public PointService(IRepositoryBase<Point, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}
