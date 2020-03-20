using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSProductLifeInfo
    {
        #region Contructors
        public WMSProductLifeInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public long CellGroupOID { get; set; }

    //    public long ProductOnlyID { get; set; }

        public long ProductOID { get; set;}

        public string ProductID { get; set; }

    //    public string Status { get; set; }

        /// <summary>
        /// 前一种状态
        /// </summary>
        public int PreState { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int State { get; set; }

        public string OperateMsg { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperateMode { get; set; }

        public string OperateUser { get; set; }

        public string OperateShift { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

       
        #endregion
    }
}
