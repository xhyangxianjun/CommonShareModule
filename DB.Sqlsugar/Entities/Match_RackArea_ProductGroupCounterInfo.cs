using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Match_RackArea_ProductGroupCounter")]
    public class Match_RackArea_ProductGroupCounterInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public long OID { get; set; }

        public long ProductGroupOID { get; set; }
        public int RackAreaOID { get; set; }
        public long AllocateCount { get; set; }
        public long RemainCount { get; set; }
        public DateTime CreateTime { get; set; }      

        public DateTime RackAreaAllocTime { get; set; }
    }
}
