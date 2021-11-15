
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


	public partial class ActionInfoService:BaseService <ActionInfo>,IActionInfoService
	{
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ActionInfoDal;
		}
	}
	
	public partial class OrderInfoService:BaseService <OrderInfo>,IOrderInfoService
	{
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.OrderInfoDal;
		}
	}
	
	public partial class RoleInfoService:BaseService <RoleInfo>,IRoleInfoService
	{
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.RoleInfoDal;
		}
	}
	
	public partial class UserInfoService:BaseService <UserInfo>,IUserInfoService
	{
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.UserInfoDal;
		}
	}
	


}