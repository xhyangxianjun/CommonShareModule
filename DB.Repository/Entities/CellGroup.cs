using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("CellGroup")]    //对应数据库的CellGroup表
    public class CellGroup
    {
        /// <summary>
        /// CellGroupOID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")] //是主键和标识列
        public long OID { get; set; }

        /// <summary>
        /// 总宽度
        /// </summary>
        public decimal TotalWidth { get; set; }

        /// <summary>
        /// 总长度
        /// </summary>
        public decimal TotalLength { get; set; }

        /// <summary>
        /// 总高度
        /// </summary>
        public decimal TotalHeight { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 总数量  产品Product数量
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// 产品分类号 
        /// </summary>
        public long ProductGroupOID { get; set; }

        /// <summary>
        /// 单位数量 箱数
        /// </summary>
        public decimal UnitAmount { get; set; }

        /// <summary>
        /// 去向
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// 组盘数据来源
        /// </summary>
        public string SrcPosition { get; set; }

        /// <summary>
        /// 表示组盘后编号
        /// </summary>
        public string PalletNO { get; set; }

        /// <summary>
        /// 是否为余盘
        /// </summary>
        public bool IsReturnPallet { get; set; } = false;

        
    }
}
