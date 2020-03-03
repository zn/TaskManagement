using ApplicationCore;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    class TaskObjectConfiguration : IEntityTypeConfiguration<TaskObject>
    {
        public void Configure(EntityTypeBuilder<TaskObject> builder)
        {
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Executors)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.CompletionDate)
                .IsRequired();

            builder.Property(t => t.Status)
                .HasDefaultValue(Status.Assigned);

            builder.Property(t => t.RegisterDate)
                .IsRequired();

            builder.Ignore(t => t.LabourIntensity);
            builder.Ignore(t => t.LeadTime);

            builder.HasMany(t => t.Children)
              .WithOne(t => t.Parent)
              .HasForeignKey(t => t.ParentId);
        }
    }
}
