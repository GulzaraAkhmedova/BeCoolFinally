using Microsoft.EntityFrameworkCore;
using BeCool.Domain.Models.Entities.Membership;

namespace BeCool.Domain.Models.DataContexts
{
    public partial class BeCoolDbContext {
        
        public DbSet<BeCoolRole> Roles { get; set; }
        public DbSet<BeCoolRoleClaim> RoleClaims { get; set; }
        public DbSet<BeCoolUser> Users{ get; set; }
        public DbSet<BeCoolUserClaim> UserClaims{ get; set; }
        public DbSet<BeCoolUserLogin> UserLogins{ get; set; }
        public DbSet<BeCoolUserRole> UserRoles{ get; set; }
        public DbSet<BeCoolUserToken> UserTokens{ get; set; }
        public object Baskets { get; internal set; }
    }
}
