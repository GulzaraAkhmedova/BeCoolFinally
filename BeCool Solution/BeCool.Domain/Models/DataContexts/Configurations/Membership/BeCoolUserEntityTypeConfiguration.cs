using BeCool.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Models.DataContexts.Configurations.Membership
{
    public class BeCoolUserEntityTypeConfiguration : IEntityTypeConfiguration<BeCoolUser>
    {
        public void Configure(EntityTypeBuilder<BeCoolUser> builder)
        {
            builder.ToTable("Users", "Membership");
        }
    }
}
