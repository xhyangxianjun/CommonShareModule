using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    [SugarTable("WCS_USER")]
    public class WCSUserInfo
    {

        #region Public Properties
        [SugarColumn(IsPrimaryKey = true, ColumnName = "USERID")]
        public string OID { get; set; }

        public string PASSWORD { get; set; }

        public string USERNO { get; set; }

        public string USERNAME { get; set; }

        public string ROLEID { get; set; }

        //public string SFLBHead { get; set; }

        //public bool IsEnable { get; set; }

        //public DateTime CreateTime { get; set; }

        //public DateTime LastActionTime { get; set; }


        #endregion


    }
}
