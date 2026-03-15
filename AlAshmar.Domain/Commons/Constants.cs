using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Domain.Commons;

public static class Constants
{
    public static Guid DefaultUserId => Guid.Parse("00000000-0000-0000-0000-000000000010");
    public static Guid DefaultRoleId => Guid.Parse("00000000-0000-0000-0000-000000000011");
    public static Guid DefaultTeacherRoleId => Guid.Parse("00000000-0000-0000-0000-000000000111");
    public static Guid DefaultStudentRoleId => Guid.Parse("00000000-0000-0000-0000-000000001111");
    public static Guid DefaultManagerRoleId => Guid.Parse("00000000-0000-0000-0000-000000011111");
    public static string SuperAdminUserType => "SuperAdmin";
    public static string TeacherUserType => "Teacher";
    public static string StuedentUserType => "Student";
    public static string ManagerUserType => "Manager";

    public static User DefaultSuperAdminUser => new() { Id = DefaultUserId, UserName = "1", HashedPassword = "$2a$11$ugm3BJebpiSW.gjw5fVcYedC2J1OsKlNdykJ6wtVJIemVYnT2rJQO", RoleId = DefaultRoleId };
    public static Role DefaultSuperAdminRole => new() { Id = DefaultRoleId, Type = SuperAdminUserType };
    public static Role DefaultTeacherRole => new() { Id = DefaultTeacherRoleId, Type = TeacherUserType };
    public static Role DefaultStudentRole => new() { Id = DefaultStudentRoleId, Type = StuedentUserType };

}
