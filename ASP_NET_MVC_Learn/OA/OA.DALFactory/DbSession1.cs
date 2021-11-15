
using OA.IDAL;
using OA.EFDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.DALFactory
{
	public partial class DbSession : IDbSession
	{

	public IActionInfoDal ActionInfoDal
	{
		get{ return StaticDalFactory.GetActionInfoDal();}
	}
		public IOrderInfoDal OrderInfoDal
	{
		get{ return StaticDalFactory.GetOrderInfoDal();}
	}
		public IRoleInfoDal RoleInfoDal
	{
		get{ return StaticDalFactory.GetRoleInfoDal();}
	}
		public IUserInfoDal UserInfoDal
	{
		get{ return StaticDalFactory.GetUserInfoDal();}
	}
	
	}

}