using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Common.Cache
{
    /// <summary>
    /// 封装缓存的常用操作方法
    /// </summary>
    public interface ICacheWriter
    {
        void AddCache(string key, object value,DateTime expDate);
        void AddCache(string key, object value);
        object GetCache(string key);
        T GetCache<T>(string key);

        void SetCache(string key, object value, DateTime extDate);
        void SetCache(string key, object value);

    }




}
