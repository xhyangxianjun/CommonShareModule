using System;


/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-07-16
 * 
 * 功能描述: 
 *      搜索巷道对应对象
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Data.SqlSugar.Entities
{
    public class MatchSearchAreaInfo
    {
        public int OID { get; set; }

        public int MCTID { get; set; }
        public int CellGroupOID { get; set; }

        public int RackAreaOID { get; set; }
        public int RackSideOID { get; set; }

        public int PLCDestValue { get; set; }

        public int Station { get; set; }

    }
}
