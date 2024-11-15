﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Models
{
    [Table("User", Schema = "VueNetBlogDB_Test")]
    public partial class User : IdentityUser<int>
    {
        public User()
        {
            AutherInfos = new HashSet<AutherInfo>();
            Blogs = new HashSet<Blog>();
            Comments = new HashSet<Comment>();
            Dailies = new HashSet<Daily>();
        }

        [Key]
        public override int Id { get; set; }
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public override string? Email { get; set; }
        [StringLength(255)]
        public string? Password { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }
        [StringLength(255)]
        public string? AvatarPath { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<AutherInfo> AutherInfos { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Blog> Blogs { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Daily> Dailies { get; set; }
    }
}
