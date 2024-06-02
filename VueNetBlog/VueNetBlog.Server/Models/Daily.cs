using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Models
{
    [Table("Daily", Schema = "VueNetBlogDB_Test")]
    [Index("UserId", Name = "FK_Daily_UserId")]
    public partial class Daily
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [StringLength(255)]
        public string? Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Dailies")]
        public virtual User? User { get; set; }
    }
}
