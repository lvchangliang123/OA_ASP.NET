using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this System.Enum @enum)
        {
            var type = @enum.GetType();
            var menInfo = type.GetMember(@enum.ToString());
            if (menInfo!=null && menInfo.Length>0) {
                var attrs = menInfo[0].GetCustomAttributes(typeof(DisplayAttribute), true);
                if (attrs!=null&&attrs.Length>0) {
                    return ((DisplayAttribute)attrs[0]).Name;
                }
            }
            return @enum.ToString();
        }
    }
}
