using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Infrastructure;




public class AuthorizationSeeder
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthorizationSeeder> _logger;

    public AuthorizationSeeder(AppDbContext context, ILogger<AuthorizationSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {

            if (await _context.Permissions.AnyAsync())
            {
                _logger.LogInformation("Authorization data already seeded. Skipping...");
                return;
            }

            _logger.LogInformation("Seeding authorization data...");


            var permissions = CreateDefaultPermissions();
            await _context.Permissions.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();


            var roles = CreateDefaultRoles(permissions);
            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();


            var superAdminRole = roles.First(r => r.Type == "SuperAdmin");
            CreateDefaultUser(superAdminRole);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Authorization data seeded successfully.");
            _logger.LogInformation("Default SuperAdmin user created: Username='admin', Password='Admin@123'");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding authorization data.");
            throw;
        }
    }

    private List<Permission> CreateDefaultPermissions()
    {
        return new List<Permission>
        {

            Permission.FromString("students.view", "View student information"),
            Permission.FromString("students.viewSelf", "View own student information"),
            Permission.FromString("students.create", "Create new students"),
            Permission.FromString("students.update", "Update student information"),
            Permission.FromString("students.delete", "Delete students"),
            Permission.FromString("students.manage", "Full student management"),


            Permission.FromString("teachers.view", "View teacher information"),
            Permission.FromString("teachers.viewSelf", "View own teacher information"),
            Permission.FromString("teachers.create", "Create new teachers"),
            Permission.FromString("teachers.update", "Update teacher information"),
            Permission.FromString("teachers.delete", "Delete teachers"),
            Permission.FromString("teachers.manage", "Full teacher management"),


            Permission.FromString("points.view", "View points"),
            Permission.FromString("points.viewSelf", "View own points"),
            Permission.FromString("points.assign", "Assign points to students"),
            Permission.FromString("points.update", "Update points"),
            Permission.FromString("points.delete", "Delete points"),
            Permission.FromString("points.manage", "Full points management"),


            Permission.FromString("attendance.view", "View attendance"),
            Permission.FromString("attendance.viewSelf", "View own attendance"),
            Permission.FromString("attendance.mark", "Mark attendance"),
            Permission.FromString("attendance.update", "Update attendance records"),
            Permission.FromString("attendance.delete", "Delete attendance records"),


            Permission.FromString("hadith.view", "View hadith records"),
            Permission.FromString("hadith.viewSelf", "View own hadith records"),
            Permission.FromString("hadith.manage", "Manage hadith records"),
            Permission.FromString("hadith.assign", "Assign hadith to students"),


            Permission.FromString("quraan.view", "View Quran page records"),
            Permission.FromString("quraan.viewSelf", "View own Quran page records"),
            Permission.FromString("quraan.manage", "Manage Quran page records"),
            Permission.FromString("quraan.assign", "Assign Quran pages to students"),


            Permission.FromString("classes.view", "View classes"),
            Permission.FromString("classes.manage", "Manage classes"),
            Permission.FromString("classes.enroll", "Enroll students/teachers in classes"),


            Permission.FromString("semesters.view", "View semesters"),
            Permission.FromString("semesters.manage", "Manage semesters"),
            Permission.FromString("semesters.create", "Create new semesters"),


            Permission.FromString("reports.view", "View reports"),
            Permission.FromString("reports.generate", "Generate reports"),
            Permission.FromString("reports.export", "Export reports"),


            Permission.FromString("users.view", "View users"),
            Permission.FromString("users.manage", "Manage users"),
            Permission.FromString("roles.view", "View roles"),
            Permission.FromString("roles.manage", "Manage roles and permissions"),


            Permission.FromString("system.settings", "Access system settings"),
            Permission.FromString("system.audit", "View audit logs"),
        };
    }

    private List<Role> CreateDefaultRoles(List<Permission> allPermissions)
    {
        var roles = new List<Role>();
        Role superAdmin;
        if(_context.Roles.Any(x => x.Type == Constants.SuperAdminUserType))
        {
            superAdmin = _context.Roles.First(x => x.Type == Constants.SuperAdminUserType);
        }
        else
        {
            superAdmin = new Role { Type = "SuperAdmin" };
        }
        superAdmin.Permissions = [.. allPermissions];
        roles.Add(superAdmin);

        var admin = new Role { Type = "Admin" };
        admin.Permissions = allPermissions
            .Where(p => p.Resource != "system")
            .ToList();
        roles.Add(admin);


        var teacherPermissions = allPermissions.Where(p =>
            p.Resource == "students" && p.Action is "view" or "viewSelf" or "update" ||
            p.Resource == "points" && p.Action is "view" or "viewSelf" or "assign" ||
            p.Resource == "attendance" && p.Action is "view" or "viewSelf" or "mark" ||
            p.Resource == "hadith" && p.Action is "view" or "viewSelf" or "assign" or "manage" ||
            p.Resource == "quraan" && p.Action is "view" or "viewSelf" or "assign" or "manage" ||
            p.Resource == "classes" ||
            p.Resource == "teachers" && p.Action is "view" or "viewSelf"
        ).ToList();
        var teacher = new Role { Type = "Teacher" };
        teacher.Permissions = teacherPermissions;
        roles.Add(teacher);


        var studentPermissions = allPermissions.Where(p =>
            p.Action.EndsWith(".self") ||
            (p.Resource == "points" && p.Action == "view") ||
            (p.Resource == "attendance" && p.Action == "view") ||
            (p.Resource == "hadith" && p.Action == "view") ||
            (p.Resource == "quraan" && p.Action == "view") ||
            (p.Resource == "semesters" && p.Action == "view") ||
            (p.Resource == "reports" && p.Action == "view")
        ).ToList();
        var student = new Role { Type = "Student" };
        student.Permissions = studentPermissions;
        roles.Add(student);

        return roles;
    }

    private void CreateDefaultUser(Role superAdminRole)
    {
        if (_context.Users.Any())
        {
            return;
        }

        var defaultPassword = "Admin@123";
        var hashedPassword = Domain.Commons.PasswordHasher.Hash(defaultPassword);

        var superAdminUser = new User
        {
            UserName = "admin",
            HashedPassword = hashedPassword,
            RoleId = superAdminRole.Id
        };

        _context.Users.Add(superAdminUser);
    }
}
