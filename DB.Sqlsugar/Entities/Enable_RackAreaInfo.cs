using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Enable_RackArea")]   
    public class Enable_RackAreaInfo
    {
        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public int RackAreaOID { get; set; }

        public bool IsLocked { get; set; }

        public string Description { get; set; }
     

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        
        #endregion
    }
}
