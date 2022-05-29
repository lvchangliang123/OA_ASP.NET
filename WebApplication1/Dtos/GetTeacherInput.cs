namespace WebApplication1.Dtos
{
    public class GetTeacherInput:PagedSortedAndFilterInput
    {
        public int? Id { get; set; }
        public int? CourseId { get; set; }
        public GetTeacherInput()
        {
            Sorting = "Id";
            MaxResultCount = 2;
        }
    }
}
