using System;
using System.Collections;
using System.IO;

using log4net.Core;
using log4net.Layout.Pattern;
using log4net.Util;
using log4net.Layout;
using TGLog.Layout;

namespace TGLog.ExpandLayout
{
    public class MyLayout : log4net.Layout.PatternLayout
    {
        public MyLayout()
        {
            this.AddConverter("Operator", typeof(OperatorPatternConverter)); //操作者
            this.AddConverter("Message", typeof(MessagePatternConverter)); //消息
            this.AddConverter("Operand", typeof(OperandPatternConverter)); //操作对象
            this.AddConverter("ActionType", typeof(ActionTypePatternConverter)); //动作
            this.AddConverter("IP", typeof(IPPatternConverter)); //IP
            this.AddConverter("MachineName", typeof(MachineNamePatternConverter)); //机器名
            this.AddConverter("Browser", typeof(BrowserPatternConverter)); //浏览器
        }
    }
  
}
