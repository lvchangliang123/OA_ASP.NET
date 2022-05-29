using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos
{
    public class PagedSortedAndFilterInput
    {
        public PagedSortedAndFilterInput()
        {
            CurrentPage = 1;
            MaxResultCount = 2;
        }
        /// <summary>
        /// 每页分页条数
        /// </summary>
        [Range(0,1000)]
        public int MaxResultCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        [Range(0,1000)]
        public int CurrentPage { get; set; }
        /// <summary>
        /// 排序字段ID
        /// </summary>
        public string Sorting { get; set; }
        /// <summary>
        /// 查询名称
        /// </summary>
        public string FilterText { get; set; }


    }
}
