using OA.IDAL;
using OA.EFDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.DALFactory
{
    //拥有所有Dal实例的类
    public partial class DbSession : IDbSession
    {
        #region  模板自动生成
        //public IUserInfoDal UserInfoDal
        //{
        //    get { return StaticDalFactory.GetUserInfoDal(); }
        //}

        //public IOrderInfoDal OrderInfoDal
        //{
        //    get { return StaticDalFactory.GetOrderInfoDal(); }
        //}

        //public IRoleInfoDal RoleInfoDal
        //{
        //    get { return StaticDalFactory.GetRoleInfoDal(); }
        //}
        #endregion

        public int SaveChanges()
        {
            return DbContextFactory.GetCurrentDbContext().SaveChanges();

        }

    }

}
