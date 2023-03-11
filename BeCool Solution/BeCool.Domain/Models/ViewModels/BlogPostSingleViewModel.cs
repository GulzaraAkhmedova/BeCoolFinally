using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Models.ViewModels
{
    public class BlogPostSingleViewModel
    {
        public BlogPost Post { get; set; }
        public List<Category> Categories { get; set; }
        public List<BlogPostComment> Comments { get; set; }
    }
}
