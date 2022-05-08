using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DataRepositories;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Students;

namespace WebApplication1.Controllers
{
    [Route("Student")]
    public class StudentController : Controller
    {
        private readonly IRepository<Student,int> _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDataProtector _protector;
        private readonly IStudentService _studentService;

        public StudentController(IRepository<Student, int> studentRepository,IWebHostEnvironment webHostEnvironment,IDataProtectionProvider dataProtectionProvider,Extensions.DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this._studentRepository = studentRepository;
            this._webHostEnvironment = webHostEnvironment;
            this._protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.StudentIdRouteValue);
        }

        [Route("Details/{id?}")]
        //属性路由支持分层
        //public ViewResult Details(int? id)
        public ViewResult Details(string id)
        {
            #region Old Code
            //string decryptedId = _protector.Unprotect(id);
            //int decryptedStudentId=Convert.ToInt32(decryptedId);

            //var student = _studentRepository.GetStudentById(decryptedStudentId);
            //if (student==null) {
            //    Response.StatusCode = 404;
            //    return View("StudentNotFound", id);
            //}
            //StudentDetailsViewModel studentDetailsViewModel = new StudentDetailsViewModel
            //{
            //    Student = student,
            //    PageTitle = "学生详细信息"
            //};
            //return View(studentDetailsViewModel);
            #endregion

            var student = DecryptedStudent(id);
            if (student==null) {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试!";
                return View("NotFound");

            }
            StudentDetailsViewModel studentDetailsViewModel = new StudentDetailsViewModel {
                Student = student,
                PageTitle = "学生详细信息"
            };
            studentDetailsViewModel.Student.EncryptedId = _protector.Protect(student.Id.ToString());
            return View(studentDetailsViewModel);
        }

        private Student DecryptedStudent(string id)
        {
            string decryptedId = _protector.Unprotect(id);
            int decryptedStudentId = Convert.ToInt32(decryptedId);
            Student student = _studentRepository.FirstOrDefault(x => x.Id == decryptedStudentId);
            return student;
        }

        [Route("Index")]
        [Route("")]
        [Route("/")]
        [HttpGet]
        public async Task<ViewResult> Index(int?pageNumber,int pageSize =10,string sortBy="Id")
        {
            #region old Code
            //var students = _studentRepository.GetAllList().Select(s => { s.EncryptedId = _protector.Protect(s.Id.ToString());
            //    return s;
            //}).ToList();    //加密学生ID
            //return View(students);
            #endregion

            //IQueryable<Student> query = _studentRepository.GetAll().OrderBy(sortBy).AsNoTracking();
            //var model = query.ToList().Select(s => { s.EncryptedId = _protector.Protect(s.Id.ToString()); return s; }).ToList();
            //return View(model);
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.Count = await _studentRepository.CountAsync();
            paginationModel.CurrentPage = pageNumber??1;
            var students = await _studentRepository.GetAllListAsync();
            paginationModel.Data = students.Select(s => { s.EncryptedId = _protector.Protect(s.Id.ToString()); return s; }).ToList();
            return View(paginationModel);
        }

        [Route("Index")]
        [HttpPost]
        public async Task<IActionResult> Index(string searchString,string sortBy="Id")
        {
            ViewBag.CurrentFilter=searchString?.Trim();
            IQueryable<Student> query = _studentRepository.GetAll();
            if (!String.IsNullOrEmpty(searchString)) {
                query= query.Where(s => s.Name.Contains(searchString)||s.Email.Contains(searchString));
            }
            query = query.OrderBy(sortBy).AsNoTracking();
            var model=query.ToList().Select(s => { s.EncryptedId = _protector.Protect(s.Id.ToString());return s;}).ToList();
            return View(model);
        }

