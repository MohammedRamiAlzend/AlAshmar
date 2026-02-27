using AlAshmar.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlAshmar.Domain.Commons;

public static class Constants
{
    public static Guid DefaultUserId => Guid.Parse("00000000-0000-0000-0000-000000000010");
    public static Guid DefaultRoleId => Guid.Parse("00000000-0000-0000-0000-000000000011");
    public static Guid DefaultTeacherRoleId => Guid.Parse("00000000-0000-0000-0000-000000000111");
    public static Guid DefaultStudentRoleId => Guid.Parse("00000000-0000-0000-0000-000000001111");
    public static string SuperAdminUserType => "SuperAdmin";
    public static string TeacherUserType => "Teacher";
    public static string StuedentUserType => "Student";

    public static User DefaultSuperAdminUser => new User { Id = DefaultUserId, UserName = "1", HashedPassword = "1", RoleId = DefaultRoleId };
    public static Role DefaultSuperAdminRole => new Role { Id = DefaultRoleId , Type = SuperAdminUserType };
    public static Role DefaultTeacherRole => new Role { Id = DefaultTeacherRoleId, Type = TeacherUserType };
    public static Role DefaultStudentRole => new Role { Id = DefaultStudentRoleId, Type = StuedentUserType };

}
