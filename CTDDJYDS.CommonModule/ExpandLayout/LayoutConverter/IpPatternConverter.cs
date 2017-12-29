using System;
using System.Collections.Generic;
using System.Text;
using log4net.Layout.Pattern;
using System.IO;
using log4net.Core;

namespace TGLog.ExpandLayout
{
    /// <summary>
    /// IP
    /// </summary>
    internal sealed class IPPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.IP );
            }
        }
    }
}
