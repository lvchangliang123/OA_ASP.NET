using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Models
{
    [Table("Blog", Schema = "VueNetBlogDB_Test")]
    [Index("UserId", Name = "FK_Blog_UserId")]
    public partial class Blog
    {
        public Blog()
        {
            Comments = new HashSet<Comment>();
            SourceCodes = new HashSet<SourceCode>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Title { get; set; }
        [StringLength(255)]
        public string? OverView { get; set; }
        [StringLength(int.MaxValue)]
        public string? Content { get; set; }
        [StringLength(255)]
        public string? Tags { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateTime { get; set; }
        public int? ViewCount { get; set; }
        [Column(TypeName = "blob")]
        public byte[]? Cover { get; set; }
        public int? UserId { get; set; }

        [StringLength(255)]
        public string? CoverPath { get; set; }

        [StringLength(255)]
        public string? CodePath { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Blogs")]
        public virtual User? User { get; set; }
        [InverseProperty("Blog")]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty("Blog")]
        public virtual ICollection<SourceCode> SourceCodes { get; set; }
    }
}
