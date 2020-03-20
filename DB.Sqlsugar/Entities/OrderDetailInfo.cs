using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2017-02-27
 * 
 * 功能描述: 
 *      订单分录信息 
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class OrderDetailInfo
    {
        /// <summary>
        /// 当前订单分录 自增号
        /// </summary>
        public long OrderDetailOID { get; set; }

        /// <summary>
        /// 当前订单分录 对应订单ID
        /// </summary>
        public long OrderOID { get; set; }

        /// <summary>
        /// 当前订单分录 ID
        /// </summary>
        public int EntryID { get; set; }

        /// <summary>
        /// 当前订单分录 搜索条件
        /// </summary>
        public string FilterString { get; set; }

        /// <summary>
        /// 当前订单分录 去向
        /// </summary>
        public int DestinationOID { get; set; }

        /// <summary>
        /// 当前订单分录是否已拆解
        /// </summary>
        public bool IsUnPacked { get; set; }

        /// <summary>
        /// 当前订单分录拆解时间
        /// </summary>
        public DateTime UnPackedTime { get; set; }

        /// <summary>
        /// 当前订单分录是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 当前订单分录开始运行时间
        /// </summary>
        public DateTime RunningTime { get; set; }

        /// <summary>
        /// 当前订单分录是否完成
        /// </summary>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// 当前订单分录完成时间
        /// </summary>

        public DateTime CompletedTime { get; set; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal ScheduleAmount { get; set; }
        /// <summary>
        /// 计划重量
        /// </summary>
        public decimal ScheduleWeight { get; set; }

        /// <summary>
        /// 拆解重量
        /// </summary>
        public decimal UnPackedAmount { get; set; }
        /// <summary>
        /// 拆解重量
        /// </summary>
        public decimal UnPackedWeight { get; set; }
        /// <summary>
        /// 当前仓库余量 数量
        /// </summary>
        public decimal CurrentAmount { get; set; }
        /// <summary>
        /// 当前仓库余量 重量
        /// </summary>
        public decimal CurrentWeight { get; set; }

        /// <summary>
        /// 分录备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单分录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单分录上次修改时间
        /// </summary>
        public DateTime LastActionTime { get; set; }

    }
}
