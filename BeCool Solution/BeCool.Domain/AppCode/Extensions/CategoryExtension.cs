using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.AppCode.Extensions
{
    public static partial class Extension
    {
        static public IEnumerable<Category> GetAllChilds(this Category parent)
        {
            if (parent.ParentId != null)
                yield return parent;

            foreach (var item in parent.Children.SelectMany(c => c.GetAllChilds()))
            {
                yield return item;
            }
        }
    }
}
