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
 *      托盘信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Pallet")]
    public class PalletInfo
    {
        /// <summary>
        /// 托盘自增号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int PalletOID { get; set; }

        /// <summary>
        /// 托盘编号
        /// </summary>
        public string PalletNO { get; set; }

        /// <summary>
        /// 托盘流水
        /// </summary>
        public string PalletSerialID { get; set; }

        /// <summary>
        /// 托盘类型
        /// </summary>
        public int PalletTypeOID { get; set; }


        //public int PalletWidth { get; set; }

        //public int PalletHeight { get; set; }

        //public int PalletLength { get; set; }

        //public int PalletLoadWeight { get; set; }

        //public string PalletColor { get; set; }

        //public string PalletRawMaterial { get; set; }

        //public string PalletMill { get; set; }


        //public DateTime PalletLastActionTime { get; set; }

        /// <summary>
        /// 托盘使用次数
        /// </summary>
        public int UseAmount { get; set; }


        public int PrintAmount { get; set; }


        //public string MaterialCode { get; set; }

        //public string MaterialName { get; set; }

        //public string SKU { get; set; }

        ///// <summary>
        ///// 规格
        ///// </summary>
        //public string Specification { get; set; }

        /// <summary>
        /// 定量
        /// </summary>
        public decimal BasisWeight { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        ///// <summary>
        ///// 组织
        ///// </summary>
        //public string Organizition { get; set; }

        ///// <summary>
        ///// 机台号
        ///// </summary>
        //public string MachineID { get; set; }

        ///// <summary>
        ///// 生产班组
        ///// </summary>
        //public string ProductShift { get; set; }

        ///// <summary>
        ///// 质检员
        ///// </summary>
        //public string Inspector { get; set; }

        ///// <summary>
        ///// 品牌
        ///// </summary>
        //public string Brand { get; set; }

        ///// <summary>
        ///// 生产日期
        ///// </summary>
        //public DateTime ProductDate { get; set; }

        ///// <summary>
        ///// 质保天数
        ///// </summary>
        //public int ExpireDays { get; set; }

        ///// <summary>
        ///// 过期日期
        ///// </summary>
        //public DateTime ExpireDateTime { get; set; }

        ///// <summary>
        ///// 材质
        ///// </summary>
        //public string Texture { get; set; }

        ///// <summary>
        ///// 标准
        ///// </summary>
        //public string Standard { get; set; }


        ///// <summary>
        ///// 箱数
        ///// </summary>
        //public int Amount { get; set; }


        //public string Cdefine1 { get; set; }
        //public string Cdefine2 { get; set; }
        //public string Cdefine3 { get; set; }


        //public decimal Udefine1 { get; set; }

        //public decimal Udefine2 { get; set; }
        //public decimal Udefine3 { get; set; }

        //public string Remark { get; set; }


        //public int CarType { get; set; }
        //public int LoadAmount { get; set; }
    }
}
