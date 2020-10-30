using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2017-02-16
 * 
 * 功能描述: 
 *      堆垛机反馈的实时信息，包含实时状态、位置、执行的任务等
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Data.SqlSugar.Entities
{
    public class StackerInfo
    {
        /// <summary>
        /// 任务ID   对应TashCacheOID
        /// </summary>
        public long TaskID { get; set; }

        /// <summary>
        /// 任务类型代码 1:入库,2:出库,3:盘库，4:栈台搬运,5：货位移动，6:回栈台
        /// </summary>
        public int TaskTypeCode { get; set; }
        
        /// <summary>
        /// 堆垛机编号/巷道号 1,2,3,4,5
        /// </summary>
        public int StackerID { get; set; }

        /// <summary>
        /// 取货货台
        /// </summary>
        public int FromStation { get; set; }
        /// <summary>
        /// 放货货台
        /// </summary>
        public int ToStation { get; set; }

        /// <summary>
        /// 起始排/取货排
        /// </summary>
        public int FromRow { get; set; }
        /// <summary>
        /// 起始列/取货列
        /// </summary>
        public int FromBay { get; set; }
        /// <summary>
        /// 起始层/取货层
        /// </summary>
        public int FromLevel { get; set; }

        /// <summary>
        /// 起始货位/取货货位
        /// </summary>
        public int FromLocation { get; set; }

        /// <summary>
        /// 目标排/放货货排
        /// </summary>
        public int ToRow { get; set; }

        /// <summary>
        /// 目标列/放货列
        /// </summary>
        public int ToBay { get; set; }
        /// <summary>
        /// 目标层/放货层
        /// </summary>
        public int ToLevel { get; set; }
        /// <summary>
        /// 目标货位/货货货位
        /// </summary>
        public int ToLocation { get; set; }


        /// <summary>
        /// 盘库排/放货货排
        /// </summary>
        public int FromCheckRow { get; set; }

        /// <summary>
        /// 盘库列/放货列
        /// </summary>
        public int FromCheckBay { get; set; }
        /// <summary>
        /// 盘库层
        /// </summary>
        public int FromCheckLevel { get; set; }
        /// <summary>
        /// 盘库货位
        /// </summary>
        public int FromCheckLocation { get; set; }


        /// <summary>
        /// 盘库排/放货货排
        /// </summary>
        public int ToCheckRow { get; set; }

        /// <summary>
        /// 盘库列/放货列
        /// </summary>
        public int ToCheckBay { get; set; }


        /// <summary>
        /// 盘库层
        /// </summary>
        public int ToCheckLevel { get; set; }

        /// <summary>
        /// 盘库货位
        /// </summary>
        public int ToCheckLocation { get; set; }

        /// <summary>
        /// 任务执行请求   WMS-->Stacker
        /// </summary>
        public int TaskExecuteRequest { get; set; }

        /// <summary>
        /// 任务完成请求响应 WMS-->Stacker
        /// </summary>
        public int TaskFinishRequestAck { get; set; }


        /// <summary>
        /// 备用标签1
        /// </summary>
        public int Tag1 { get; set; }

        /// <summary>
        /// 备用标签2
        /// </summary>
        public int Tag2 { get; set; }

        /// <summary>
        /// 堆垛机是否可用
        /// </summary>
        public int IsEnable { get; set; }

        /// <summary>
        /// 任务执行状态 1:取货完成,2：放货完成,99:任务完成
        /// </summary>
        public int TaskExeStep { get; set; }

   
        /// <summary>
        /// 设备运行状态  0:空闲,1:运行,2:待命，2:故障,4:钥匙手动
        /// </summary>
        public int DeviceStatus { get; set; }

        /// <summary>
        /// 设备运行模式 
        /// 1：正在入库，2：正在出库，3：正在盘库，4：站台搬运，6：回栈台(只有设备状态为运行状态时）
        /// </summary>
        public int DeviceRunMode { get; set; }



        /// <summary>
        /// 当前排
        /// </summary>
        public int CurrRow { get; set; }

        /// <summary>
        /// 当前列
        /// </summary>
        public int CurrBay { get; set; }

        /// <summary>
        /// 当前层
        /// </summary>
        public int CurrLevel { get; set; }

        /// <summary>
        /// 水平X坐标
        /// </summary>
        public decimal CurrPosX { get; set; }

        /// <summary>
        /// 垂直Y坐标
        /// </summary>
        public decimal CurrPosY { get; set; }

        /// <summary>
        /// 叉臂行走Z坐标
        /// </summary>
        public decimal CurrPosZ { get; set; }

        /// <summary>
        /// 任务执行结果代码  86:任务执行成功,1:重入库,2:无法放货，3:空取货,4：无法取货，10:其他异常
        /// 
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// 运行结果消息
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 任务完成请求
        /// Stacker-->WMS  86
        /// </summary>
        public int TaskFinishRequest { get; set; }

        /// <summary>
        /// 水平运行方向
        /// </summary>
        public int HRunDirection { get; set; }

        /// <summary>
        /// 垂直运行方向
        /// </summary>
        public int VRunDirection { get; set; }

        /// <summary>
        /// 叉臂运行方向
        /// </summary>
        public int FRunDirection { get; set; }


        /// <summary>
        /// 系统故障复位 WMS-->堆垛机
        /// </summary>
        public int SystemReset { get; set; }

        /// <summary>
        /// 系统急停 WMS-->堆垛机
        /// </summary>
        public int SurgentStop { get; set; }

        /// <summary>
        /// 重新执行任务 WMS-->堆垛机
        /// </summary>
        public int ReExecutive { get; set; }

        /// <summary>
        /// 任务清除 WMS-->堆垛机
        /// </summary>
        public int TaskClear { get; set; }

        /// <summary>
        /// 当前任务货台
        /// </summary>
        public int CurrTaskStation { get; set; }

        /// <summary>
        /// 当前货台
        /// </summary>
        public int CurrStation { get; set; }

        /// <summary>
        /// 堆垛机报警 WMS---堆垛机
        /// </summary>
        public int Alarm { get; set; }

        /// <summary>
        /// 远程启动 新增
        /// </summary>
        public int RemoteStart { get; set; }

        /// <summary>
        /// 备用1
        /// </summary>
        public int Spare1 { get; set; }


        /// <summary>
        /// 备用2
        /// </summary>
        public int Spare2 { get; set; }

        /// <summary>
        /// 备用3
        /// </summary>
        public int Spare3 { get; set; }

        /// <summary>
        /// 备用4
        /// </summary>
        public int Spare4 { get; set; }



        


        /// <summary>
        /// 上次运行时间
        /// </summary>
        public DateTime LastActionTime { get; set; }


    }
}
