using OA.DALFactory;
using OA.IBLL;
using OA.IDAL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public partial class UserInfoService:BaseService<UserInfo>,IUserInfoService
    {
        //面向接口编程
        //private IUserInfoDal userInfoDal = StaticDalFactory.GetUserInfoDal();
        //DbSession dbSession = new DbSession();
        //IDbSession dbSession = new DbSession();

        //private IDbSession dbSession = DbSessionFactory.GetCurrentDbSession();


        //public userinfo Add(userinfo userinfo)
        //{
        //    dbSession.UserInfoDal.Add(userinfo);

        //    dbSession.SaveChanges();//数据提交的权利从数据库访问层移动至业务逻辑层

        //}

        #region 有模板生成
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = DbSession.UserInfoDal;
        //}
        #endregion
    }
}
