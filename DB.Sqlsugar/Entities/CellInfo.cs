using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Chaint.Data.SqlSugar.Entities
{
    public class CellInfo
    {

        /// <summary>
        /// 货位
        /// </summary>
        public int LocationOID { get; set; }

        /// <summary>
        /// 货格
        /// </summary>
   
        public int CellOID { get; set; }
    
        public string CellName { get; set; }
        /// <summary>
        /// 排
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Bay { get; set; }

        /// <summary>
        /// 层
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 进深位 靠近堆垛机的为1
        /// </summary>
        public int LocationID { get; set; }


        public string LocationPosition { get; set; }
        public string LocationName { get; set; }

        public string CellPosition { get; set; }
        
        public int RackBunchOID { get; set; }
        public int RackSideOID { get; set; }
        public int RackAreaOID { get; set; }

        /// <summary>
        /// 巷道
        /// </summary>
        public int LaneWay { get; set; }
        public string RackAreaPosition { get; set; }

        public override string ToString()
        {
            return String.Format("CellOID:{0},Row:{1},Bay:{2},Level:{3},LocationID:{4},CellPosition:{5},CellName:{6},RackBunchOID:{7},RackSideOID:{8},RackAreaOID:{9},LaneWay:{10}",
               CellOID, Row, Bay, Level,LocationID, CellPosition, CellName, RackBunchOID, RackSideOID, RackAreaOID, LaneWay);
        }

    }
}
