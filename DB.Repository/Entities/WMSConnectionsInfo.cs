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
 *      DB连接串信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSConnectionsInfo
    {
        #region Contructors
        public WMSConnectionsInfo()
        {
        }
#endregion

        #region Public Properties

        public int OID { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public bool IsEnable { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }
}
