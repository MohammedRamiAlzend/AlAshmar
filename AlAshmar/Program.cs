global using Scalar.AspNetCore;
global using AlAshmar.Infrastructure.Persistence;
global using AlAshmar.Infrastructure;
global using AlAshmar.Application.Interfaces;
global using AlAshmar.Application.Common;
global using AlAshmar.Domain.Commons;
global using AlAshmar.Infrastructure.Authorization.Policies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);

AuthorizationPolicies.AddAuthorizationPolicies(builder.Services);

builder.Services.AddAutoMapper(typeof(EntityMappingProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

await SeedAuthorizationDataAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();

static async Task SeedAuthorizationDataAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<AuthorizationSeeder>();
    await seeder.SeedAsync();
}
