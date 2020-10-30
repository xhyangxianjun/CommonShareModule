using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar.Entities
{
    /// <summary>
    /// 角色授权表
    /// </summary>
    public class RolePermissionInfo
    {
        public int OID { get; set; } = 0;
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsEnable { get; set; } = false;
        public bool IsVisible { get; set; } = false;
        public string GroupName { get; set; }
        public string ParentCode { get; set; }
        public string OptionType { get; set; }
        public string Description { get; set; }
        public int CodeLevel { get; set; } = 1;
        public string StartURL { get; set; }
        public DateTime LastActionTime { get; set; }
        public int PermissionOID { get; set; } = 0;
        public bool IsUse { get; set; } = false;
        /// <summary>
        /// 下一级集合
        /// </summary>
        //public List<RolePermissionInfo> ChildList = new List<RolePermissionInfo>();
    }
}
