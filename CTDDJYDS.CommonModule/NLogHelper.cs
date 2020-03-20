using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTDDJYDS.CommonModule
{
    public class NLogHelper
    {
        private static string LogName = "AppLog";
        private static Logger myLogger;
        private static bool isEnableLog = false;
        public static bool IsEnableLog
        {
            set => isEnableLog = value;
        }
        private static Logger AppLogger
        {
            get
            {
                if (myLogger == null && isEnableLog)
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
    }
}
