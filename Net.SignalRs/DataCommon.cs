﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.SignalRs
{
    public class DataCommon
    {
        public static DateTime StartTime { get; set; } = DateTime.Parse("2021-1-4");
        public static int UseNumber { get; set; } = 2;
        public static bool IsAllow
        {
            get
            {
                return File.Exists(@"C:\key.txt");
            }
        }
        /// <summary>
        /// 传输超时时间 
        /// </summary>
        public static int ClientConnectTimeout { get; set; } = 20000;
    }
}
