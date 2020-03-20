using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*-----------------------------------------------------------------------------------
 * 作者: AI Dept. 
 * 
 * 创建时间: 2017-02-27
 * 
 * 功能描述: 
 *      订单信息 
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class OrderInfo
    {
        /// <summary>
        /// 订单OID 自增号
        /// </summary>
        public long OrderOID { get; set; }

        /// <summary>
        /// 订单编号 
        /// </summary>
        public string OrderNO { get; set; }

        /// <summary>
        /// 订单对应任务类型
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 订单对应任务类型描述 
        /// </summary>
        public string TaskDesc { get; set; }

        /// <summary>
        /// 订单去向 对应出库货台
        /// </summary>
        public int DestinationOID { get; set; }

        /// <summary>
        /// 订单对应车牌号
        /// </summary>
        public string CarLicense { get; set; }

        /// <summary>
        /// 创建订单用户
        /// </summary>
        public int CreateUserOID { get; set; }

        /// <summary>
        /// 订单是否已审核
        /// </summary>
        public bool IsVerified { get; set; }
        /// <summary>
        /// 订单审核用户
        /// </summary>
        public int VerifiedUserOID { get; set; }

        /// <summary>
        /// 订单审核时间
        /// </summary>
        public DateTime VerifiedTime { get; set; }

        /// <summary>
        /// 订单是否拆解
        /// </summary>
        public bool IsUnPacked { get; set; }
        /// <summary>
        /// 订单拆解时间
        /// </summary>
        public DateTime UnPackedTime { get; set; }
        /// <summary>
        /// 订单拆解用户
        /// </summary>
        public int UnPackedUserOID { get; set; }
        /// <summary>
        /// 订单是否已创建任务
        /// </summary>
        public bool IsCreateTask { get; set; }

        /// <summary>
        /// 订单创建任务时间
        /// </summary>
        public DateTime CreateTaskTime { get; set; }
        /// <summary>
        /// 订单是否开始执行
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        /// 订单开始执行时间
        /// </summary>
        public DateTime RunningTime { get; set; }
        /// <summary>
        /// 订单是否已完成
        /// </summary>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// 订单完成时间
        /// </summary>
        public DateTime CompletedTime { get; set; }
        /// <summary>
        /// 订单计划时间
        /// </summary>
        public DateTime ScheduleTime { get; set; }

        /// <summary>
        /// 订单是否有效
        /// </summary>
        public bool IsInvalid { get; set; }
        /// <summary>
        /// 订单备注 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单上次修改时间
        /// </summary>
        public DateTime LastActionTime { get; set; }

        public string SrcOrderNO { get; set; }
    }
}
