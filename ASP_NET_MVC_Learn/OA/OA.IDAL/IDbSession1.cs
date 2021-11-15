
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.IDAL
{
	public partial interface IDbSession
	{
		IActionInfoDal ActionInfoDal {get; }
			IOrderInfoDal OrderInfoDal {get; }
			IRoleInfoDal RoleInfoDal {get; }
			IUserInfoDal UserInfoDal {get; }
	
	}

}