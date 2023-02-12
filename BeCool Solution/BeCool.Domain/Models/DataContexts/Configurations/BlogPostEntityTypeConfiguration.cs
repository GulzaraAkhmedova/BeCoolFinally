using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeCool.Domain.Models.Entities;

namespace BeCool.Domain.Models.DataContexts.Configurations
{
    public class BlogPostEntityTypeConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title)
                .IsRequired();
            builder.Property(p => p.Body)
                .IsRequired();
            builder.Property(p => p.ImagePath)
                .IsRequired();

            builder.Property(p => p.Slug)
                .IsUnicode(false)
                .HasMaxLength(900)
                .IsRequired();

            builder.HasIndex(p => p.Slug)
                .IsUnique();
        }
    }
}
