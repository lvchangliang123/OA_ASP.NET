using OA.EFDAL;
using OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OA.DALFactory
{
    /// <summary>
    /// 抽象工厂模式，利用项目配置文件，配置需要的Dal实例
    /// </summary>
    public partial class StaticDalFactory
    {
        public static string assemblyName = System.Configuration.ConfigurationManager.AppSettings["DalAssemblyName"];
        #region  模板自动生成
        //public static IUserInfoDal GetUserInfoDal()
        //{
        //    //return new UserInfoDal();
        //    return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".UserInfoDal") as IUserInfoDal;
        //}

        //internal static IRoleInfoDal GetRoleInfoDal()
        //{
        //    return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".RoleInfoDal") as IRoleInfoDal;
        //}

        //public static IOrderInfoDal GetOrderInfoDal()
        //{
        //    return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".OrderInfoDal") as IOrderInfoDal;
        //}
        #endregion

    }
}
