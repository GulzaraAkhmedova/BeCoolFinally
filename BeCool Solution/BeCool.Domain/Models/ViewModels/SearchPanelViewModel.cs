using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeCool.Domain.Models.ViewModels
{
    public class SearchPanelViewModel
    {
        public ProductMaterial[] Materials { get; set; }
       
        public ProductSize[] Sizes { get; set; }
        public ProductType[] Types { get; set; }
        public Brand[] Brands { get; set; }

        public Category[] Categories { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

    }
}
