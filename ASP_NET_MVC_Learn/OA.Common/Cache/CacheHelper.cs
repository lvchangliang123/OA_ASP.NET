using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Common.Cache
{
    /// <summary>
    /// 使用缓存的帮助类，同时能够实现缓存的切换
    /// </summary>
    public class CacheHelper 
    {
        //利用Spring.Net实现Cache的实例注入，实现多种缓存的实现(利用属性注入)

        //注意：静态属性在Spring注入的时候，由于不需要创建类的实例，所以可能为空！！！

        static CacheHelper()
        {
            //通过构造函数中，直接操作Spring容器，强制注入,防止静态属性注入为空
            IApplicationContext applicationContext = ContextRegistry.GetContext();
            CacheHelper.CacheWriter = applicationContext.GetObject("CacheWriter") as ICacheWriter;

        }


        public static ICacheWriter CacheWriter { get; set; }
        public static void AddCache(string key, object value, DateTime expDate)
        {
            CacheWriter.AddCache(key, value, expDate);
        }

        public static void AddCache(string key, object value)
        {
            CacheWriter.AddCache(key, value);
        }

        public static object GetCache(string key)
        {
           return CacheWriter.GetCache(key);
        }

        public static T GetCache<T>(string key)
        {
            return (T)CacheWriter.GetCache(key);
        }

        public static void SetCache(string key,object value,DateTime extTime)
        {
            CacheWriter.SetCache(key, value, extTime);
        }


        public static void SetCache(string key, object value)
        {
            CacheWriter.SetCache(key, value);
        }


    }
}
