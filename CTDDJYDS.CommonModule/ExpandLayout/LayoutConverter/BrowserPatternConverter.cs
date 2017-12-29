using System;
using System.Collections.Generic;
using System.Text;
using log4net.Layout.Pattern;
using System.IO;
using log4net.Core;

namespace TGLog.ExpandLayout
{
    /// <summary>
    /// 浏览器
    /// </summary>
    internal sealed class BrowserPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.Browser );
            }
        }
    }
}
