using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Match_RackCell_SearchInfeed")]
    public class Match_RackCell_SearchInfeedInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public long OID { get; set; }
        public long CellGroupOID { get; set; }
        public int LocationOID { get; set; }

        public int CellOID { get; set; }
        public int LocationID { get; set; }
        public int MCID { get; set; }
        public string TaskType { get; set; }
        public string Tag2 { get; set; }
        public string TaskDesc { get; set; }
        public string Tag1 { get; set; }

        public string Description { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }



    }
}
