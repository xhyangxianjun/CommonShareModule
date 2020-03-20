using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockBillPackingListInfo
    {
        #region Contructors
        public WMSStockBillPackingListInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long WMSStockBillOID { get; set; }

        public long WMSStockBillEntryOID { get; set; }

        public string ProductID { get; set; }

        public long ProductOID { get; set; }

        public long ProductOnlyID { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string CustomerCode { get; set; }

        public int LocationOID { get; set; }

        public string LocationName { get; set; }

        public string OrganizationCode { get; set; }

        public string WarehouseCode { get; set; }

        public int Status { get; set; }

        public decimal CommitQty { get; set; }

        public decimal CommitAuxQty1 { get; set; }

        public decimal CommitAuxQty2 { get; set; }

        public string Remark { get; set; }

        public string CDefine1 { get; set; }

        public string CDefine2 { get; set; }

        public string CDefine3 { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

     
        #endregion
    }
}
