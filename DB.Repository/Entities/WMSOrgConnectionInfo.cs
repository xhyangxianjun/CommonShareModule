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
 *      WMS 组织下对应的DB连接串
 * 
 ------------------------------------------------------------------------------------*/


namespace Chaint.Data.SqlSugar.Entities
{
    public class WMSOrgConnectionInfo
    {
        #region Contructors
        public WMSOrgConnectionInfo()
        {
        }


        #endregion

        #region Public Properties

        public int OID { get; set; }

        public string OrganizationCode { get; set; }

        public string WarehouseCode { get; set; }

        public string ConnectionString { get; set; }

        public bool IsEnable { get; set; }

        public bool IsLocal { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

       
        #endregion
    }
}
