namespace WebApplication1.Dtos
{
    public class GetStudentInput:PagedSortedAndFilterInput
    {
        public GetStudentInput()
        {
            Sorting = "Id";
        }
    }
}
