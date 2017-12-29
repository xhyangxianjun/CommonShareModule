using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TGLog.ExpandLayout
{

    /// <summary>
    /// 动作
    /// </summary>
    public enum ActionType
    {
        View = 0,
        Add,
        Edit,
        Delete,
        Verify,
        Other
    }
 /// <summary>
    /// 日志信息类
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// 1个参数构造
        /// </summary>
        /// <param name="message"></param>
        public LogMessage(string message)
        {
            this.ActionType = 0;
            this.Operator = 0;
            this.Message = message;
            this.Operand = "";
            this.IP = "";
            this.Browser = "";
            this.MachineName = "";
        }
        /// <summary>
        /// 3个参数构造
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="ActionType"></param>
        /// <param name="message"></param>
        public LogMessage(int operatorID, int ActionType, string message)
        {
            this.ActionType = ActionType;
            this.Operator = operatorID;
            this.Message = message;
            this.Operand = "";
            this.IP = "";
            this.Browser = "";
            this.MachineName = "";
        }

        /// <summary>
        /// 4个参数构造
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="operand"></param>
        /// <param name="ActionType"></param>
        /// <param name="message"></param>
        public LogMessage(
             int operatorID,
             string operand,
             int ActionType,
             string message
             )
        {
            this.ActionType = ActionType;
            this.Operator = operatorID;
            this.Message = message;
            this.Operand = operand;
            this.IP = "";
            this.Browser = "";
            this.MachineName = "";
        }

        /// <summary>
        /// 全部参数构造
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="operand"></param>
        /// <param name="ActionType"></param>
        /// <param name="message"></param>
        /// <param name="ip"></param>
        /// <param name="machineName"></param>
        /// <param name="browser"></param>
        public LogMessage(
            int operatorID,
            string operand,
            int ActionType,
            string message,
            string ip,
            string machineName,
            string browser
            )
        {
            this.ActionType = ActionType;
            this.Operator = operatorID;
            this.Message = message;
            this.Operand = operand;
            this.IP = ip;
            this.Browser = browser;
            this.MachineName = machineName;
        }

        /// <summary>
        /// 操作者
        /// </summary>
        public int Operator
        { 
            get;
            set;
        }

        /// <summary>
        /// 操作对象
        /// </summary>
        public string Operand
        {
            get;
            set;
        }

        /// <summary>
        /// 当前IP地址
        /// </summary>
        public string IP
        {
            get;
            set;
        }

        /// <summary>
        /// 当前浏览器
        /// </summary>
        public string Browser
        {
            get;
            set;
        }

        /// <summary>
        /// 当前机器名
        /// </summary>
        public string MachineName
        {
            get;
            set;
        }


        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 动作(类型和枚举类型ActionType一致)
        /// </summary>
        public int ActionType
        {
            get;
            set;
        }
    
        /// <summary>
        /// 得到哈希表
        /// </summary>
        /// <returns></returns>
        public Hashtable ToHashtable()
        {
            Hashtable ht = new Hashtable();
            ht.Add("Operator", this.Operator);
            ht.Add("Message", this.Message);
            ht.Add("ActionType",  this.ActionType);
            ht.Add("Operand", this.Operand);
            ht.Add("IP", this.IP);
            ht.Add("MachineName", this.MachineName);
            ht.Add("Browser", this.Browser);
            return ht;
        }
    }
}