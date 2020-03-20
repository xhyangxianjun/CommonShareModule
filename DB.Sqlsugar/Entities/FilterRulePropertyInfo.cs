using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-06-21
 * 
 * 功能描述: 
 *      
 *      针对查找某一货位的搜索属性字段  Order By中的属性信息
 ------------------------------------------------------------------------------------*/
namespace Chaint.Data.SqlSugar.Entities
{
    /// <summary>
    /// 某一种货位搜索的属性信息
    /// </summary>
    [SugarTable("FilterRule_Allocate_Property")]
    public class FilterRulePropertyInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        /// <summary>
        /// 分配类型 1:InfeedArea,2:InfeedCell,3:OutfeedOrder
        /// </summary>
        public int AllocateType { get; set; }


        /// <summary>
        /// 属性名称--字段
        /// </summary>
        [SugarColumn(ColumnName = "Name")]
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性描述
        /// </summary>
        public string Description { get; set; }


    }



}
