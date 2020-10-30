using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("RackArea")]
    public class RackAreaInfo
    {
        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int LaneWay { get; set; }

        public bool IsLocked { get; set; }

        public int RackOID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

      
        #endregion
    }
}
