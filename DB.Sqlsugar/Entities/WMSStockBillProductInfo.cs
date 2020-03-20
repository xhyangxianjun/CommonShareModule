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
 *      WMS_Stock_Entry 对应的产品信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockBillProductInfo
    {
        #region Contructors
        public WMSStockBillProductInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long WMSStockBillOID { get; set; }

        public long WMSStockBillEntryOID { get; set; }

        public long ProductOID { get; set; }

        public long ProductOnlyID { get; set; }

        public string ProductID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string OperateUser { get; set; }

        /// <summary>
        /// 操作班组
        /// </summary>
        public string OperateShift { get; set; }


        public string OperateMode { get; set; }

        public decimal CommitQty { get; set; }

        public decimal CommitAuxQty1 { get; set; }

        public decimal CommitAuxQty2 { get; set; }

        public string Remark { get; set; }

        public string CDefine1 { get; set; }

        public string CDefine2 { get; set; }

        public string CDefine3 { get; set; }

        /// <summary>
        /// 对应Product.PreState
        /// </summary>
        public int ProductPreState { get; set; }

        /// <summary>
        /// 对应Product.State
        /// </summary>
        public int ProductState { get; set; }


        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

    
        #endregion
    }
}
