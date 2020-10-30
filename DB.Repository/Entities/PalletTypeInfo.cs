using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2019-07-21
 * 
 * 功能描述: 
 *      托盘类型信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("PalletType")]
    public class PalletTypeInfo
    {
        /// <summary>
        /// 托盘类型自增号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 托盘编号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 限高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 承载重量
        /// </summary>
        public decimal LoadWeight { get; set; }

        public int LoadAmount { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        public string RawMaterial { get; set; }


        /// <summary>
        /// 供应商
        /// </summary>
        public string Mill { get; set; }


        public DateTime CreateTime { get; set; }


        public DateTime LastActionTime{get;set;}
        
    }
}
