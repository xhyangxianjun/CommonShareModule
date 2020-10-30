using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("RackCell")]
    public class RackCellInfo
    {

        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int Row { get; set; }

        public int Bay { get; set; }

        public int Level { get; set; }

        public bool IsLocked { get; set; }

        public int LocationOID { get; set; }

        public int RackBunchOID { get; set; }

        public int RackOID { get; set; }

        public int FilterSystemOID { get; set; }

        public int FilterNormalOID { get; set; }

        public int CellPriorityOID { get; set; }

        public int CellStatusOID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

   
        #endregion
    }
}
