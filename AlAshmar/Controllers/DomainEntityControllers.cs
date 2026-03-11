using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Controllers;

public class UsersController(IUserService service, IMapper mapper) : BaseController<User, UserDto, CreateUserDto, UpdateUserDto, Guid>(service, mapper)
{
    protected override Guid GetId(UserDto dto) => dto.Id;
}

public class RolesController(IRoleService service, IMapper mapper) : BaseController<Role, RoleDto, RoleDto, RoleDto, Guid>(service, mapper)
{
    protected override Guid GetId(RoleDto dto) => dto.Id;
}

public class ManagersController(IManagerService service, IMapper mapper) : BaseController<Manager, ManagerDto, CreateManagerDto, UpdateManagerDto, Guid>(service, mapper)
{
    protected override Guid GetId(ManagerDto dto) => dto.Id;
}

public class TeachersController(ITeacherService service, IMapper mapper) : BaseController<Teacher, TeacherDto, CreateTeacherDto, UpdateTeacherDto, Guid>(service, mapper)
{
    protected override Guid GetId(TeacherDto dto) => dto.Id;
}

public class TeacherAttencancesController(ITeacherAttencanceService service, IMapper mapper) : BaseController<TeacherAttencance, TeacherAttencanceDto, TeacherAttencanceDto, TeacherAttencanceDto, Guid>(service, mapper)
{
    protected override Guid GetId(TeacherAttencanceDto dto) => dto.Id;
}

public class ClassTeacherEnrollmentsController(IClassTeacherEnrollmentService service, IMapper mapper) : BaseController<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, ClassTeacherEnrollmentDto, ClassTeacherEnrollmentDto, Guid>(service, mapper)
{
    protected override Guid GetId(ClassTeacherEnrollmentDto dto) => dto.Id;
}

public class StudentAttendancesController(IStudentAttendanceService service, IMapper mapper)
    : BaseController<StudentAttendance, StudentAttendanceDto, StudentAttendanceDto, StudentAttendanceDto, Guid>(service, mapper)
{
    protected override Guid GetId(StudentAttendanceDto dto) => dto.Id;
}
