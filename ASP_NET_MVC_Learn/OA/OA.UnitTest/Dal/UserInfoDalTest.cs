using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA.BLL;
using OA.EFDAL;
using OA.Model;
using System;
using System.Linq;

namespace OA.UnitTest
{
    [TestClass]
    public class UserInfoDalTest
    {
        [TestMethod]
        public void TestGetUsers()
        {
            //UserInfoDal dal = new UserInfoDal();

            UserInfoService userInfoService = new UserInfoService();


            //单元测试在处理数据，不能依赖第三方数据，依赖数据自己创建，用完并删除
            //for (int i = 0; i < 10; i++)
            //{
            //    dal.Add(new UserInfo()
            //    {
            //        UName = i + "ssss";
            //    }); ;
            //}

            for (int i = 0; i < 3; i++)
            {
                userInfoService.Add(new UserInfo { UName = "sss" + i.ToString() });
            }

            //IQueryable<UserInfo> temp = dal.GetUsers(u => u.UName.Contains("ss"));

            //断言
            //Assert.AreEqual(true, temp.Count() >= 10);
            Assert.AreEqual(true, userInfoService.GetEntities(e => e.UName.Contains("s")).FirstOrDefault()!=null);



        }
    }
}
