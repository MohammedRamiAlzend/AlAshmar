using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlAshmar.Domain.Entities.Managers;

namespace AlAshmar.Infrastructure.Persistence.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(m => m.ManagerContactInfos)
            .WithOne(mci => mci.Manager)
            .HasForeignKey(mci => mci.ManagerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.ManagerAttachments)
            .WithOne(ma => ma.Manager)
            .HasForeignKey(ma => ma.ManagerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ManagerContactInfoConfiguration : IEntityTypeConfiguration<ManagerContactInfo>
{
    public void Configure(EntityTypeBuilder<ManagerContactInfo> builder)
    {
        builder.ToTable("ManagerContactInfos");

        builder.HasKey(mci => new { mci.ManagerId, mci.ContactInfoId });

        builder.HasOne(mci => mci.Manager)
            .WithMany(m => m.ManagerContactInfos)
            .HasForeignKey(mci => mci.ManagerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mci => mci.ContactInfo)
            .WithMany()
            .HasForeignKey(mci => mci.ContactInfoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ManagerAttachmentConfiguration : IEntityTypeConfiguration<ManagerAttachment>
{
    public void Configure(EntityTypeBuilder<ManagerAttachment> builder)
    {
        builder.ToTable("ManagerAttachments");

        builder.HasKey(ma => new { ma.ManagerId, ma.AttachmentId });

        builder.HasOne(ma => ma.Manager)
            .WithMany(m => m.ManagerAttachments)
            .HasForeignKey(ma => ma.ManagerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ma => ma.Attachment)
            .WithMany()
            .HasForeignKey(ma => ma.AttachmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
