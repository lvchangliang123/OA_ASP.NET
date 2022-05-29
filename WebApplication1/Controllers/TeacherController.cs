using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services.TeachersService;
using WebApplication1.ViewModels;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WebApplication1.DataRepositories;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ViewModels;
using WebApplication1.Models;
using System;
using System.Collections.Generic;
using WebApplication1.Infrastructure.Repositories;
namespace WebApplication1.Controllers
{
    [Route("Teacher")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IRepository<OfficeLocation, int> _officeLocationRepository;

        public TeacherController(ITeacherService teacherService, ITeacherRepository teacherRepository, ICourseRepository courseRepository)
        {
            this._teacherService = teacherService;
            this._teacherRepository = teacherRepository;
            this._courseRepository = courseRepository;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index(GetTeacherInput input)
        {
            var models = await _teacherService.GetPagedTeacherList(input);
            var dto = new TeacherListViewModel();

            if (input.Id!=null) {
                var teacher = models.Data.FirstOrDefault(t => t.Id == input.Id.Value);
                if (teacher!=null) {
                    dto.Courses = teacher.CourseAssignments.Select(a => a.Course).ToList();
                }
                dto.SelectedId=input.Id.Value;
            }
            if (input.CourseId.HasValue) {
                var course = dto.Courses.FirstOrDefault(c => c.CourseID == input.CourseId.Value);
                if (course!=null) {
                    dto.StudentCourses = course.StudentCourses.ToList();
                }
                dto.SelectedCourseId = input.CourseId.Value;
            }
            dto.Teachers = models;

            return View(dto);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            var model = await _teacherRepository.GetAll().Include(t => t.OfficeLocation).Include(t => t.CourseAssignments).ThenInclude(ca => ca.Course).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (model==null) {
                ViewBag.ErrorMessage = $"教师ID为{id}的信息不存在，请重试!";
                return View("NotFound");
            }
            var dto = new TeacherCreateViewModel {
                Name = model.Name,
                Id = model.Id,
                HireDate = model.HireDate,
                OfficeLocation = model.OfficeLocation,
            };
            var assignedCourses = AssignedCourseDropDownList(model);
            dto.AssignedCourses = assignedCourses;
            return View(dto);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(TeacherCreateViewModel model)
        {
            if (ModelState.IsValid) {
                var teacher = await _teacherRepository.GetAll().Include(t => t.OfficeLocation).Include(t => t.CourseAssignments).ThenInclude(ca => ca.Course).FirstOrDefaultAsync(t => t.Id == model.Id);
                if (teacher==null) {
                    ViewBag.ErrorMessage = $"教师ID为{model.Id}的信息不存在，请重试!";
                    return View("NotFound");
                }
                //属性赋值
                teacher.HireDate = model.HireDate;
                teacher.Name=model.Name;
                teacher.OfficeLocation=model.OfficeLocation;
                teacher.CourseAssignments=new List<CourseAssignment>();
                var courses = model.AssignedCourses.Where(a => a.IsSelected == true).ToList();
                foreach (var item in courses) {
                    teacher.CourseAssignments.Add(new CourseAssignment { CourseID = item.CourseID, TeacherID = teacher.Id });
                }
                await _teacherRepository.UpdateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            var allCourses = _courseRepository.GetAllCourses();
            var viewModel = new List<AssignedCourseViewModel>();
            foreach (var course in allCourses) {
                viewModel.Add(new AssignedCourseViewModel {
                    CourseID=course.CourseID,
                    Title=course.Title,
                    IsSelected=false,
                });
            }
            var dto = new TeacherCreateViewModel();
            dto.AssignedCourses = viewModel;
            return View(dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
        {
            if (ModelState.IsValid) {
                var teacher = new Teacher {
                    HireDate = model.HireDate,
                    Name = model.Name,
                    OfficeLocation = model.OfficeLocation,
                    CourseAssignments = new List<CourseAssignment>()
                };
                var courses = model.AssignedCourses.Where(a => a.IsSelected == true).ToList();
                foreach (var item in courses) {
                    teacher.CourseAssignments.Add(new CourseAssignment {
                        CourseID = item.CourseID,
                        TeacherID = model.Id,
                    });
                }
                await _teacherRepository.InsertAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _teacherRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (model==null) {
                ViewBag.ErrorMessage = $"教师ID为{id}的信息不存在，请重试!";
                return View("NotFound");
            }
            await _officeLocationRepository.DeleteAsync(a=>a.TeacherId==id);
            await _teacherRepository.DeleteAsync(a => a.Id == id);
            return RedirectToAction(nameof(Index));
        }

        private List<AssignedCourseViewModel> AssignedCourseDropDownList(Teacher teacher)
        {
            var allCourses=_courseRepository.GetAllCourses();
            var teacherCourses = new HashSet<int>(teacher.CourseAssignments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseViewModel>();
            foreach (var course in allCourses) {
                viewModel.Add(new AssignedCourseViewModel {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    IsSelected = teacherCourses.Contains(course.CourseID),
                });
            }
            return viewModel;
        }
    }
}
