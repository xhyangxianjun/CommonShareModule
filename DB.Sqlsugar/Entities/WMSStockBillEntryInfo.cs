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
 *      单据信息 分录信息
 * 
 ------------------------------------------------------------------------------------*/


namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockBillEntryInfo
    {


        #region Contructors
        public WMSStockBillEntryInfo()
        {

        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long WMSStockBillOID { get; set; }

        public long CTRequestBillOID { get; set; }

        public long WMSStockBillPlanOID { get; set; }

        public int EntryID { get; set; }

        /// <summary>
        /// ERP用 分录主键
        /// </summary>
        public string BodyID { get; set; }


        public string EntryOrganizationCode { get; set; }
        public string EntryWarehouseCode { get; set; }

        public string EntryCustomerCode { get; set; }


        public string BatchNO { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string SKU { get; set; }

        public string SpecificationCode { get; set; }

        public string UnitCode { get; set; }

        public string OrderNO { get; set; }



        public string Remark { get; set; }

        public decimal PlanQty { get; set; }

        public decimal PlanAuxQty1 { get; set; }

        public decimal PlanAuxQty2 { get; set; }

        public decimal CommitQty { get; set; }

        public decimal CommitAuxQty1 { get; set; }

        public decimal CommitAuxQty2 { get; set; }

        public decimal RemainQty { get; set; }

        public decimal RemainAuxQty1 { get; set; }

        public decimal RemainAuxQty2 { get; set; }

        public string CDefine1 { get; set; }

        public string CDefine2 { get; set; }

        public string CDefine3 { get; set; }

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
