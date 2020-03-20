
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-07-16
 * 
 * 功能描述: 
 *      
 *          满足条件货位排序策略 Order By
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("FilterRule_Allocate")]
    public class FilterRuleAllocateInfo
    {
        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public string Name { get; set; }

        public string FilterString { get; set; }

        public int AllocateType { get; set; }

        public string AllocateTypeDesc { get; set; }

        public string Description { get; set; }

        public bool IsEnable { get; set; }

        public bool IsReadOnly { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }
}