        //分页操作
        public async Task<IActionResult> Index(string searchString, int currentPage, string sortBy = "Id")
        {
            ViewBag.CurrentFilter = searchString?.Trim();
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.Count = await _studentRepository.CountAsync();
            paginationModel.CurrentPage = currentPage;
            var students = await _studentService.GetPaginatedResult(paginationModel.CurrentPage, searchString, sortBy);
            paginationModel.Data = students.Select(s => { s.EncryptedId = _protector.Protect(s.Id.ToString()); return s; }).ToList();
            IQueryable<Student> query = _studentRepository.GetAll();
            if (!string.IsNullOrEmpty(searchString)) {
                query = query.Where(s => s.Name.Contains(searchString) || s.Email.Contains(searchString));
            }
            query = query.OrderBy(sortBy).AsNoTracking();
            var model = query.ToList().Select(s => { s.EncryptedId = _protector.Protect(s.Id.ToString()); return s; }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            #region Old Code
            //if (ModelState.IsValid) 
            //{

            //    string uniqueFileName = null;
            //    if (model.Photos != null&&model.Photos.Count>0) {
            //        foreach (IFormFile Photo in model.Photos) {
            //            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages");
            //            uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
            //            string filePath = Path.Combine(uploadFolder, uniqueFileName);
            //            Photo.CopyTo(new FileStream(filePath, FileMode.OpenOrCreate));
            //        }
            //        Student newStudent = new Student {
            //            Name = model.Name,
            //            Email = model.Email,
            //            Major = model.Major,
            //            PhotoPath = uniqueFileName
            //        };
            //        _studentRepository.Insert(newStudent);
            //        return RedirectToAction("Details", new { id = newStudent.Id });
            //    }

            //}
            //return View();
            #endregion

            if (ModelState.IsValid) {
                var uniqueFileName = ProcessUploadedFile(model);
                Student newStudent = new Student {
                    Name = model.Name,
                    Email = model.Email,
                    Major = model.Major,
                    EnrollmentDate = model.EnrollmentDate,
                    PhotoPath = uniqueFileName
                };
                _studentRepository.Insert(newStudent);
                var encryptedId = _protector.Protect(newStudent.Id.ToString());
                return RedirectToAction("Details", new { id = encryptedId });
            }
            return View();

        }


        //[Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Edit/{id}")]
        [HttpGet]
        public ViewResult Edit(string id)
        {
            #region Old Code
            //var editStudent = _studentRepository.GetStudentById(id);
            //if (editStudent==null) {
            //    Response.StatusCode = 404;
            //    return View("StudentNotFound", id);
            //}
            //var studentViewModel = new StudentEditViewModel() {
            //    Id = editStudent.Id,
            //    Name = editStudent.Name,
            //    Email = editStudent.Email,
            //    Major = editStudent.Major,
            //    ExistingPhotoPath = editStudent.PhotoPath
            //};
            //return View(studentViewModel);
            #endregion

            var student = DecryptedStudent(id);
            if (student == null) {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试";
                return View("NotFound");
            }
            var studentEditViewModel = new StudentEditViewModel {
                Id = student.Id,
                Name = student.Name,
                Major = student.Major,
                ExistingPhotoPath = student.PhotoPath,
                EnrollmentDate = student.EnrollmentDate,
            };
            return View(studentEditViewModel);
        }


        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            #region Old Code
            ////检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            ////这样用户就可以更正并重新提交编辑表单
            //if (ModelState.IsValid) {
            //    var student = _studentRepository.GetStudentById(model.Id);

            //    //用模型对象中的数据更新student对象
            //    student.Name = model.Name;
            //    student.Email = model.Email;
            //    student.Major = model.Major;

            //    if (model.Photos != null && model.Photos.Count > 0) {
            //        //如果上传了新的照片，则必须显示新的照片信息
            //        //因此我们会检查当前学生信息中是否有照片，有的话，就会删除它。
            //        if (model.ExistingPhotoPath != null) {
            //            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages",  model.ExistingPhotoPath);

            //            if (System.IO.File.Exists(filePath)) {
            //                System.IO.File.Delete(filePath);
            //            }
            //        }
            //        student.PhotoPath = ProcessUploadedFile(model);
            //    }
            //    Student updatedstudent = _studentRepository.Update(student);

            //    return RedirectToAction("Index");
            //}

            //return View(model);
            #endregion

            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid) {
                var student = DecryptedStudent(model.Id.ToString());

                //用模型对象中的数据更新student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;
                student.EnrollmentDate=model.EnrollmentDate;

                if (model.Photos != null && model.Photos.Count > 0) {
                    if (model.ExistingPhotoPath != null) {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages", model.ExistingPhotoPath);

                        if (System.IO.File.Exists(filePath)) {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    student.PhotoPath = ProcessUploadedFile(model);
                }
                Student updatedstudent = _studentRepository.Update(student);
                return RedirectToAction("Index");
            }

            return View(model);

        }

        //删除功能
        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var student =await _studentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null) {
                ViewBag.ErrorMessage = $"无法找到ID为{id}的学生信息";
                return View("NotFound");
            }
            await _studentRepository.DeleteAsync(student);
            return RedirectToAction("Index");
        }


        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photos != null && model.Photos.Count > 0) {
                foreach (var photo in model.Photos) {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        photo.CopyTo(fileStream);
                    }
                }
            }
            return uniqueFileName;
        }
    }
}
