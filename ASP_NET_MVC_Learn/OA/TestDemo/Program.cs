using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MemcachedClient MemClient = getInstance();
            //MemClient.Store(StoreMode.Set, "key0", 0);
            //var value = MemClient.Get("key0").ToString();
            //Console.WriteLine("Add Successfull");
            //Console.WriteLine(value);
            //var result = MemClient.Remove("key0");
            //Console.WriteLine(result);
            Console.ReadKey();
        }


        private static MemcachedClient MemClient;
        static readonly object padlock = new object();
        //线程安全的单例模式
        public static MemcachedClient getInstance()
        {
            if (MemClient == null)
            {
                lock (padlock)
                {
                    if (MemClient == null)
                    {
                        MemClientInit();
                    }
                }
            }
            return MemClient;
        }

        private static void MemClientInit()
        {
            //初始化缓存
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            IPAddress newaddress =
   IPAddress.Parse(Dns.GetHostEntry
 ("192.168.1.102").AddressList[0].ToString());//your_ocs_host替换为OCS内网地址
            IPEndPoint ipEndPoint = new IPEndPoint(newaddress, 11211);
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
}
