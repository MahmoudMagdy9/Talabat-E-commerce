using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specification.ProductsSpecs
{
    public class ProductSpecParameters
    {

        public string? Sort { get; set; }
        //public string? Search { get; set; }
        public string? Search
        {
            get => _search;
            set => _search = value?.ToLower();
        }
        private string? _search;

        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        public int MaxPageSize { get; set; } = 10;

        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public int PageCount { get; set; } = 0;
        public int PageIndex { get; set; } = 1;


    }
}
