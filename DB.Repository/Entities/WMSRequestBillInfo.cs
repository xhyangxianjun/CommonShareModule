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
 *      单据计划  信息
 * 
 ------------------------------------------------------------------------------------*/


namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSRequestBillInfo
    {
        #region Contructors
        public WMSRequestBillInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long CTRequestBillOID { get; set; }

        /// <summary>
        /// 单据主键
        /// </summary>
        public string HeadID { get; set; }

        public string BillType { get; set; }

        public string VoucherNO { get; set; }

        public string BusinessType { get; set; }

        public string BusinessTypeHead { get; set; }

        /// <summary>
        /// 收发类别，上级为单据类型
        /// </summary>
        public string SFLB { get; set; }

        public string BR { get; set; }

        /// <summary>
        /// 发起公司
        /// </summary>
        public string GroupCodeFrom { get; set; }

        public string OrganizationCodeFrom { get; set; }

        public string WarehouseCodeFrom { get; set; }

        /// <summary>
        /// 目标公司
        /// </summary>
        public string GroupCodeTo { get; set; }
        public string OrganizationCodeTo { get; set; }

        public string WarehouseCodeTo { get; set; }

        public string DepartmentCode { get; set; }

        public string CustomerCode { get; set; }

        public string TransportTypeCode { get; set; }

        public string OriginatorCode { get; set; }

        public string SalerCode { get; set; }

        public DateTime BillDate { get; set; }

        public string BillRemark { get; set; }

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

        //public decimal PlanCommitQty { get; set; }

        //public decimal PlanCommitAuxQty1 { get; set; }

        //public decimal PlanCommitAuxQty2 { get; set; }

        public decimal CommitQty { get; set; }

        public decimal CommitAuxQty1 { get; set; }

        public decimal CommitAuxQty2 { get; set; }

        public string OperateMode { get; set; }

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

        /// <summary>
        /// 保管员
        /// </summary>
        public string KeeperCode { get; set; }

        /// <summary>
        /// 提货人
        /// </summary>
        public string Shipper { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string VehicleNO { get; set; }

        /// <summary>
        /// 运输公司
        /// </summary>
        public string TransportCompany { get; set; }


        #endregion

    }
}
