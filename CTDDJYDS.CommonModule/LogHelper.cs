using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CTDDJYDS.CommonModule
{
    public class LogHelper
    {
        private static bool isEnableLog = false;
        private static bool isLogConfigured = false;
        private static string LogName = "AppLog";
        private static ILog myLogger;
        public static bool IsEnableLog
        {
            set
            {
                if (!isLogConfigured && value)
                {
                    using (Stream log4netConfig = Assembly.GetExecutingAssembly().GetManifestResourceStream("ctddjyds.CommonModule.log4net.config"))
                    {
                        XmlConfigurator.Configure(log4netConfig);
                    }
                    isLogConfigured = true;
                }
                isEnableLog = value;
            }
        }
        private static ILog AppLogger
        {
            get
            {
                myLogger = LogManager.Exists(LogName);
                if (myLogger == null)
                {
                    myLogger = LogManager.GetLogger(LogName);
                }
                return myLogger;
            }
        }
        public static void Log(Exception ex)
        {
#if DEBUG
            Trace.WriteLine("Log Exception:");
            Trace.WriteLine(ex.Source ?? "No Source Information");
            Trace.WriteLine(ex.Message);
            Trace.WriteLine(ex.StackTrace);
#endif
            if (!string.IsNullOrEmpty(ex.Source))
            {
                Log(LogLevel.Error, ex.Source);
            }
            Log(LogLevel.Error, ex.Message);
            Log(LogLevel.Error, ex.StackTrace);

            int maxLogEx = 3;
            Exception currentEx = ex;
            int i = 0;
            while (currentEx.InnerException != null && i++ < maxLogEx)
            {
                Log(LogLevel.Error, "InnerException Index - " + i);
                if (!string.IsNullOrEmpty(ex.InnerException.Source))
                {
                    Log(LogLevel.Error, ex.InnerException.Source);
                }
                Log(LogLevel.Error, ex.InnerException.Message);
                Log(LogLevel.Error, ex.InnerException.StackTrace);
                currentEx = currentEx.InnerException;
            }
        }

        public static void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Info:
                    AppLogger.Info(message);
                    break;

                case LogLevel.Debug:
                    AppLogger.Debug(message);
                    break;

                case LogLevel.Warn:
                    AppLogger.Warn(message);
                    break;

                case LogLevel.Error:
                    AppLogger.Error(message);
                    break;

                case LogLevel.Fatal:
                    AppLogger.Fatal(message);
                    break;

                default:
                    break;
            }
        }

        public static void Log(string message)
        {
            Trace.WriteLine(message);
            Log(LogLevel.Info, message);
        }
    }
}
