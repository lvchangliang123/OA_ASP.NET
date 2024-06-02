using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Models
{
    [Table("SourceCode", Schema = "VueNetBlogDB_Test")]
    [Index("BlogId", Name = "FK_SourceCode_BlogId")]
    public partial class SourceCode
    {
        [Key]
        public int Id { get; set; }
        public int? BlogId { get; set; }
        [StringLength(255)]
        public string? Code { get; set; }

        [ForeignKey("BlogId")]
        [InverseProperty("SourceCodes")]
        public virtual Blog? Blog { get; set; }
    }
}
