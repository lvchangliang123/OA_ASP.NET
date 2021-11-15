using OA.IDAL;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.EFDAL
{
    public partial class UserInfoDal : BaseDal<UserInfo>, IUserInfoDal
    {

        //oaEntities db = new oaEntities();
        //public userinfo GetUserInfoById(int id)
        //{
        //    return db.userinfoes.Find(id);
        //}


        //public List<userinfo> GetAllUserInfoes()
        //{
        //    return db.userinfoes.ToList();
        //}

        //public IQueryable<userinfo> GetUsers(Expression<Func<userinfo,bool>> whereLambda)
        //{
        //    return db.userinfoes.Where(whereLambda).AsQueryable();
        //}

        ////分页查询方法
        //public IQueryable<userinfo> GetPageUser<S>(int pageSize,int pageIndex,out int total,Expression<Func<userinfo,bool>> whereLambda,Expression<Func<userinfo,S>> orderByLambda,bool isAsc)
        //{
        //    total = db.userinfoes.Where(whereLambda).Count();
        //    IQueryable<userinfo> temp = null;
        //    if (isAsc)
        //    {
        //        temp = db.userinfoes.Where(whereLambda).OrderBy<userinfo, S>(orderByLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
        //    }
        //    else
        //    {
        //        temp = db.userinfoes.Where(whereLambda).OrderByDescending<userinfo, S>(orderByLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
        //    }

        //    return temp;
        //}

        //public userinfo Add(userinfo userinfo)
        //{
        //    db.userinfoes.Add(userinfo);
        //    db.SaveChanges();
        //    return userinfo;
        //}

        //public bool Update(userinfo userinfo)
        //{
        //    db.Entry(userinfo).State = System.Data.Entity.EntityState.Modified;
        //    return db.SaveChanges()>0;
        //}

        //public bool Delete(userinfo userinfo)
        //{
        //    db.Entry(userinfo).State = System.Data.Entity.EntityState.Deleted;
        //    return db.SaveChanges() > 0; 
        //}

    }
}
