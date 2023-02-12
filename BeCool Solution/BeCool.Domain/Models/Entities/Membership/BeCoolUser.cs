using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BeCool.Domain.Models.Entities.Membership
{
    public class BeCoolUser : IdentityUser<int>
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}
