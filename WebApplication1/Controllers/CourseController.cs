using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Courses;
using WebApplication1.DataRepositories;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("Course")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseAssignmentsRepository _courseAssignmentsRepository;
        private readonly ICourseService _courseService;
        private readonly IDepartmentRepository _departmentRepository;

        public CourseController(ICourseRepository courseRepository, ICourseService courseService, IDepartmentRepository departmentRepository)
        {
            this._courseRepository = courseRepository;
            this._courseService = courseService;
            this._departmentRepository = departmentRepository;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index(GetCourseInput input)
        {
            var models = await _courseService.GetPaginatedResult(input);
            return View(models);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            var dto = DepartmentsDropDownList();
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel {
                DepartmentList = dto,
            };
            return View(courseCreateViewModel);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            if (ModelState.IsValid) {
                Course course = new Course {
                    CourseID = model.CourseID,
                    Title = model.Title,
                    Credits = model.Credits,
                    DepartmentID = model.DepartmentID,
                };
                await _courseRepository.InsertAsync(course);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int? courseId)
        {
            if (!courseId.HasValue) {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试!";
                return View("NotFound");
            }
            var course = _courseRepository.GetAllCourses().FirstOrDefault(a => a.CourseID == courseId);
            if (course==null) {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试!";
                return View("NotFound");
            }
            var dto = DepartmentsDropDownList(course.DepartmentID);
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel {
                DepartmentList = dto,
                CourseID = course.CourseID,
                Credits = course.Credits,
                Title = course.Title,
                DepartmentID = course.DepartmentID,
            };
            return View(courseCreateViewModel);
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(CourseCreateViewModel model)
        {
            if (ModelState.IsValid) {
                var course = await _courseRepository.GetCourseById(model.CourseID);
                if (course!=null) {
                    course.CourseID = model.CourseID;
                    course.Credits = model.Credits;
                    course.Title = model.Title;
                    course.DepartmentID = model.DepartmentID;
                    _courseRepository.Update(course);
                    return RedirectToAction(nameof(Index));
                }
                else {
                    ViewBag.ErrorMessage = $"课程编号{model.CourseID}的信息不存在，请重试!";
                    return View("NotFound");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("Details")]
        public async Task<ViewResult> Details(int courseId)
        {
            var course = await _courseRepository.GetAllCourses().Include(a => a.Department).FirstOrDefaultAsync(a => a.CourseID == courseId);
            if (course==null) {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试!";
                return View("NotFound");
            }
            return View(course);
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var model = await _courseRepository.GetCourseById(courseId);
            if (model==null) {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试!";
                return View("NotFound");
            }
            await _courseAssignmentsRepository.DeleteAsync(a => a.CourseID == model.CourseID);
            await _courseRepository.DeleteAsync(a => a.CourseID == courseId);
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// 创建学院下拉框列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private SelectList DepartmentsDropDownList(object selectedDepartment=null)
        {
            var models = _departmentRepository.GetAllDepartments().OrderBy(dp => dp.Name).AsNoTracking().ToList();
            var dtos = new SelectList(models, "DepartmentID", "Name", selectedDepartment);
            return dtos;
        }
    }
}
