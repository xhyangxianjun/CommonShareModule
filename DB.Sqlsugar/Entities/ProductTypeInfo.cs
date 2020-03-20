using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class ProductTypeInfo
    {
        public int ProductTypeOID { get; set; }

        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }
    }
}
