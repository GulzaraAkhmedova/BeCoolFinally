using BeCool.Application.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace BeCool.Domain.Models.Entities
{
    public class Faq : BaseEntity
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
