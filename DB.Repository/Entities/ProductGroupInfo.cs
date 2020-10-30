using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    /// <summary>
    /// <para>ProductGroup Object</para>
    /// <para>Summary description for ProductGroup.</para>
    /// <para><see cref="member"/></para>
    /// <remarks></remarks>
    /// </summary>
    [Serializable]
    [SugarTable("ProductGroup")]
    public class ProductGroupInfo
    {
        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "OID")]
        public int OID { get; set; }

        public int ActionCount { get; set; }

        public string MaterialCode { get; set; }

        public string CustomerCode { get; set; }

        public string OrderNO { get; set; }

        public string PackType { get; set; }

        public string ProductType { get; set; }

        public string PalletRawMaterial { get; set; }

        public string Specification { get; set; }

        public string Cdefine1 { get; set; }

        public string Cdefine2 { get; set; }

        public string Cdefine3 { get; set; }

        public string Cdefine4 { get; set; }

        public string Cdefine5 { get; set; }

        public string Cdefine6 { get; set; }

        public string Cdefine7 { get; set; }

        public string Cdefine8 { get; set; }

        public string Cdefine9 { get; set; }

        public string Cdefine10 { get; set; }

        public string Cdefine11 { get; set; }

        public string Cdefine12 { get; set; }

        public string Cdefine13 { get; set; }

        public string Cdefine14 { get; set; }

        public string Cdefine15 { get; set; }

        public decimal Udefine1 { get; set; }

        public decimal Udefine2 { get; set; }

        public decimal Udefine3 { get; set; }

        public decimal Udefine4 { get; set; }

        public decimal Udefine5 { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }

        #endregion
    }
}
