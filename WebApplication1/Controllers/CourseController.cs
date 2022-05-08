using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataRepositories;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
