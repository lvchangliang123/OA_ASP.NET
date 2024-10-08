﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="姓名")]
        [StringLength(50)]
        [Column("TeacherName")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name="聘用时间")]
        public DateTime HireDate { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        public OfficeLocation OfficeLocation { get; set; }

    }
}
