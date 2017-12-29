using System;
using System.Collections.Generic;
using System.Text;
using log4net.Layout.Pattern;
using System.IO;
using log4net.Core;

namespace TGLog.ExpandLayout
{
    /// <summary>
    /// 动作
    /// </summary>
    internal sealed class ActionTypePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write((int)logMessage.ActionType);
            }
        }
    }
}