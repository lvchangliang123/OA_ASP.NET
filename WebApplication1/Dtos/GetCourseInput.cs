namespace WebApplication1.Dtos
{
    public class GetCourseInput:PagedSortedAndFilterInput
    {
        public GetCourseInput()
        {
            Sorting = "CourseID";
            MaxResultCount = 2;
        }
    }
}
