global using Scalar.AspNetCore;
global using AlAshmar.Infrastructure.Persistence;
global using AlAshmar.Infrastructure;
global using AlAshmar.Application.Interfaces;
global using AlAshmar.Application.Common;
global using AlAshmar.Domain.Commons;
global using AlAshmar.Infrastructure.Authorization.Policies;
global using AlAshmar.Application.Services.Domain;

var builder = WebApplication.CreateBuilder(args);

// Register persistence services (DbContext, Repository, UnitOfWork)
builder.Services.AddPersistenceServices(builder.Configuration);

// Register infrastructure services (Authentication, Token, HttpContext, Authorization)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register authorization policies and handlers
AuthorizationPolicies.AddAuthorizationPolicies(builder.Services);

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(EntityMappingProfile).Assembly);

// CORS
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

// Controllers
builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// JWT Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
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

// Seed default roles and permissions
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

// Separate method for clarity
async Task SeedAuthorizationDataAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<AuthorizationSeeder>();
    await seeder.SeedAsync();
}
