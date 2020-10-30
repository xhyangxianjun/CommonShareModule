using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class RecordInfeedInfo
    {
        #region Public Properties

        public long OID { get; set; }

        public int LocationOID { get; set; }

        public string LocationName { get; set; }


        public long CellGroupOID { get; set; }

        public string OrganizationCode { get; set; }
        public string WarehouseCode { get; set; }

        public int UserOID { get; set; }

        /// <summary>
        /// 用户名称 用于保存至ProductLife
        /// </summary>
        public string UserName { get; set; }

        public string BusinessType { get; set; }

        public string BR { get; set; }

        public string InfeedStation { get; set; }

        public int Amount { get; set; }

        public string Remark { get; set; }

        public long WMSStockBillEntryOID { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// 入库班组 甲乙丙
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// 班次 早中晚
        /// </summary>
        public string ShiftSlot { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }

}
