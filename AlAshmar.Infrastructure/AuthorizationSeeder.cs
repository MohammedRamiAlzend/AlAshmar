using AlAshmar.Domain.Entities.Users;
using AlAshmar.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Infrastructure;

/// <summary>
/// Seeds default roles and permissions for the authorization system.
/// </summary>
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
            // Check if permissions already exist
            if (await _context.Permissions.AnyAsync())
            {
                _logger.LogInformation("Authorization data already seeded. Skipping...");
                return;
            }

            _logger.LogInformation("Seeding authorization data...");

            // Create permissions
            var permissions = CreateDefaultPermissions();
            await _context.Permissions.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();

            // Create roles with permissions
            var roles = CreateDefaultRoles(permissions);
            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Authorization data seeded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding authorization data.");
            throw;
        }
    }

    private static List<Permission> CreateDefaultPermissions()
    {
        return new List<Permission>
        {
            // Student permissions
            Permission.FromString("students.view", "View student information"),
            Permission.FromString("students.view.self", "View own student information"),
            Permission.FromString("students.create", "Create new students"),
            Permission.FromString("students.update", "Update student information"),
            Permission.FromString("students.delete", "Delete students"),
            Permission.FromString("students.manage", "Full student management"),

            // Teacher permissions
            Permission.FromString("teachers.view", "View teacher information"),
            Permission.FromString("teachers.view.self", "View own teacher information"),
            Permission.FromString("teachers.create", "Create new teachers"),
            Permission.FromString("teachers.update", "Update teacher information"),
            Permission.FromString("teachers.delete", "Delete teachers"),
            Permission.FromString("teachers.manage", "Full teacher management"),

            // Point permissions
            Permission.FromString("points.view", "View points"),
            Permission.FromString("points.view.self", "View own points"),
            Permission.FromString("points.assign", "Assign points to students"),
            Permission.FromString("points.update", "Update points"),
            Permission.FromString("points.delete", "Delete points"),
            Permission.FromString("points.manage", "Full points management"),

            // Attendance permissions
            Permission.FromString("attendance.view", "View attendance"),
            Permission.FromString("attendance.view.self", "View own attendance"),
            Permission.FromString("attendance.mark", "Mark attendance"),
            Permission.FromString("attendance.update", "Update attendance records"),
            Permission.FromString("attendance.delete", "Delete attendance records"),

            // Hadith permissions
            Permission.FromString("hadith.view", "View hadith records"),
            Permission.FromString("hadith.view.self", "View own hadith records"),
            Permission.FromString("hadith.manage", "Manage hadith records"),
            Permission.FromString("hadith.assign", "Assign hadith to students"),

            // Quran permissions
            Permission.FromString("quraan.view", "View Quran page records"),
            Permission.FromString("quraan.view.self", "View own Quran page records"),
            Permission.FromString("quraan.manage", "Manage Quran page records"),
            Permission.FromString("quraan.assign", "Assign Quran pages to students"),

            // Class permissions
            Permission.FromString("classes.view", "View classes"),
            Permission.FromString("classes.manage", "Manage classes"),
            Permission.FromString("classes.enroll", "Enroll students/teachers in classes"),

            // Semester permissions
            Permission.FromString("semesters.view", "View semesters"),
            Permission.FromString("semesters.manage", "Manage semesters"),
            Permission.FromString("semesters.create", "Create new semesters"),

            // Report permissions
            Permission.FromString("reports.view", "View reports"),
            Permission.FromString("reports.generate", "Generate reports"),
            Permission.FromString("reports.export", "Export reports"),

            // User/Role permissions
            Permission.FromString("users.view", "View users"),
            Permission.FromString("users.manage", "Manage users"),
            Permission.FromString("roles.view", "View roles"),
            Permission.FromString("roles.manage", "Manage roles and permissions"),

            // System permissions
            Permission.FromString("system.settings", "Access system settings"),
            Permission.FromString("system.audit", "View audit logs"),
        };
    }

    private static List<Role> CreateDefaultRoles(List<Permission> allPermissions)
    {
        var roles = new List<Role>();

        // SuperAdmin - All permissions
        var superAdmin = new Role { Type = "SuperAdmin" };
        superAdmin.Permissions = allPermissions.ToList();
        roles.Add(superAdmin);

        // Admin - Most permissions except system settings
        var admin = new Role { Type = "Admin" };
        admin.Permissions = allPermissions
            .Where(p => p.Resource != "system")
            .ToList();
        roles.Add(admin);

        // Teacher - Classroom-related permissions
        var teacherPermissions = allPermissions.Where(p =>
            p.Resource == "students" && p.Action is "view" or "view.self" or "update" ||
            p.Resource == "points" && p.Action is "view" or "view.self" or "assign" ||
            p.Resource == "attendance" && p.Action is "view" or "view.self" or "mark" ||
            p.Resource == "hadith" && p.Action is "view" or "view.self" or "assign" or "manage" ||
            p.Resource == "quraan" && p.Action is "view" or "view.self" or "assign" or "manage" ||
            p.Resource == "classes" ||
            p.Resource == "teachers" && p.Action is "view" or "view.self"
        ).ToList();
        var teacher = new Role { Type = "Teacher" };
        teacher.Permissions = teacherPermissions;
        roles.Add(teacher);

        // Student - View-only permissions for own data
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
}
