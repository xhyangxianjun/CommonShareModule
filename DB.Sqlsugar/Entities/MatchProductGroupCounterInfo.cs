using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class MatchProductGroupCounterInfo
    {
        public long ProductGroupOID { get; set; }
        public int RackAreaOID { get; set; }
        public long AllocateCount { get; set; }
        public long RemainCount { get; set; }
        public DateTime ProductAllocTime { get; set; }
        public string ProductAllocSpanTime { get; set; }
    }
}
