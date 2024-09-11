using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Models
{
    [Table("Comment", Schema = "VueNetBlogDB_Test")]
    [Index("BlogId", Name = "FK_Comment_BlogId")]
    [Index("UserId", Name = "FK_Comment_UserId")]
    public partial class Comment
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Content { get; set; }
        public int? UserId { get; set; }
        public int? BlogId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateTime { get; set; }

        [ForeignKey("BlogId")]
        [InverseProperty("Comments")]
        public virtual Blog? Blog { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Comments")]
        public virtual User? User { get; set; }
    }
}
