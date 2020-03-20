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
 *      单据计划 分录 信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSRequestBillEntryInfo
    {

        #region Contructors
        public WMSRequestBillEntryInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long WMSRequestBillOID { get; set; }

        public long CTRequestBillOID { get; set; }

        public int EntryID { get; set; }

        /// <summary>
        /// ERP用 分录主键 对应EntryPK
        /// </summary>
        public string BodyID { get; set; }

        public string EntryOrganizationCode { get; set; }
        public string EntryWarehouseCode { get; set; }

        public string BatchNO { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string SKU { get; set; }

        public string SpecificationCode { get; set; }

        public string UnitCode { get; set; }

        public string OrderNO { get; set; }

        /// <summary>
        /// 客户从表头放至分录
        /// </summary>
        public string CustomerCode { get; set; }

        public string Remark { get; set; }

        public decimal PlanQty { get; set; }

        public decimal PlanAuxQty1 { get; set; }

        public decimal PlanAuxQty2 { get; set; }

        /// <summary>
        /// 件数(箱数)
        /// </summary>
        public decimal CommitQty { get; set; }

        /// <summary>
        /// 重量 KG
        /// </summary>
        public decimal CommitAuxQty1 { get; set; }

        public decimal CommitAuxQty2 { get; set; }

        /// <summary>
        /// 来源单号ID
        /// </summary>
        public string SourceBillID { get; set; }

        /// <summary>
        /// 来源单据行ID
        /// </summary>
        public string SourceBillRowID { get; set; }

        /// <summary>
        /// 来源单据类型
        /// </summary>
        public string SourceBillType { get; set; }

        /// <summary>
        /// 到货日期
        /// </summary>
        public string CDefine1 { get; set; }

        /// <summary>
        /// 来源单据号
        /// </summary>
        public string CDefine2 { get; set; }

        /// <summary>
        /// 来源单据行号
        /// </summary>
        public string CDefine3 { get; set; }

        /// <summary>
        /// 商超单号
        /// </summary>
        public string CDefine4 { get; set; }

        public string CDefine5 { get; set; }

        public string CDefine6 { get; set; }

        public string CDefine7 { get; set; }

        public string CDefine8 { get; set; }

        public string CDefine9 { get; set; }

        public string CDefine10 { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

     
        #endregion
    }
}
