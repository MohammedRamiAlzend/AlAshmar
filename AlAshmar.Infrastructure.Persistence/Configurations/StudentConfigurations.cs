using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(s => s.FatherName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(s => s.MotherName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(s => s.NationalityNumber)
            .HasMaxLength(50);
        
        builder.Property(s => s.Email)
            .HasMaxLength(255);
        
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(s => s.StudentContactInfos)
            .WithOne(sci => sci.Student)
            .HasForeignKey(sci => sci.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.StudentAttachments)
            .WithOne(sa => sa.Student)
            .HasForeignKey(sa => sa.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.StudentHadiths)
            .WithOne(sh => sh.Student)
            .HasForeignKey(sh => sh.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.StudentQuraanPages)
            .WithOne(sq => sq.Student)
            .HasForeignKey(sq => sq.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.StudentClassEventsPoints)
            .WithOne(sc => sc.Student)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(s => s.Points)
            .WithOne(p => p.Student)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(s => s.NationalityNumber)
            .IsUnique();
        
        builder.HasIndex(s => s.Email);
    }
}

public class StudentContactInfoConfiguration : IEntityTypeConfiguration<StudentContactInfo>
{
    public void Configure(EntityTypeBuilder<StudentContactInfo> builder)
    {
        builder.ToTable("StudentContactInfos");
        
        builder.HasKey(sci => new { sci.StudentId, sci.ContactInfoId });
        
        builder.HasOne(sci => sci.Student)
            .WithMany(s => s.StudentContactInfos)
            .HasForeignKey(sci => sci.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sci => sci.ContactInfo)
            .WithMany()
            .HasForeignKey(sci => sci.ContactInfoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class StudentAttachmentConfiguration : IEntityTypeConfiguration<StudentAttachment>
{
    public void Configure(EntityTypeBuilder<StudentAttachment> builder)
    {
        builder.ToTable("StudentAttachments");
        
        builder.HasKey(sa => new { sa.StudentId, sa.AttachmentId });
        
        builder.HasOne(sa => sa.Student)
            .WithMany(s => s.StudentAttachments)
            .HasForeignKey(sa => sa.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sa => sa.Attachment)
            .WithMany()
            .HasForeignKey(sa => sa.AttachmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class StudentHadithConfiguration : IEntityTypeConfiguration<StudentHadith>
{
    public void Configure(EntityTypeBuilder<StudentHadith> builder)
    {
        builder.ToTable("StudentHadiths");
        
        builder.HasKey(sh => sh.Id);
        
        builder.Property(sh => sh.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasOne(sh => sh.Hadith)
            .WithMany()
            .HasForeignKey(sh => sh.HadithId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sh => sh.Student)
            .WithMany(s => s.StudentHadiths)
            .HasForeignKey(sh => sh.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sh => sh.Teacher)
            .WithMany()
            .HasForeignKey(sh => sh.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(sh => sh.Status)
            .HasMaxLength(50);
        
        builder.Property(sh => sh.Notes)
            .HasMaxLength(1000);
        
        builder.HasIndex(sh => new { sh.StudentId, sh.HadithId });
    }
}

public class StudentQuraanPageConfiguration : IEntityTypeConfiguration<StudentQuraanPage>
{
    public void Configure(EntityTypeBuilder<StudentQuraanPage> builder)
    {
        builder.ToTable("StudentQuraanPages");
        
        builder.HasKey(sq => sq.Id);
        
        builder.Property(sq => sq.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(sq => sq.PageNumber)
            .IsRequired();
        
        builder.HasOne(sq => sq.Student)
            .WithMany(s => s.StudentQuraanPages)
            .HasForeignKey(sq => sq.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sq => sq.Teacher)
            .WithMany()
            .HasForeignKey(sq => sq.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(sq => sq.Status)
            .HasMaxLength(50);
        
        builder.Property(sq => sq.Notes)
            .HasMaxLength(1000);
        
        builder.HasIndex(sq => new { sq.StudentId, sq.PageNumber });
    }
}

public class StudentAttendanceConfiguration : IEntityTypeConfiguration<StudentAttendance>
{
    public void Configure(EntityTypeBuilder<StudentAttendance> builder)
    {
        builder.ToTable("StudentAttendances");
        
        builder.HasKey(sa => sa.Id);
        
        builder.Property(sa => sa.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(sa => sa.StartDate)
            .IsRequired();
        
        builder.Property(sa => sa.EndDate)
            .IsRequired();
        
        builder.HasIndex(sa => new { sa.ClassStudentId, sa.StartDate, sa.EndDate });
    }
}

public class StudentClassEventsPointConfiguration : IEntityTypeConfiguration<StudentClassEventsPoint>
{
    public void Configure(EntityTypeBuilder<StudentClassEventsPoint> builder)
    {
        builder.ToTable("StudentClassEventsPoints");
        
        builder.HasKey(sc => sc.Id);
        
        builder.Property(sc => sc.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasOne(sc => sc.Student)
            .WithMany(s => s.StudentClassEventsPoints)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sc => sc.Semester)
            .WithMany()
            .HasForeignKey(sc => sc.SmesterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(sc => sc.QuranPoints)
            .HasDefaultValue(0);
        
        builder.Property(sc => sc.HadithPoints)
            .HasDefaultValue(0);
        
        builder.Property(sc => sc.AttendancePoints)
            .HasDefaultValue(0);
        
        builder.Property(sc => sc.BehaviorPoints)
            .HasDefaultValue(0);
        
        builder.Property(sc => sc.TotalPoints)
            .HasDefaultValue(0);
        
        builder.HasIndex(sc => new { sc.StudentId, sc.SmesterId, sc.ClassId });
    }
}

public class ClassStudentEnrollmentConfiguration : IEntityTypeConfiguration<ClassStudentEnrollment>
{
    public void Configure(EntityTypeBuilder<ClassStudentEnrollment> builder)
    {
        builder.ToTable("ClassStudentEnrollments");
        
        builder.HasKey(cse => cse.Id);
        
        builder.Property(cse => cse.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasOne(cse => cse.Student)
            .WithMany()
            .HasForeignKey(cse => cse.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(cse => new { cse.StudentId, cse.ClassId });
    }
}
