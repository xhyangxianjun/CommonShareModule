using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class OptionInfo
    {
        public int OID { get; set; } = 0;
        public string Code { get; set;}
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string ParentCode { get; set; }
        public string OptionType { get; set;}
        public string Description { get; set; }
        public int CodeLevel { get; set; } = 1;
        public string StartURL { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastActionTime { get; set; }
    }
}
