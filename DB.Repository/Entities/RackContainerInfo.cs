using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("RackContainer")]
    public class RackContainerInfo
    {

        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public long OID { get; set; }

        public int LocationOID { get; set; }

        public long CellGroupOID { get; set; }

        public string OrganizationCode { get; set; }

        public string WarehouseCode { get; set; }

        public int Amount { get; set; }

        public string Remark { get; set; }

        public bool IsExist { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

       
        #endregion
        
    }
}
