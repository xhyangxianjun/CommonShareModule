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
 *      
 *      盘点结果表(盘盈Profit/盘亏Loss)
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockCheckResultInfo
    {
        #region Contructors
        public WMSStockCheckResultInfo()
        {

        }

        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long CheckPlanOID { get; set; }


        public string CheckNO { get; set; }
        public string ResultType { get; set; }

        public string ResultTypeDesc { get; set; }

        public int LocationOID { get; set; }

        public long CellGroupOID { get; set; }

        public string OrganizationCode { get; set; }

        public string WarehouseCode { get; set; }

        public int Amount { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }
}
