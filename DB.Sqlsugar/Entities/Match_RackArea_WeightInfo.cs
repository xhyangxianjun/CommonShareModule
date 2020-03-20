using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Match_RackArea_Weight")]
    public class Match_RackArea_WeightInfo
    {
        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public int RackAreaOID { get; set; }

        public decimal WidthMax { get; set; }

        public decimal WeightMax { get; set; }

        public int AreaPriority { get; set; }        

        public decimal AreaWeightValue { get; set; }

        public int KPLValue { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastActionTime { get; set; }
        #endregion
    }
}
