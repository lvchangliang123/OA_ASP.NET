using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Models
{
    [Table("AutherInfo", Schema = "VueNetBlogDB_Test")]
    [Index("UserId", Name = "FK_AutherInfo_UserId")]
    public partial class AutherInfo
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [StringLength(255)]
        public string? WeChatCount { get; set; }
        [StringLength(255)]
        public string? QQCount { get; set; }
        [StringLength(255)]
        public string? WeiboCount { get; set; }
        [StringLength(255)]
        public string? GithubCount { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AutherInfos")]
        public virtual User? User { get; set; }
    }
}
