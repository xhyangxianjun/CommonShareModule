using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    public class LogScanningInfo
    {
        #region Public Properties

        public long OID { get; set; }

        public string ProductID { get; set; }

        public int ScanSignal { get; set; }

        public string Position { get; set; }

        public decimal Width { get; set; }

        public decimal Length { get; set; }

        public bool IsAutoScan { get; set; }

        public DateTime CreateTime { get; set; }


        #endregion
    }
}
