
namespace AlAshmar.Infrastructure.Authorization.Policies;




public static class AuthorizationPolicies
{



    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ResourceOwnershipHandler>();
        services.AddScoped<IAuthorizationHandler, ClassEnrollmentHandler>();
        services.AddSingleton<IAuthorizationHandler, TimeBasedAuthorizationHandler>();


        services.AddAuthorization(options =>
        {




            options.AddPolicy("owns:students", policy =>
                policy.RequireAuthenticatedUser()
                      .AddRequirements(new ResourceOwnershipRequirement("students")));

            options.AddPolicy("owns:teachers", policy =>
                policy.RequireAuthenticatedUser()
                      .AddRequirements(new ResourceOwnershipRequirement("teachers")));


            options.AddPolicy("EnrolledInClass", policy =>
                policy.RequireAuthenticatedUser()
                      .AddRequirements(new ClassEnrollmentRequirement()));


            options.AddPolicy("SchoolHours", policy =>
                policy.RequireAuthenticatedUser()
                      .AddRequirements(new TimeBasedRequirement(8, 17, [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday])));

            options.AddPolicy("ExamHours", policy =>
                policy.RequireAuthenticatedUser()
                      .AddRequirements(new TimeBasedRequirement(8, 16, [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday])));


            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin", "SuperAdmin"));

            options.AddPolicy("TeacherOnly", policy =>
                policy.RequireRole("Teacher", "Admin", "SuperAdmin"));

            options.AddPolicy("StudentOnly", policy =>
                policy.RequireRole("Student"));


            options.AddPolicy("CanManageStudents", policy =>
                policy.RequireRole("Teacher", "Admin", "SuperAdmin")
                      .RequireAssertion(context =>
              {

                  return context.User.Claims.Any(c => c.Type == "permission" && c.Value.StartsWith("students."));
              }));

            options.AddPolicy("CanAssignPoints", policy =>
                policy.RequireRole("Teacher", "Admin", "SuperAdmin")
                      .RequireAssertion(context =>
              {
                  return context.User.Claims.Any(c => c.Type == "permission" && (c.Value == "points.assign" || c.Value == "points.*"));
              }));


            options.AddPolicy("SuperAdminOnly", policy =>
                policy.RequireRole("SuperAdmin"));
        });

        return services;
    }
}
