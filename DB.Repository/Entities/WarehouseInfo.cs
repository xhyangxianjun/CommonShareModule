using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-06-21
 * 
 * 功能描述: 
 *      仓库信息
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class WarehouseInfo
    {
        public int WarehouseOID { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
  

        public string OrganizationCode { get; set; }

        public string OrganizationName { get; set; }

        public string ParentCode { get; set; }

        public string LevelCode { get; set; }


        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

    }
}
