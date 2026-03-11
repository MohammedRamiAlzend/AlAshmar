using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Infrastructure.Services.Domain;

// ==================== COMMON DOMAIN SERVICES ====================

public interface IAllowableExtentionService : IAdvancedCrudService<AllowableExtention, AllowableExtentionDto, Guid> { }
public class AllowableExtentionService : CrudServiceBase<AllowableExtention, AllowableExtentionDto, Guid>, IAllowableExtentionService
{
    public AllowableExtentionService(IRepositoryBase<AllowableExtention, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IAttacmentService : IAdvancedCrudService<Attacment, AttacmentDto, Guid> { }
public class AttacmentService : CrudServiceBase<Attacment, AttacmentDto, Guid>, IAttacmentService
{
    public AttacmentService(IRepositoryBase<Attacment, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IContactInfoService : IAdvancedCrudService<ContactInfo, ContactInfoDto, Guid> { }
public class ContactInfoService : CrudServiceBase<ContactInfo, ContactInfoDto, Guid>, IContactInfoService
{
    public ContactInfoService(IRepositoryBase<ContactInfo, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

// ==================== USERS DOMAIN SERVICES ====================

public interface IUserService : IAdvancedCrudService<User, UserDto, Guid> { }
public class UserService : CrudServiceBase<User, UserDto, Guid>, IUserService
{
    public UserService(IRepositoryBase<User, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IRoleService : IAdvancedCrudService<Role, AlAshmar.Application.DTOs.Domain.RoleDto, Guid> { }
public class RoleService : CrudServiceBase<Role, AlAshmar.Application.DTOs.Domain.RoleDto, Guid>, IRoleService
{
    public RoleService(IRepositoryBase<Role, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

// ==================== MANAGERS DOMAIN SERVICES ====================

public interface IManagerService : IAdvancedCrudService<Manager, AlAshmar.Application.DTOs.ManagerDto, Guid> { }
public class ManagerService : CrudServiceBase<Manager, AlAshmar.Application.DTOs.ManagerDto, Guid>, IManagerService
{
    public ManagerService(IRepositoryBase<Manager, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

// ==================== TEACHERS DOMAIN SERVICES ====================

public interface ITeacherService : IAdvancedCrudService<Teacher, AlAshmar.Application.DTOs.TeacherDto, Guid> { }
public class TeacherService : CrudServiceBase<Teacher, AlAshmar.Application.DTOs.TeacherDto, Guid>, ITeacherService
{
    public TeacherService(IRepositoryBase<Teacher, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface ITeacherAttencanceService : IAdvancedCrudService<TeacherAttencance, TeacherAttencanceDto, Guid> { }
public class TeacherAttencanceService : CrudServiceBase<TeacherAttencance, TeacherAttencanceDto, Guid>, ITeacherAttencanceService
{
    public TeacherAttencanceService(IRepositoryBase<TeacherAttencance, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

public interface IClassTeacherEnrollmentService : IAdvancedCrudService<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, Guid> { }
public class ClassTeacherEnrollmentService : CrudServiceBase<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, Guid>, IClassTeacherEnrollmentService
{
    public ClassTeacherEnrollmentService(IRepositoryBase<ClassTeacherEnrollment, Guid> repository, IMapper mapper)
        : base(repository, mapper) { }
}

// ==================== STUDENTS DOMAIN SERVICES ====================
// Note: For student operations, use the specialized student DTOs:
// - StudentListItemDto for lists
// - StudentDetailDto for full details
// - StudentBasicInfoDto for creation responses
// The legacy StudentDto has been deprecated and split into specialized DTOs.

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

// ==================== ACADEMIC DOMAIN SERVICES ====================

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
