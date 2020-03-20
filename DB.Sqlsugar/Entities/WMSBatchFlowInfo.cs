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
 *      批次号生成方式
 * 
 ------------------------------------------------------------------------------------*/


namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSBatchFlowInfo
    {

        #region Contructors
        public WMSBatchFlowInfo()
        {
        }


        #endregion

        #region Public Properties

        public long OID { get; set; }

        public string BatchNO { get; set; }

        public string BatchType { get; set; }

        public string BatchSerialNO { get; set; }

        public string BatchStatus { get; set; }

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

        public string CDefine11 { get; set; }

        public string CDefine12 { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        public int OptimisticLockField { get; set; }
        #endregion
    }
}
