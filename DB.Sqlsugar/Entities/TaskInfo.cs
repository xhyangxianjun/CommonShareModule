using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chaint.Data.SqlSugar.Entities
{
    public class TaskInfo
    {
        public long TaskCacheOID { get; set; }

        public int MCTID { get; set; }

        public int RackSideOID { get; set; }
        public long CellGroupOID { get; set; }

        /// <summary>
        /// 当前任务下一个要执行的任务ID   任务间有依赖关系时
        /// </summary>
        public long NextTaskCacheOID { get; set; }

        /// <summary>
        /// 当前任务的初始任务ID  任务间有依赖关系时
        /// </summary>
        public long FirstTaskCacheOID { get; set; }

        /// <summary>
        /// 起始栈台类型 入库10,出库20,或者 A,B,C
        /// </summary>
        public string FromStation { get; set; }

        /// <summary>
        /// 目标栈台  入库10,出库20,或者 A,B,C
        /// </summary>
        public string ToStation { get; set; }

        public int EnmTaskType { get; set; }

        public string MCTStation { get; set; }

        public string TaskDesc { get; set; }

       
        public int FromCellOID { get; set; }

        public int FromRow { get; set; }
        public int FromBay { get; set; }
        public int FromLevel { get; set; }

        public CellPosition FromCellPosition { get; set; }
        public CellPosition ToCellPosition { get; set; }

        public int ToCellOID { get; set; }

        public int ToRow { get; set; }

        public int ToBay { get; set; }
        public int ToLevel { get; set; }

        
        public int FromLocationID { get; set; }

        public int ToLocationID { get; set; }

        public bool IsInfeedDirection { get; set; }

        public bool IsOutfeedDirection { get; set; }

        public string Tag1 { get; set; }

        public string Tag2 { get; set; }

        public bool IsTimeOut { get; set; }

        public int Priority { get; set; }

        public string Description{get;set;}
        public int DestinationOID { get; set; }

        public string LastAlarmCode { get; set; }

        /// <summary>
        /// 从堆垛机反馈的执行结果代码 86:成功,1:重入库,2:无法入库,3:空取货,4:无法取货
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 货位OID
        /// </summary>
        public int FromLocationOID { get; set; }

        /// <summary>
        /// 货位OID
        /// </summary>
        public int ToLocationOID { get; set; }


    }
}
