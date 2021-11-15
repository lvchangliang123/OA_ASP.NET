using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IApplicationContext context = ContextRegistry.GetContext();

            var dal=context.GetObject("UserInfoDal") as IUserInfoDal;
            dal.Show();

            Console.ReadKey();
        }
    }
}
