// File: UserInfo.cs
// 2017-5-14: Chaint    Original Version
// 
// ===================================================================

using System;

namespace Chaint.Data.SqlSugar.Entities
{
    /// <summary>
    /// <para>UserInfo Object</para>
    /// <para>Summary description for UserInfo.</para>
    /// <para><see cref="member"/></para>
    /// <remarks></remarks>
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        #region Fields
        private int m_oID=0;
        private string m_name = String.Empty;
        private string m_password = String.Empty;
        private string m_loginName = String.Empty;
        private string m_emailAdress = String.Empty;
        private DateTime m_createTime;
        private DateTime m_lastLoginTime;
        private DateTime m_lastActionTime;
        private DateTime m_lastLockTime;
        private DateTime m_lastChangePasswordTime;
        private bool m_isOnline=false;
        private bool m_isEnableChangePassword;
        private bool m_isGrantPermission;
        private int m_userShiftOID;
        private int m_RoleOID;
        #endregion

        #region Contructors
        public UserInfo()
        {
        }

        public UserInfo
        (
            int oID,
            string name,
            string password,
            string loginName,
            string emailAdress,
            DateTime createTime,
            DateTime lastLoginTime,
            DateTime lastActionTime,
            DateTime lastLockTime,
            DateTime lastChangePasswordTime,
            bool isOnline,
            bool isEnableChangePassword,
            bool isGrantPermission,
            int userShiftOID,
            int optimisticLockField,
            int userRoleOID
        )
        {
            m_oID = oID;
            m_name = name;
            m_password = password;
            m_loginName = loginName;
            m_emailAdress = emailAdress;
            m_createTime = createTime;
            m_lastLoginTime = lastLoginTime;
            m_lastActionTime = lastActionTime;
            m_lastLockTime = lastLockTime;
            m_lastChangePasswordTime = lastChangePasswordTime;
            m_isOnline = isOnline;
            m_isEnableChangePassword = isEnableChangePassword;
            m_isGrantPermission = isGrantPermission;
            m_userShiftOID = userShiftOID;
            m_RoleOID = userRoleOID;

        }
        #endregion

        #region Public Properties

        public int OID
        {
            get { return m_oID; }
            set { m_oID = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        public string LoginName
        {
            get { return m_loginName; }
            set { m_loginName = value; }
        }

        public string EmailAdress
        {
            get { return m_emailAdress; }
            set { m_emailAdress = value; }
        }

        public DateTime CreateTime
        {
            get { return m_createTime; }
            set { m_createTime = value; }
        }

        public DateTime LastLoginTime
        {
            get { return m_lastLoginTime; }
            set { m_lastLoginTime = value; }
        }

        public DateTime LastActionTime
        {
            get { return m_lastActionTime; }
            set { m_lastActionTime = value; }
        }

        public DateTime LastLockTime
        {
            get { return m_lastLockTime; }
            set { m_lastLockTime = value; }
        }

        public DateTime LastChangePasswordTime
        {
            get { return m_lastChangePasswordTime; }
            set { m_lastChangePasswordTime = value; }
        }

        public bool IsOnline
        {
            get { return m_isOnline; }
            set { m_isOnline = value; }
        }

        public bool IsEnableChangePassword
        {
            get { return m_isEnableChangePassword; }
            set { m_isEnableChangePassword = value; }
        }

        public bool IsGrantPermission
        {
            get { return m_isGrantPermission; }
            set { m_isGrantPermission = value; }
        }

        public int UserShiftOID
        {
            get { return m_userShiftOID; }
            set { m_userShiftOID = value; }
        }


        public int RoleOID
        {
            get { return m_RoleOID; }
            set { m_RoleOID = value; }
        }

        string m_EmployeePK = "";
        public string EmployeePK
        {
            get { return m_EmployeePK; }
            set { m_EmployeePK = value; }
        }

        #endregion

        public string Code { get; set; }
        public string RoleCode { get; set; }
        public bool IsChangeRole { get; set; } = false;
        public string RoleName { get; set; }
        public bool IsEnable { get; set; } = true;
        public int LoginCount { get; set; } = 0;
    }
}