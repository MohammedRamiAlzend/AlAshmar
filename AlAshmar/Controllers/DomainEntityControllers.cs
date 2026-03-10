using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Controllers;

public class UsersController : BaseController<User, UserDto, CreateUserDto, UpdateUserDto, Guid>
{
    public UsersController(IRepositoryBase<User, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(UserDto dto) => dto.Id;
}

public class RolesController : BaseController<Role, RoleDto, RoleDto, RoleDto, Guid>
{
    public RolesController(IRepositoryBase<Role, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(RoleDto dto) => dto.Id;
}

public class ManagersController : BaseController<Manager, ManagerDto, CreateManagerDto, UpdateManagerDto, Guid>
{
    public ManagersController(IRepositoryBase<Manager, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(ManagerDto dto) => dto.Id;
}

public class TeachersController : BaseController<Teacher, TeacherDto, CreateTeacherDto, UpdateTeacherDto, Guid>
{
    public TeachersController(IRepositoryBase<Teacher, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(TeacherDto dto) => dto.Id;
}

public class TeacherAttencancesController : BaseController<TeacherAttencance, TeacherAttencanceDto, TeacherAttencanceDto, TeacherAttencanceDto, Guid>
{
    public TeacherAttencancesController(IRepositoryBase<TeacherAttencance, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(TeacherAttencanceDto dto) => dto.Id;
}

public class ClassTeacherEnrollmentsController : BaseController<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, ClassTeacherEnrollmentDto, ClassTeacherEnrollmentDto, Guid>
{
    public ClassTeacherEnrollmentsController(IRepositoryBase<ClassTeacherEnrollment, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(ClassTeacherEnrollmentDto dto) => dto.Id;
}

// Note: Student CRUD operations are now handled by StudentManagementController
// which provides specialized endpoints for different student scenarios.
// - Use StudentManagementController for student operations (GET, POST, PUT, DELETE with specialized DTOs)
// - Use StudentAttendancesController for student attendance CRUD operations

public class StudentAttendancesController : BaseController<StudentAttendance, StudentAttendanceDto, StudentAttendanceDto, StudentAttendanceDto, Guid>
{
    public StudentAttendancesController(IRepositoryBase<StudentAttendance, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(StudentAttendanceDto dto) => dto.Id;
}
