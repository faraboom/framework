using Faraboom.Framework.DataAnnotation;

namespace Faraboom.Framework.Data
{
    public class PagingDto
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public SortFilterDto SortFilter { get; set; }

        public SearchFilterDto SearchFilter { get; set; }

        public bool Export { get; set; }

        public class SearchFilterDto
        {
            [Required]
            public string Phrase { get; set; }

            [Required]
            public string Column { get; set; }
        }

        public class SortFilterDto
        {
            public bool Descending { get; set; }

            [Required]
            public string Column { get; set; }
        }
    }
}
