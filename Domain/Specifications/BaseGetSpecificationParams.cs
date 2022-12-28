using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specifications
{
    public abstract class BaseGetSpecificationParams
    {
        private const int MaxPageSize = 50;

        [Range(1, 1000000,ErrorMessage = "PageIndex must be between 1 to 1,000,000")]
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;

        [Range(1, 50, ErrorMessage = "PageSize must be between 1 to 50")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        [RegularExpression(@"^[\u0061-\u007a\u0041-\u005a\u0030-\u0039\u0600-\u06ff\u0750-\u077f\ufb50-\ufdff\u08a0\u2014\u08ff\ufe70\u002d\ufefc\u200b\u200c\s]{0,10}$", ErrorMessage = "Invalid Sort. Name must be less than 10 character.")]
        public string? Sort { get; set; }

        private string? _search;

        [RegularExpression(@"^[\u0061-\u007a\u0041-\u005a\u0030-\u0039\u0600-\u06ff\u0750-\u077f\ufb50-\ufdff\u08a0\u2014\u08ff\ufe70\u002d\ufefc\u200b\u200c\s]{0,50}$", ErrorMessage = "Invalid Search. Name must be less than 50 character.")]
        public string? Search
        {
            get => _search;
            set => _search = value?.ToLower();
        }
    }
}
