using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OA.Common.Cache
{
    public class MemcacheWriter : ICacheWriter
    {

        public MemcacheWriter()
        {
            //利用构造函数，初始化属性，防止Spring.Net注入，缺失属性（理论上这种属性也是可以注入的，可以尝试）
            getInstance(GetServerDicFormConfig());
        }


        public void AddCache(string key, object value, DateTime expDate)
        {
            MemClient.Store(StoreMode.Add,key, value, expDate);
        }

        public void AddCache(string key, object value)
        {
            MemClient.Store(StoreMode.Add, key, value);
        }

        public object GetCache(string key)
        {
           return MemClient.Get(key);
        }

        public T GetCache<T>(string key)
        {
            return (T)MemClient.Get(key);
        }

        private MemcachedClient memClient;
        public MemcachedClient MemClient 
        {
            get
            {
                return memClient;
            }

            set
            {
                memClient = value;
            }
        }

        //static readonly object padlock = new object();
        //private static readonly object padlock { get; set; }

        public Dictionary<string, string> GetServerDicFormConfig()
        {
            string strAppMemcachedServer = System.Configuration.ConfigurationManager.AppSettings["MemcachedServerList"];
            var servers = strAppMemcachedServer.Split(',');
            Dictionary<string, string> serverDic = new Dictionary<string, string>();
            foreach (var server in servers)
            {
                var config = server.Split(':');
                serverDic.Add(config[0], config[1]);
            }
            return serverDic;
        }

        //线程安全的单例模式
        public MemcachedClient getInstance(Dictionary<string,string> ServerDic)
        {
            if (MemClient == null)
            {
                //lock (padlock)
                //{
                    if (MemClient == null)
                    {
                        MemClientInit(ServerDic);
                    }
                //}
            }
            return MemClient;
        }

        private void MemClientInit(Dictionary<string, string> ServerDic)
        {
            foreach (var keyValuePair in ServerDic)
            {
                //初始化缓存
                MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
                IPAddress newaddress = IPAddress.Parse(Dns.GetHostEntry(keyValuePair.Key).AddressList[0].ToString());
                IPEndPoint ipEndPoint = new IPEndPoint(newaddress, Int32.Parse(keyValuePair.Value));
                // 配置文件 - ip
                memConfig.Servers.Add(ipEndPoint);
                // 配置文件 - 协议
                memConfig.Protocol = MemcachedProtocol.Binary;
                // 配置文件-权限
                //memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
                //memConfig.Authentication.Parameters["zone"] = "";
                //memConfig.Authentication.Parameters["userName"] = "username";
                //memConfig.Authentication.Parameters["password"] = "password";
                //下面请根据实例的最大连接数进行设置
                memConfig.SocketPool.MinPoolSize = 5;
                memConfig.SocketPool.MaxPoolSize = 200;
                MemClient = new MemcachedClient(memConfig);
            }


        }

        void ICacheWriter.SetCache(string key, object value, DateTime extDate)
        {
            MemClient.Store(StoreMode.Set, key, value, extDate);
        }

        void ICacheWriter.SetCache(string key, object value)
        {
            MemClient.Store(StoreMode.Set, key, value);
        }
    }

}
