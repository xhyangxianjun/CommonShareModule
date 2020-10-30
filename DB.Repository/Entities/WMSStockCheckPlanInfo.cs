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
 *      盘点计划表
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockCheckPlanInfo
    {

        #region Contructors
        public WMSStockCheckPlanInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public string CheckNO { get; set; }

        public string OrganizationCode { get; set; }

        public string WarehouseCode { get; set; }

        public string CheckUser { get; set; }

        public string CheckShift { get; set; }

        public string Remark { get; set; }

        public bool IsLocked { get; set; }
        public bool IsCompleted { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

    
        #endregion
    }
}
