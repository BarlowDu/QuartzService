using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzService.Web.Log
{
    public class LogHandler
    {
        static ILog log = LogManager.GetLogger("RollingLogFileAppender");

        public static void Debug(object message)
        {
            log.Debug(message);
        }
        public static void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }


        public static void Error(object message)
        {
            log.Error(message);
        }
        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public static void Fatal(object message)
        {
            log.Fatal(message);
        }
        public static void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public static void Info(object message)
        {
            log.Info(message);
        }
        public static void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }



        public static void Warn(object message)
        {
            log.Warn(message);
        }
        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }
    }
}