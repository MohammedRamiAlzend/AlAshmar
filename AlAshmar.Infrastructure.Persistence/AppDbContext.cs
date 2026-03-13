using Microsoft.EntityFrameworkCore;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Forms;

namespace AlAshmar.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
        
    }
    // Core Entities
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Attacment> Attachments { get; set; }
    public DbSet<AllowableExtention> AllowableExtentions { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }

    // Manager Entities
    public DbSet<Manager> Managers { get; set; }
    public DbSet<ManagerContactInfo> ManagerContactInfos { get; set; }
    public DbSet<ManagerAttachment> ManagerAttachments { get; set; }

    // Teacher Entities
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherContactInfo> TeacherContactInfos { get; set; }
    public DbSet<TeacherAttachment> TeacherAttachments { get; set; }
    public DbSet<TeacherAttencance> TeacherAttencances { get; set; }
    public DbSet<ClassTeacherEnrollment> ClassTeacherEnrollments { get; set; }

    // Student Entities
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentContactInfo> StudentContactInfos { get; set; }
    public DbSet<StudentAttachment> StudentAttachments { get; set; }
    public DbSet<StudentHadith> StudentHadiths { get; set; }
    public DbSet<StudentQuraanPage> StudentQuraanPages { get; set; }
    public DbSet<StudentAttendance> StudentAttendances { get; set; }
    public DbSet<StudentClassEventsPoint> StudentClassEventsPoints { get; set; }
    public DbSet<ClassStudentEnrollment> ClassStudentEnrollments { get; set; }

    // Academic Entities
    public DbSet<Book> Books { get; set; }
    public DbSet<Hadith> Hadiths { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Halaqa> Halaqas { get; set; }
    public DbSet<Point> Points { get; set; }
    public DbSet<PointCategory> PointCategories { get; set; }

    // Form Entities
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormQuestion> FormQuestions { get; set; }
    public DbSet<FormQuestionOption> FormQuestionOptions { get; set; }
    public DbSet<FormResponse> FormResponses { get; set; }
    public DbSet<FormAnswer> FormAnswers { get; set; }
    public DbSet<FormAnswerSelectedOption> FormAnswerSelectedOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        // Ignore DomainEvent type (it's not an entity, just a base class)
        modelBuilder.Ignore<Domain.Events.DomainEvent>();
    }
}
