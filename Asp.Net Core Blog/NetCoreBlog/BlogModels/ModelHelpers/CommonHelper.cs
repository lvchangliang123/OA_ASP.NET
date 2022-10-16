using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.ModelHelpers
{
    public static class CommonHelper
    {
        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(stream);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }
    }
}
