using BeCool.Application.Infrastructure;
using System.Collections.Generic;

namespace BeCool.Domain.Models.Entities
{
    public class ProductSize : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductCatalogItem> ProductCatalog { get; set; }

    }
}
