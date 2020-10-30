using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class ShiftSlotInfo
    {
        public int ShiftSlotOID { get; set; }

        public string ShiftSlotCode { get; set; }
        public string ShiftSlotName { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime LastActionTime { get; set; }
    }
}
