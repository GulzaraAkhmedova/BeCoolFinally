using BeCool.Application.Infrastructure;
using BeCool.Domain.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Models.Entities
{
    public class BlogPostComment : BaseEntity
    {
        public string Text { get; set; }
        public int BlogPostId { get; set; }
        public virtual BlogPost BlogPost { get; set; }
        public int? ParentId { get; set; }
        public virtual BlogPostComment Parent { get; set; }
        public virtual ICollection<BlogPostComment> Comments { get; set; }
        public bool Approved { get; set; }
        public int? CreatedByUserId { get; set; }
        public virtual BeCoolUser CreatedByUser { get; set; }

    }
}
