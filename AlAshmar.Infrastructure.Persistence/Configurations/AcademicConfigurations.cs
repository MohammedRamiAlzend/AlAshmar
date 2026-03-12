using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}

public class HadithConfiguration : IEntityTypeConfiguration<Hadith>
{
    public void Configure(EntityTypeBuilder<Hadith> builder)
    {
        builder.ToTable("Hadiths");
        
        builder.HasKey(h => h.Id);
        
        builder.Property(h => h.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(h => h.Text)
            .IsRequired();
        
        builder.Property(h => h.Chapter)
            .HasMaxLength(200);
        
        builder.HasOne(h => h.Book)
            .WithMany()
            .HasForeignKey(h => h.BookId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(h => h.BookId);
    }
}

public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.ToTable("Semesters");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(s => s.StartDate)
            .IsRequired();
        
        builder.Property(s => s.EndDate)
            .IsRequired();
        
        builder.HasIndex(s => s.Name)
            .IsUnique();
        
        builder.HasQueryFilter(s => s.StartDate <= s.EndDate);

        builder.HasMany(s => s.Dawras)
            .WithOne(m => m.Semester)
            .HasForeignKey(m => m.SemesterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DawraConfiguration : IEntityTypeConfiguration<Dawra>
{
    public void Configure(EntityTypeBuilder<Dawra> builder)
    {
        builder.ToTable("Dawras");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.EventName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(m => m.Semester)
            .WithMany(s => s.Dawras)
            .HasForeignKey(m => m.SemesterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Halaqas)
            .WithOne(h => h.Dawra)
            .HasForeignKey(h => h.DawraId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.SemesterId);
    }
}

public class HalaqaConfiguration : IEntityTypeConfiguration<Halaqa>
{
    public void Configure(EntityTypeBuilder<Halaqa> builder)
    {
        builder.ToTable("Halaqas");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .ValueGeneratedOnAdd();

        builder.Property(h => h.ClassName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(h => h.Dawra)
            .WithMany(m => m.Halaqas)
            .HasForeignKey(h => h.DawraId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => h.DawraId);
    }
}

public class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.ToTable("Points");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(p => p.PointValue)
            .IsRequired();
        
        builder.Property(p => p.Notes)
            .HasMaxLength(1000);
        
        builder.HasOne(p => p.Student)
            .WithMany(s => s.Points)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(p => p.GivenByTeacher)
            .WithMany(t => t.GivenPoints)
            .HasForeignKey(p => p.GivenByTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Dawra)
            .WithMany()
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Halaqa)
            .WithMany()
            .HasForeignKey(p => p.ClassId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(p => new { p.StudentId, p.SmesterId, p.ClassId });
    }
}

public class PointCategoryConfiguration : IEntityTypeConfiguration<PointCategory>
{
    public void Configure(EntityTypeBuilder<PointCategory> builder)
    {
        builder.ToTable("PointCategories");
        
        builder.HasKey(pc => pc.Id);
        
        builder.Property(pc => pc.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(pc => pc.Type)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(pc => pc.Type)
            .IsUnique();
    }
}
