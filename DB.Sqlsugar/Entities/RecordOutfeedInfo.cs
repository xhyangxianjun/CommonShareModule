using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class RecordOutfeedInfo
    {
        #region Contructors
        public RecordOutfeedInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public int LocationOID { get; set; }

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

        public string OutfeedStation { get; set; }

        public int Amount { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 扫描完成后 后期绑定的分录信息
        /// </summary>
        public long WMSStockBillEntryOID { get; set; }

        /// <summary>
        /// 用于扫描出库时选择的出库单据OID
        /// </summary>
        public long WMSStockBillOID { get; set; }

        public int Status { get; set; }

        public string Shift { get; set; }

        public string ShiftSlot { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }
}
