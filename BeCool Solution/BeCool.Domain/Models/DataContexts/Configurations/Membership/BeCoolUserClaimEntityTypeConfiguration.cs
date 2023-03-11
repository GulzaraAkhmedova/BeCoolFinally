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

    public class BeCoolUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<BeCoolUserClaim>
    {
        public void Configure(EntityTypeBuilder<BeCoolUserClaim> builder)
        {
            builder.ToTable("UserClaims", "Membership");
        }
    }
}
