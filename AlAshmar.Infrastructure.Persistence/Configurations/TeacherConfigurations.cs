using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlAshmar.Infrastructure.Persistence.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.FatherName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.MotherName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.NationalityNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Email)
            .HasMaxLength(255);

        builder.HasOne(t => t.RelatedUser)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.TeacherContactInfos)
            .WithOne(tci => tci.Teacher)
            .HasForeignKey(tci => tci.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TeacherAttachments)
            .WithOne(ta => ta.Teacher)
            .HasForeignKey(ta => ta.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.ClassTeacherEnrollments)
            .WithOne(cte => cte.Teacher)
            .HasForeignKey(cte => cte.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.GivenPoints)
            .WithOne(p => p.GivenByTeacher)
            .HasForeignKey(p => p.GivenByTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(t => t.NationalityNumber)
            .IsUnique();

        builder.HasIndex(t => t.Email);
    }
}

public class TeacherContactInfoConfiguration : IEntityTypeConfiguration<TeacherContactInfo>
{
    public void Configure(EntityTypeBuilder<TeacherContactInfo> builder)
    {
        builder.ToTable("TeacherContactInfos");

        builder.HasKey(tci => new { tci.TeacherId, tci.ContactInfoId });

        builder.HasOne(tci => tci.Teacher)
            .WithMany(t => t.TeacherContactInfos)
            .HasForeignKey(tci => tci.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tci => tci.ContactInfo)
            .WithMany()
            .HasForeignKey(tci => tci.ContactInfoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TeacherAttachmentConfiguration : IEntityTypeConfiguration<TeacherAttachment>
{
    public void Configure(EntityTypeBuilder<TeacherAttachment> builder)
    {
        builder.ToTable("TeacherAttachments");

        builder.HasKey(ta => new { ta.TeacherId, ta.AttachmentId });

        builder.HasOne(ta => ta.Teacher)
            .WithMany(t => t.TeacherAttachments)
            .HasForeignKey(ta => ta.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ta => ta.Attachment)
            .WithMany()
            .HasForeignKey(ta => ta.AttachmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TeacherAttencanceConfiguration : IEntityTypeConfiguration<TeacherAttencance>
{
    public void Configure(EntityTypeBuilder<TeacherAttencance> builder)
    {
        builder.ToTable("TeacherAttencances");

        builder.HasKey(ta => ta.Id);

        builder.Property(ta => ta.Id)
            .ValueGeneratedOnAdd();

        builder.Property(ta => ta.StartDate)
            .IsRequired();

        builder.Property(ta => ta.EndDate)
            .IsRequired();

        builder.HasIndex(ta => new { ta.ClassTeacherId, ta.StartDate, ta.EndDate });
    }
}

public class ClassTeacherEnrollmentConfiguration : IEntityTypeConfiguration<ClassTeacherEnrollment>
{
    public void Configure(EntityTypeBuilder<ClassTeacherEnrollment> builder)
    {
        builder.ToTable("ClassTeacherEnrollments");

        builder.HasKey(cte => cte.Id);

        builder.Property(cte => cte.Id)
            .ValueGeneratedOnAdd();

        builder.Property(cte => cte.IsMainTeacher)
            .HasDefaultValue(false);

        builder.HasOne(cte => cte.Teacher)
            .WithMany(t => t.ClassTeacherEnrollments)
            .HasForeignKey(cte => cte.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cte => cte.Halaqa)
            .WithMany(h => h.ClassTeacherEnrollments)
            .HasForeignKey(cte => cte.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cte => new { cte.TeacherId, cte.ClassId });
    }
}
