using FileManager.Extensions;
using AlAshmar.Infrastructure.Services;
using AlAshmar.Application.Interfaces;
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




public static class InfrastructureServiceRegistration
{



    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddFileManager();


        services.AddScoped<IHttpContextServiceManager, HttpContextServiceManager>();


        services.AddScoped<ITokenService, TokenService>();


        services.AddScoped<IAuthenticationService, AuthenticationService>();


        services.AddScoped<Application.Interfaces.IAuthorizationService, AuthorizationService>();


        services.AddScoped<AuthorizationSeeder>();


        services.AddScoped<IStudentManagementService, StudentManagementService>();
        services.AddScoped<ITeacherManagementService, TeacherManagementService>();


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


        services.AddScoped<AlAshmar.Application.Services.Domain.IFormService, AlAshmar.Application.Services.Domain.FormService>();
        services.AddScoped<AlAshmar.Application.Services.Domain.IFormQuestionService, AlAshmar.Application.Services.Domain.FormQuestionService>();
        services.AddScoped<AlAshmar.Application.Services.Domain.IFormQuestionOptionService, AlAshmar.Application.Services.Domain.FormQuestionOptionService>();
        services.AddScoped<AlAshmar.Application.Services.Domain.IFormResponseService, AlAshmar.Application.Services.Domain.FormResponseService>();
        services.AddScoped<AlAshmar.Application.Services.Domain.IFormAnswerService, AlAshmar.Application.Services.Domain.FormAnswerService>();


        services.AddScoped<IStudentReportService, StudentReportService>();
        services.AddScoped<ITeacherReportService, TeacherReportService>();
        services.AddScoped<IClassReportService, ClassReportService>();
        services.AddScoped<ISemesterReportService, SemesterReportService>();
        services.AddScoped<IAttendanceReportService, AttendanceReportService>();
        services.AddScoped<IPointsReportService, PointsReportService>();



        return services;
    }




    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}
