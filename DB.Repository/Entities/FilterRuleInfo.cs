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
 *      策略实体
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("FilterRule")]
    public class FilterRuleInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public string Name { get; set; }
        public string FilterString { get; set; }


        public int SortID { get; set; }

        public string RuleType { get; set; }

        public string Description { get; set; }

        public bool IsEnable { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsSystem { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }


    }
}
