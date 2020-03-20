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
 *      单据信息 表头
 * 
 ------------------------------------------------------------------------------------*/


namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockBillInfo
    {


        #region Contructors
        public WMSStockBillInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long WMSRequestBillOID { get; set; }

        public long CTRequestBillOID { get; set; }

        /// <summary>
        /// ERP的主键
        /// </summary>
        public string HeadID { get; set; }


        public string VoucherNO { get; set; }

        public string BillType { get; set; }

        public string BusinessType { get; set; }

        public string BusinessTypeHead { get; set; }

        /// <summary>
        /// 收发类别
        /// </summary>
        public string SFLB { get; set; }


        public string BR { get; set; }

        public string GroupCodeFrom { get; set; }


        public string OrganizationCodeFrom { get; set; }

        public string WarehouseCodeFrom { get; set; }

        public string GroupCodeTo { get; set; }


        public string OrganizationCodeTo { get; set; }

        public string WarehouseCodeTo { get; set; }

        public string DepartmentCode { get; set; }

        /// <summary>
        /// 此处保存的是供应商信息(来源于CT_Request.cvendorbaseid,采购是供应商，销售是客户)
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 供应商(注意此处保存的是客户信息，而表体中保存的是供应商信息
        /// </summary>
        public string SupplierCode { get; set; }

        public string TransportTypeCode { get; set; }

        public string OriginatorCode { get; set; }

        public string SalerCode { get; set; }

        public string KeeperCode { get; set; }


        public DateTime BillDate { get; set; }

        public string BillRemark { get; set; }

        public string OperateMode { get; set; }

        public string PackingBillNO { get; set; }

        public string PackingBillUser { get; set; }

        public DateTime PackingBillDate { get; set; }

        public bool IsCheck { get; set; }

        public string CheckUser { get; set; }

        public DateTime CheckDate { get; set; }

        public bool IsCancel { get; set; }

        public string CancelUser { get; set; }

        public DateTime CancelDate { get; set; }

        public bool IsCloseBill { get; set; }

        public string CloseBillUser { get; set; }

        public DateTime CloseBillDate { get; set; }

        public bool IsFinished { get; set; }

        public string FinishBillUser { get; set; }

        public DateTime FinishBillDate { get; set; }

        public bool IsUpload { get; set; }

        public string UploadUser { get; set; }

        public DateTime UploadDate { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string VehicleNO { get; set; }

        /// <summary>
        /// 提货人
        /// </summary>
        public string Shipper { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 运输公司
        /// </summary>
        public string TransportCompany { get; set; }



        public decimal CommitQty { get; set; }

        public decimal CommitAuxQty1 { get; set; }

        public decimal CommitAuxQty2 { get; set; }

        /// <summary>
        /// 是否正在编辑此单据
        /// </summary>
        public bool IsEditing { get; set; }

        public DateTime EditTime { get; set; }

        public string EditUser { get; set; }

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
