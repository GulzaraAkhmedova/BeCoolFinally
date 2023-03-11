using BeCool.Domain.Business.ProductModule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Validators.ProductValidators
{

    public class ProductEditCommandValidator : AbstractValidator<ProductEditCommand>
    {
        public ProductEditCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("Id seçilməyib");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("{0} boş buraxıla bilməz");

            RuleFor(c => c.StockKeepingUnit)
                .NotEmpty()
                .WithMessage("{0} boş buraxıla bilməz");

            RuleFor(c => c.Price)
                .GreaterThan(0)
                .WithMessage("Price müsbət olmalidir");

            RuleFor(c => c.ShortDescription)
                .NotEmpty()
                .WithMessage("{0} boş buraxıla bilməz")

                .MinimumLength(20)
                .WithMessage("Qeyd en az 20 simvol olmalidir");

            RuleFor(m => m.CategoryId)
               .GreaterThanOrEqualTo(1)
               .WithMessage("Meqale kategoriyasi secilmeyib");

            RuleFor(m => m.BrandId)
               .GreaterThanOrEqualTo(1)
               .WithMessage("Marka secilmeyib");

            RuleFor(m => m.Images)
                .Custom((list, context) =>
                {
                    if (list == null)
                    {
                        context.AddFailure("Şəkil seçilməyib");
                    }
                    else if (list.Count(l => l.IsMain == true) == 0)
                    {
                        context.AddFailure("Əsas şəkil seçilməyib");
                    }
                });

            RuleForEach(m => m.Images)
                .ChildRules(m =>
                {
                    m.RuleFor(i => i.File != null);
                });
        }
    }
}
