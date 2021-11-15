
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

    public partial class StaticDalFactory
    {

	public static IActionInfoDal GetActionInfoDal()
	{
		return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".ActionInfoDal") as IActionInfoDal;
	}
		public static IOrderInfoDal GetOrderInfoDal()
	{
		return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".OrderInfoDal") as IOrderInfoDal;
	}
		public static IRoleInfoDal GetRoleInfoDal()
	{
		return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".RoleInfoDal") as IRoleInfoDal;
	}
		public static IUserInfoDal GetUserInfoDal()
	{
		return Assembly.Load(assemblyName).CreateInstance(assemblyName + ".UserInfoDal") as IUserInfoDal;
	}
	
	}

}