using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// 学院信息
    /// </summary>
    public class Department
    {
        public int DepartmentID { get; set; }
        [StringLength(50,MinimumLength =3)]
        [Display(Name ="学院名称")]
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        //[Column(TypeName ="money")]
        [Display(Name = "预算")]
        public decimal Budget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name="成立时间")]
        public DateTime StartDate { get; set; }

        [Timestamp]
        [Display(Name = "时间戳(并发令牌)")]
        public byte[] RowVersion { get; set; }

        public int? TeacherID { get; set; }
        /// <summary>
        /// 学院主任
        /// </summary>
        [Display(Name = "负责人")]
        public Teacher Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
