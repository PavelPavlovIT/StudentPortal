using System.Collections.Generic;

namespace DBRepository.Models
{
    public class Page<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public ICollection<T> Records { get; set; }
    }
}
