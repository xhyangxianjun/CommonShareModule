using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-01-21
 * 
 * 功能描述: 
 *      区域分配（AreaAllocation)的结果信息 
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{

    public class AAResultInfo : ResultInfo
    {
        /// <summary>
        /// 货格信息
        /// </summary>
        public CellInfo Cell { get; set; }

        public long CellGroupOID { get; set; }

        public int RackAreaOID { get; set; }


        public int RetDest { get; set; }


        /// <summary>
        /// 分配至哪个入库栈台编号
        /// </summary>
        public int Station { get; set; }


        public bool IsNeedSearchNextLaneway { get; set; } = false;

        public int ProductGroupOID { get; set; }

    }
}
