using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("RackStation")]
    public class RackStationInfo
    {

        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public int Station { get; set; }

        public string StationDesc { get; set; }

        public string MCTStation { get; set; }

        public int PLCValue { get; set; }

        public int RackAreaOID { get; set; }

        public int StationAreaOID { get; set; }

        public bool IsAllowInfeed { get; set; }

        public int ToStation { get; set; }

        public bool IsAllowOutfeed { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime SendMessageTime { get; set; }
        
        #endregion
    }
}
