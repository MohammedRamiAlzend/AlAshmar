using AlAshmar.Application.Services.Domain;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Controllers;

//public class UsersController(IUserService service, IMapper mapper) : BaseController<User, UserDto, CreateUserDto, UpdateUserDto, Guid>(service, mapper)
//{
//    protected override Guid GetId(UserDto dto) => dto.Id;
//}

public class RolesController(IRoleService service, IMapper mapper) : BaseController<Role, RoleDto, RoleDto, RoleDto, Guid>(service, mapper)
{
    protected override Guid GetId(RoleDto dto) => dto.Id;
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
