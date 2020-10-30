using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class ShiftInfo
    {
        public int ShiftOID { get; set; }

        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }
    }
}
