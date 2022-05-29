using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class OfficeLocation
    {
        //这里Teacher可能为空,使用TeacherId作为外键
        [Key]
        public int TeacherId { get; set; }
        [StringLength(50)]
        [Display(Name ="办公室位置")]
        public string Location { get; set; }
        public Teacher Teacher { get; set; }

    }
}
