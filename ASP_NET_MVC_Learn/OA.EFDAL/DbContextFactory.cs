using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace OA.EFDAL
{
    public class DbContextFactory
    {
        //返回值类型是DbContext，基类，在返回过程中可以返回任意继承自该基类的上下文实例，并且一次请求返回一个上下文实例
        public static DbContext GetCurrentDbContext()
        {
            //return new oaEntities();
            DbContext db = CallContext.GetData("DbContext") as DbContext;
            if (db==null)
            {
                db = new OAEntities();
                CallContext.SetData("DbContext", db);
            }
            return db;
        }
    }
}
