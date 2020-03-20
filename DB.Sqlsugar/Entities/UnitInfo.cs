using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-06-21
 * 
 * 功能描述: 
 *      单位
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class UnitInfo
    {
        public int UnitOID { get; set; }

        public string UnitCode { get; set; }
        public string UnitName { get; set; }

        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }
    }
}
