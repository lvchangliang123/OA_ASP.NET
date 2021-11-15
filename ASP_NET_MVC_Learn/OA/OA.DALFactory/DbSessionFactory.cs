using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using OA.IDAL;

namespace OA.DALFactory
{
    public class DbSessionFactory
    {
        //一次请求只返回一个DbSession实例
        public static IDbSession GetCurrentDbSession()
        {
            IDbSession db = CallContext.GetData("DbSession") as IDbSession;
            if (db == null)
            {
                db = new DbSession();
                CallContext.SetData("DbSession", db);
            }
            return db;
        }
    }
}
