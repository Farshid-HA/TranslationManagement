using System.Collections.Generic;

namespace Domain.Dtos
{
    public class PaginationDto<T>
    {
        public int Page { get; set; } = 1;
        public int PageCount { get; set; }
        public int ItemsCount { get; set; }
        public List<T> Data { get; set; }
    }
}
