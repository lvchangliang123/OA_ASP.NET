﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class StudentEditViewModel:StudentCreateViewModel
    {
        //public string Id { get; set; }
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }

    }
}
