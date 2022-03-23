using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DataRepositories;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IStudentRepository studentRepository,IWebHostEnvironment webHostEnvironment)
        {
            this._studentRepository = studentRepository;
            this._webHostEnvironment = webHostEnvironment;
        }

        [Route("Details/{id?}")]
        //属性路由支持分层
        public ViewResult Details(int? id)
        {
            var student = _studentRepository.GetStudentById(id ?? 1);
            if (student==null) {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }
            StudentDetailsViewModel studentDetailsViewModel = new StudentDetailsViewModel
            {
                Student = student,
                PageTitle = "学生详细信息"
            };
            return View(studentDetailsViewModel);
        }

        [Route("Index")]
        [Route("")]
        [Route("/")]
        public ViewResult Index()
        {
            var students = _studentRepository.GetAllStudents();
            return View(students);
        }


        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid) 
            {
                //var newStudent = _studentRepository.Insert(student);
                //return RedirectToAction("Details", new { id = newStudent.Id });

                string uniqueFileName = null;
                //if (model.Photo!=null) {
                //    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages");
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                //    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                //    model.Photo.CopyTo(new FileStream(filePath, FileMode.OpenOrCreate));
                //}
                if (model.Photos != null&&model.Photos.Count>0) {
                    foreach (IFormFile Photo in model.Photos) {
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);
                        Photo.CopyTo(new FileStream(filePath, FileMode.OpenOrCreate));
                    }
                    Student newStudent = new Student {
                        Name = model.Name,
                        Email = model.Email,
                        Major = model.Major,
                        PhotoPath = uniqueFileName
                    };
                    _studentRepository.Insert(newStudent);
                    return RedirectToAction("Details", new { id = newStudent.Id });
                }
               
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
        public ViewResult Edit(int id)
        {
            var editStudent = _studentRepository.GetStudentById(id);
            if (editStudent==null) {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }
            var studentViewModel = new StudentEditViewModel() {
                Id = editStudent.Id,
                Name = editStudent.Name,
                Email = editStudent.Email,
                Major = editStudent.Major,
                ExistingPhotoPath = editStudent.PhotoPath
            };
            return View(studentViewModel);
        }


        [Route("Edit/{id}")]
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid) {
                var student = _studentRepository.GetStudentById(model.Id);

                //用模型对象中的数据更新student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.Major = model.Major;

                if (model.Photos != null && model.Photos.Count > 0) {
                    //如果上传了新的照片，则必须显示新的照片信息
                    //因此我们会检查当前学生信息中是否有照片，有的话，就会删除它。
                    if (model.ExistingPhotoPath != null) {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImages",  model.ExistingPhotoPath);

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

        private string ProcessUploadedFile(StudentEditViewModel model)
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
