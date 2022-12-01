using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications
{
    public class CategorySpecificationParams
    {
        private const int MazPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MazPageSize) ? MazPageSize : value;
        }


        public string? Sort { get; set; }

        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.ToLower();
        }
    }
}
