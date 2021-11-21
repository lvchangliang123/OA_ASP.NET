using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OA.Common
{
    public class LogHelper
    {
        public static Queue<string> ExceptionStringQueue = new Queue<string>();
        public static List<ILogWriter> logWriters = new List<ILogWriter>();     //1.

        static LogHelper()      //静态构造函数，在第一次用到这个类的时候执行,只被执行一次
        {
            //logWriters.Add(new TextFileWriter());               //2.
            //logWriters.Add(new SqlLogWriter());
            logWriters.Add(new Log4NetWriter());

            //将消息队列中的错误信息，写入到日志文件中
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {

                    lock (ExceptionStringQueue)
                    {
                        if (ExceptionStringQueue.Count>0)
                        {
                            string ExpStr = ExceptionStringQueue.Dequeue();

                            foreach (var logWriter in logWriters)               //3.观察者模式
                            {
                                logWriter.WriteLogInfo(ExpStr);
                            }
                        }
                        else
                        {
                            Thread.Sleep(30);
                        }
                      
                    }
                }
            });

        }

        public static void WriteLog(string exceptionText)
        {
            lock (ExceptionStringQueue)
            {
                ExceptionStringQueue.Enqueue(exceptionText);
            }
        }
    }
}
