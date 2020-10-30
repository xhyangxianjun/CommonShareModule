using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("Rack")]
    public class RackInfo
    {
        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 是否精确匹配到货位
        /// </summary>
        public bool IsAccurateMatch { get; set; }


        public string RackCode { get; set; }

        public string OrganizationCode { get; set; }

        public string WarehouseCode { get; set; }

        public int RowAmount { get; set; }

        public int BayAmount { get; set; }

        public int LevelAmount { get; set; }

        public int LanewayAmount { get; set; }

        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }
}
