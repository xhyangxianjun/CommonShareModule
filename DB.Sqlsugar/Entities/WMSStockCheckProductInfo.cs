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
 * 盘点时的扫描记录
 ------------------------------------------------------------------------------------*/


namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSStockCheckProductInfo
    {

        #region Contructors
        public WMSStockCheckProductInfo()
        {
        }

        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long CheckPlanOID { get; set; }

        public long ProductOID { get; set; }

        public string ProductID { get; set; }

        public DateTime OperateTime { get; set; }

        public string OperateUser { get; set; }

        public string OperateShift { get; set; }

        public string OperateMode { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

      
        #endregion

    }
}
