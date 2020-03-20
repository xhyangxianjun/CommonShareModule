using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Match_RackArea_Counter")]
   public  class Match_RackArea_CounterInfo
    {
        /// <summary>
        /// 自增号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public long OID { get; set; }
        public int RackAreaOID { get; set; }

        public int SortID { get; set; }

        public long TotalAmount { get; set; }
        public int WeightValue { get; set; }
        public int CurrentValue { get; set; }

        public DateTime AlternateTime { get; set; }

        public DateTime OutfeedAlternateTime { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }
        public DateTime UnPacked_LastActionTime { get; set; }

    }
}
