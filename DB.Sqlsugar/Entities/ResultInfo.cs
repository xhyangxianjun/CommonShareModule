using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class ResultInfo
    {
        /// <summary>
        /// 结果代码  00:代表成功,其他为异常
        /// </summary>
        public string RetCode { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string RetMessage { get; set; }
        
        /// <summary>
        /// 自定义对象
        /// </summary>
        public object CustomObj { get; set; }

    }
}
