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
 *      盘点过程表
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockCheckDetailInfo
    {
        #region Contructors
        public WMSStockCheckDetailInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long CheckPlanOID { get; set; }

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
