using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Controllers;

public class UsersController(IRepositoryBase<User, Guid> repository, IMapper mapper) : BaseController<User, UserDto, CreateUserDto, UpdateUserDto, Guid>(repository, mapper)
{
    protected override Guid GetId(UserDto dto) => dto.Id;
}

public class RolesController(IRepositoryBase<Role, Guid> repository, IMapper mapper) : BaseController<Role, RoleDto, RoleDto, RoleDto, Guid>(repository, mapper)
{
    protected override Guid GetId(RoleDto dto) => dto.Id;
}

public class ManagersController(IRepositoryBase<Manager, Guid> repository, IMapper mapper) : BaseController<Manager, ManagerDto, CreateManagerDto, UpdateManagerDto, Guid>(repository, mapper)
{
    protected override Guid GetId(ManagerDto dto) => dto.Id;
}

public class TeachersController(IRepositoryBase<Teacher, Guid> repository, IMapper mapper) : BaseController<Teacher, TeacherDto, CreateTeacherDto, UpdateTeacherDto, Guid>(repository, mapper)
{
    protected override Guid GetId(TeacherDto dto) => dto.Id;
}

public class TeacherAttencancesController(IRepositoryBase<TeacherAttencance, Guid> repository, IMapper mapper) : BaseController<TeacherAttencance, TeacherAttencanceDto, TeacherAttencanceDto, TeacherAttencanceDto, Guid>(repository, mapper)
{
    protected override Guid GetId(TeacherAttencanceDto dto) => dto.Id;
}

public class ClassTeacherEnrollmentsController(IRepositoryBase<ClassTeacherEnrollment, Guid> repository, IMapper mapper) : BaseController<ClassTeacherEnrollment, ClassTeacherEnrollmentDto, ClassTeacherEnrollmentDto, ClassTeacherEnrollmentDto, Guid>(repository, mapper)
{
    protected override Guid GetId(ClassTeacherEnrollmentDto dto) => dto.Id;
}
public class StudentAttendancesController(IRepositoryBase<StudentAttendance, Guid> repository, IMapper mapper)
    : BaseController<StudentAttendance, StudentAttendanceDto, StudentAttendanceDto, StudentAttendanceDto, Guid>(repository, mapper)
{
    protected override Guid GetId(StudentAttendanceDto dto) => dto.Id;
}
