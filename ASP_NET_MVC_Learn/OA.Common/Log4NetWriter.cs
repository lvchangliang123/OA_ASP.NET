using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Common
{
    public class Log4NetWriter : ILogWriter
    {
        public void WriteLogInfo(string logText)
        {
            ILog logWriter = log4net.LogManager.GetLogger("Logger4Net");
            logWriter.Error(logText);
        }
    }
}
