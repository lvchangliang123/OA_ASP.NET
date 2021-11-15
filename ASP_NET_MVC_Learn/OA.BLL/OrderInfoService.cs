using OA.IBLL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public partial class OrderInfoService : BaseService<OrderInfo>,IOrderInfoService
    {
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = DbSession.OrderInfoDal;
        //}
    }
}
