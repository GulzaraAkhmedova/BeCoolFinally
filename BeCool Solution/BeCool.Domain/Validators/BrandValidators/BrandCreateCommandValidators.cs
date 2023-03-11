using BeCool.Domain.Business.BrandModule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Validators.BrandValidators
{
    public class BrandCreateCommandValidator : AbstractValidator<BrandCreateCommand>
    {
        public BrandCreateCommandValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("Marka adi bosh buraxila bilmez")

                .MinimumLength(2)
                .WithMessage("Marka adi 2simvoldan az ola bilmez");
        }
    }
}
