using SqlSugar;
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
 *      客户信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Enable_RackArea_Infeed")]
    public class Enable_RackArea_InfeedInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }
        public int RackAreaOID { get; set; }

        public bool IsLocked { get; set; }

        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

    }
}
