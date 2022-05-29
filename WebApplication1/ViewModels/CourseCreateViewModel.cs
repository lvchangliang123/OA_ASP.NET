using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class CourseCreateViewModel
    {
        [Display(Name ="课程编号")]
        public int CourseID { get; set; }
        [Display(Name ="课程名称")]
        public string Title { get; set; }
        [Display(Name = "课程学分")]
        public int Credits { get; set; }
        public int DepartmentID { get; set; }
        [Display(Name = "学院")]
        public SelectList DepartmentList { get; set; }
    }
}
