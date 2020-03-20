using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Match_MCTTaskCounter")]
    public class Match_MCTTaskCounterInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public int MCTID { get; set; }

        public decimal AmountCounter { get; set; }

        public DateTime Task_LastActionTime { get; set; }

        public DateTime InfeedTask_LastActionTime { get; set; }

        public DateTime OutfeedTask_LastActionTime { get; set; }

    
        public int TaskCounter { get; set; }

     
        public DateTime TaskCounterTime { get; set; }

    
        public int CurrentTaskAmount { get; set; }
    

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

      
    }
}
