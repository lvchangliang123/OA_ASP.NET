using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class DepartmentCreateViewModel
    {
        public int DepartmentID { get; set; }
        [StringLength(50,MinimumLength =3)]
        [Display(Name="学院名称")]
        public string Name { get; set; }
        [Display(Name = "预算")]
        public decimal Budget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "成立时间")]
        public DateTime StartDate { get; set; }

        [Timestamp]
        [Display(Name = "时间戳(并发令牌)")]
        public byte[] RowVersion { get; set; }
        [Display(Name = "学院老师")]
        public SelectList TeacherList { get; set; }

        public int? TeacherID { get; set; }
        [Display(Name = "学院负责人")]
        public Teacher Administrator { get; set; }
    }
}
