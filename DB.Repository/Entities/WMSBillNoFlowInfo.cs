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
 *      单据编号生成方式
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSBillNoFlowInfo
    {

        #region Contructors
        public WMSBillNoFlowInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public string BillNO { get; set; }

        public string BillType { get; set; }

        public string BillSerialNO { get; set; }

        public string BillStatus { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        public int OptimisticLockField { get; set; }
        #endregion
    }
}
