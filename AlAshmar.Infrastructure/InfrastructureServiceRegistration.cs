using FileManager.Extensions;
using AlAshmar.Infrastructure.Services;
using AlAshmar.Application.Interfaces;
using AlAshmar.Application.Common;
using AlAshmar.Infrastructure.Persistence;
using AlAshmar.Infrastructure.Services.Domain;
using AlAshmar.Infrastructure.Services.Reports;
using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register File Manager Services
        services.AddFileManager();

        // Register HttpContext Service Manager
        services.AddScoped<IHttpContextServiceManager, HttpContextServiceManager>();

        // Register Token Service (needs DbContext for permissions)
        services.AddScoped<ITokenService, TokenService>();

        // Register Authentication Service
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Register Authorization Service
        services.AddScoped<Application.Interfaces.IAuthorizationService, AuthorizationService>();

        // Register Authorization Seeder
        services.AddScoped<AuthorizationSeeder>();

        // Register student and teacher management services
        services.AddScoped<IStudentManagementService, StudentManagementService>();
        services.AddScoped<ITeacherManagementService, TeacherManagementService>();

        // Register Domain CRUD Services
        services.AddScoped<IAllowableExtentionService, AllowableExtentionService>();
        services.AddScoped<IAttacmentService, AttacmentService>();
        services.AddScoped<IContactInfoService, ContactInfoService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IManagerService, ManagerService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<ITeacherAttencanceService, TeacherAttencanceService>();
        services.AddScoped<IClassTeacherEnrollmentService, ClassTeacherEnrollmentService>();
        services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
        services.AddScoped<IClassStudentEnrollmentService, ClassStudentEnrollmentService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IHadithService, HadithService>();
        services.AddScoped<ISemesterService, SemesterService>();
        services.AddScoped<IPointCategoryService, PointCategoryService>();
        services.AddScoped<IPointService, PointService>();

        // Register Report Services
        services.AddScoped<IStudentReportService, StudentReportService>();
        services.AddScoped<ITeacherReportService, TeacherReportService>();
        services.AddScoped<IClassReportService, ClassReportService>();
        services.AddScoped<ISemesterReportService, SemesterReportService>();
        services.AddScoped<IAttendanceReportService, AttendanceReportService>();
        services.AddScoped<IPointsReportService, PointsReportService>();

        // Note: MediatR handlers are registered via AddMediatR() in Program.cs

        return services;
    }

    /// <summary>
    /// Adds authorization policies for the application.
    /// </summary>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}
