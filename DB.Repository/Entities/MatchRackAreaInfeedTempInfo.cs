using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class MatchRackAreaInfeedTempInfo
    {
        public int RackAreaOID { get; set; }

        public int KPLValue { get; set; }
        public long TotalAllocateCount { get; set; }

        public long AllocateCount { get; set; }
        public long RemainCount { get; set; }

        public long RemainAveWeightCount { get; set; }
        public int CurrentTaskAmount { get; set; }
        public bool IsExceedMinLocations { get; set; }

        public DateTime ProductAllocTime { get; set; }

        public DateTime RackAreaAllocTime { get; set; }
        public int AreaPriority { get; set; }
        public decimal AreaWeightValue { get; set; }
        public string ProductAllocSpanTime { get; set; }
        public int Station { get; set; }
        public int PLCDestValue { get; set; }
        public bool AreaIsLock { get; set; }
        public bool AreaInfeedIsLock { get; set; }
        public int StationArea { get; set; }
    }
}
