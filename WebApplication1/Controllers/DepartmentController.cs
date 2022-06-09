using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.DataRepositories;
using WebApplication1.Dtos;
using WebApplication1.Infrastructure;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;
using WebApplication1.Services.DepartmentService;
using WebApplication1.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IRepository<Department,int> _departmentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IDepartmentService _departmentService;
        private readonly AppDbContext _dbContext;   //???

        public DepartmentController(IRepository<Department, int> departmentRepository, ITeacherRepository teacherRepository, IDepartmentService departmentService, AppDbContext dbContext)
        {
            _departmentRepository = departmentRepository;
            _teacherRepository = teacherRepository;
            _departmentService = departmentService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index(GetDepartmentInput input)
        {
            var models = await _departmentService.GetPagedDepartmentsList(input);
            return View(models);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            var dto = new DepartmentCreateViewModel {
                TeacherList = TeachersDropDownList(),
            };
            return View(dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(DepartmentCreateViewModel model)
        {
            if (ModelState.IsValid) {
                Department department = new Department {
                    StartDate = model.StartDate,
                    DepartmentID = model.DepartmentID,
                    TeacherID = model.TeacherID,
                    Budget = model.Budget,
                    Name = model.Name,
                };
                await _departmentRepository.InsertAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _departmentRepository.GetAll().Include(a => a.Administrator).FirstOrDefaultAsync(a => a.DepartmentID == id);
            if (model==null) {
                ViewBag.ErrorMessage = $"部门ID{id}的信息不存在，请重试!";
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _departmentRepository.FirstOrDefaultAsync(a => a.DepartmentID == id);
            if (model==null) {
                ViewBag.ErrorMessage = $"部门ID{id}的信息不存在，请重试!";
                return View("NotFound");
            }
            await _departmentRepository.DeleteAsync(a => a.DepartmentID == id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _departmentRepository.GetAll().Include(a => a.Administrator).AsNoTracking().FirstOrDefaultAsync(a => a.DepartmentID == id);
            if (model==null) {
                ViewBag.ErrorMessage = $"部门ID{id}的信息不存在，请重试!";
                return View("NotFound");
            }
            var teacherList = TeachersDropDownList();
            var dto = new DepartmentCreateViewModel {
                DepartmentID = model.DepartmentID,
                Name = model.Name,
                Budget = model.Budget,
                StartDate = model.StartDate,
                TeacherID = model.TeacherID,
                Administrator = model.Administrator,
                RowVersion = model.RowVersion,
                TeacherList = teacherList,
            };
            return View(dto);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(DepartmentCreateViewModel input)
        {
            if (ModelState.IsValid) {
                var model = await _departmentRepository.GetAll().Include(a => a.Administrator).FirstOrDefaultAsync(a => a.DepartmentID == input.DepartmentID);
                if (model==null) {
                    ViewBag.ErrorMessage = $"部门ID{input.DepartmentID}的信息不存在，请重试!";
                    return View("NotFound");
                }
                model.DepartmentID=input.DepartmentID;
                model.Name=input.Name;
                model.Budget=input.Budget;
                model.StartDate=input.StartDate;
                model.TeacherID=input.TeacherID;

                _dbContext.Entry(model).Property("RowVersion").OriginalValue = input.RowVersion;

                try {
                    await _departmentRepository.UpdateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex) {

                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = exceptionEntry.Entity as Department;
                    var dataBaseEntry = exceptionEntry.GetDatabaseValues();
                    if (dataBaseEntry==null) {
                        ModelState.AddModelError(string.Empty, "无法进行数据的修改，该部分信息已经被他人删除!");
                    }
                    else {
                        var dataBaseValues = dataBaseEntry.ToObject() as Department;
                        if (dataBaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", $"当前值:{dataBaseValues.Name}");
                        if (dataBaseValues.Budget != clientValues.Budget)
                            ModelState.AddModelError("Budget", $"当前值:{dataBaseValues.Budget}");
                        if (dataBaseValues.StartDate != clientValues.StartDate)
                            ModelState.AddModelError("StartDate", $"当前值:{dataBaseValues.StartDate}");
                        if (dataBaseValues.TeacherID != clientValues.TeacherID) {
                            var teacherEntity =
                                 await _teacherRepository.FirstOrDefaultAsync(a => a.Id == dataBaseValues.TeacherID);
                            ModelState.AddModelError("TeacherId", $"当前值:{teacherEntity?.Name}");
                        }
                        ModelState.AddModelError("", "你正在编辑的记录已经被其他用户所修改，编辑操作已经被取消，数据库当前的值已经显示在页面上。请再次点击保存。否则请返回列表。");
                        input.RowVersion = dataBaseValues.RowVersion;
                        //记得初始化老师列表
                        input.TeacherList = TeachersDropDownList();
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            return View(input);
        }


        private SelectList TeachersDropDownList(object selectedTeacher=null)
        {
            var models = _teacherRepository.GetAll().OrderBy(a => a.Name).AsNoTracking().ToList();
            var dtos = new SelectList(models, "Id","Name", selectedTeacher);
            return dtos;
        }
    }
}
