using BeCool.Application.Infrastructure;
using System.Collections.Generic;

namespace BeCool.Domain.Models.Entities
{
    public class Category : BaseEntity, IPageable
    {
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
