using AlAshmar.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlAshmar.Infrastructure.Persistence.Configurations;

public class AllowableExtensionConfiguration : IEntityTypeConfiguration<AllowableExtension>
{
    public void Configure(EntityTypeBuilder<AllowableExtension> builder)
    {
        builder.ToTable("AllowableExtensions");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.ExtName)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(a => a.ExtName)
            .IsUnique();
    }
}

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Path)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.SafeName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.OriginalName)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(a => a.Extension)
            .WithMany()
            .HasForeignKey(a => a.ExtensionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.SafeName);
    }
}

public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder.ToTable("ContactInfos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(255);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);
    }
}
