using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Models.DataContexts.Configurations
{
    //public class ContactPostEntityTypeConfiguration : IEntityTypeConfiguration<ContactPost>
    //{
    //    public void Configure(EntityTypeBuilder<ContactPost> builder)
    //    {
    //        builder.HasKey(x => x.Id);

    //        builder.Property(x => x.Id)
    //            .UseIdentityColumn();

    //        builder.Property(x => x.Name)
    //            .IsRequired();

    //        builder.Property(x => x.Email)
    //            .IsRequired();

    //        builder.Property(x => x.Subject)
    //            .IsRequired();

    //        builder.Property(x => x.Message)
    //            .IsRequired();
    //    }
    //}
}
