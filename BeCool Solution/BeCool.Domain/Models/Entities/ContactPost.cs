using BeCool.Application.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace BeCool.Domain.Models.Entities
{
    public class ContactPost : BaseEntity
    {
        [Required(ErrorMessage = "Adınızı daxil etməlisiniz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Emailinizi daxil etməlisiniz")]
        [EmailAddress(ErrorMessage = "Email düzgün deyil (name@example.com)")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mövzunu daxil etməlisiniz")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Mesajınızı daxil etməlisiniz")]
        public string Message { get; set; }
        public string Answer { get; set; }
        public int? AnsweredByUserId { get; set; }
        public DateTime? AnswerDate { get; set; }

    }
}
