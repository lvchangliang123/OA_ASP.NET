namespace WebApplication1.Dtos
{
    public class GetDepartmentInput:PagedSortedAndFilterInput
    {
        public GetDepartmentInput()
        {
            Sorting = "Name";
            MaxResultCount = 2;
        }
    }
}
