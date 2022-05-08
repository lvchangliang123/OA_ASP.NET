using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Dtos
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 1;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count,PageSize));
        public List<Student> Data { get; set; }
        public bool ShowPrevious =>CurrentPage > 1;
        public bool ShowNext =>CurrentPage < TotalPages;
        public bool ShowLast =>CurrentPage != TotalPages;
        public bool ShowFirst => CurrentPage != 1;


    }
}
