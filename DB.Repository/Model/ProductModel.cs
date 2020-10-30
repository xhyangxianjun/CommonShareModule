using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Repository.Model
{
    //如果实体类名称和表名不一致可以加上SugarTable特性指定表名
    [SugarTable("Product")]
    public class ProductModel
    {
        //指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        [SugarColumn(ColumnName="OID",IsPrimaryKey = true, IsIdentity = true)]
        public long ProductOID { get; set; }

        /// <summary>
        /// 来源于 WCS 的产品OnlyID
        /// </summary>
        public long OnlyID { get; set; } = 0;

        /// <summary>
        /// 产品编号  如没有单件产品，可用托盘号代替
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string WLTM { get; set; }


        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNO { get; set; }

        /// <summary>
        /// 产品分类信息
        /// </summary>
        public long ProductGroupOID { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }


        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 机台编号
        /// </summary>
        public string MachineID { get; set; }


        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProduceDate { get; set; }


        /// <summary>
        /// 一托内或者一组内产品数量，默认为1
        /// </summary>
        public decimal Amount { get; set; } = 1;

        /// <summary>
        /// 产品规格代码
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 产品包装类型
        /// </summary>
        public string PackType { get; set; }


        public string SKU { get; set; }

        /// <summary>
        /// 产品宽度( 托盘)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 产品长度( 托盘)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 产品高度( 托盘)
        /// </summary>
        public int Height { get; set; }


        /// <summary>
        /// 产品重量( 托盘)
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 产品类型 成品托盘,空托盘,余盘
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 客户信息
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 生产订单
        /// </summary>
        public string OrderNO { get; set; }

        /// <summary>
        /// 产品等级 
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 产品单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 托盘原材料
        /// </summary>
        public string PalletRawMaterial { get; set; }

        /// <summary>
        /// 托盘颜色
        /// </summary>
        public string PalletColor { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }


        /// <summary>
        /// 生产班组
        /// </summary>
        public string ProduceShift { get; set; }


        /// <summary>
        /// 质检员
        /// </summary>
        public string Inspector { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 插入还是删除，与PR消息对应
        /// </summary>
        public bool UpdateFlag { get; set; }

        /// <summary>
        /// 仓库组织代码  王新增
        /// </summary>
        public string WHOrgCode { get; set; }

        /// <summary>
        /// 仓库代码 王新增
        /// </summary>
        public string WHCode { get; set; }


        public decimal LabelWidth { get; set; }

        public decimal LabelDiameter { get; set; }

        public decimal LabelLength { get; set; }

        public decimal Basisweight { get; set; }

        public int Splice { get; set; }

        public decimal LabelWeight { get; set; }

        public decimal CalcWeight { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal NetWeight { get; set; }

        public string WeightMode { get; set; }

        public int Layers { get; set; }

        public string CombineCode { get; set; }

        /// <summary>
        /// 产品状态 0,10,11,12,20,21,22
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 产品前状态 
        /// </summary>

        public int PreState { get; set; }

        /// <summary>
        /// 保质日期
        /// </summary>
        public DateTime ExpireDateTime { get; set; }

        /// <summary>
        /// 保质天数
        /// </summary>
        public int ExpireDays { get; set; }

        /// <summary>
        /// 商标
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// 产品标准
        /// </summary>
        public string ProductStandard { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string OrderCode { get; set; }

        public string OrderItem { get; set; }

        public string ProductCode { get; set; }

        public string ArticleCode { get; set; }

        public string Classification { get; set; }

        /// <summary>
        /// 复卷日期  YYYYMMDD
        /// </summary>
        public string WoundDate { get; set; }

        /// <summary>
        /// 去向
        /// </summary>
        public string Destination { get; set; }

        #region 备用字段

        /// <summary>
        /// 备用1
        /// </summary>
        public string Cdefine1 { get; set; }


        /// <summary>
        /// 备用2
        /// </summary>
        public string Cdefine2 { get; set; }


        /// <summary>
        /// 备用3
        /// </summary>
        public string Cdefine3 { get; set; }


        /// <summary>
        /// 备用4
        /// </summary>
        public string Cdefine4 { get; set; }

        /// <summary>
        /// 备用5
        /// </summary>
        public string Cdefine5 { get; set; }

        /// <summary>
        /// 备用6
        /// </summary>
        public string Cdefine6 { get; set; }

        /// <summary>
        /// 备用7
        /// </summary>
        public string Cdefine7 { get; set; }

        /// <summary>
        /// 备用8
        /// </summary>
        public string Cdefine8 { get; set; }

        /// <summary>
        /// 备用9
        /// </summary>
        public string Cdefine9 { get; set; }

        /// <summary>
        /// 备用10
        /// </summary>
        public string Cdefine10 { get; set; }


        /// <summary>
        /// 备用11
        /// </summary>
        public string Cdefine11 { get; set; }


        /// <summary>
        /// 备用12
        /// </summary>
        public string Cdefine12 { get; set; }


        /// <summary>
        /// 备用13
        /// </summary>
        public string Cdefine13 { get; set; }


        /// <summary>
        /// 备用14
        /// </summary>
        public string Cdefine14 { get; set; }


        /// <summary>
        /// 备用15
        /// </summary>
        public string Cdefine15 { get; set; }

        /// <summary>
        /// 备用16
        /// </summary>
        public string Cdefine16 { get; set; }

        /// <summary>
        /// 备用17
        /// </summary>
        public string Cdefine17 { get; set; }




        /// <summary>
        /// 备用18
        /// </summary>
        public string Cdefine18 { get; set; }



        /// <summary>
        /// 备用19
        /// </summary>
        public string Cdefine19 { get; set; }

        /// <summary>
        /// 备用20
        /// </summary>
        public string Cdefine20 { get; set; }


        /// <summary>
        /// 备用21
        /// </summary>
        public string Cdefine21 { get; set; }


        /// <summary>
        /// 备用22
        /// </summary>
        public string Cdefine22 { get; set; }


        /// <summary>
        /// 备用23
        /// </summary>
        public string Cdefine23 { get; set; }


        /// <summary>
        /// 备用24
        /// </summary>
        public string Cdefine24 { get; set; }

        /// <summary>
        /// 备用25
        /// </summary>
        public string Cdefine25 { get; set; }



        /// <summary>
        /// 备用1 数字
        /// </summary>
        public Decimal Udefine1 { get; set; }


        /// <summary>
        /// 备用2 数字
        /// </summary>
        public Decimal Udefine2 { get; set; }


        /// <summary>
        /// 备用3 数字
        /// </summary>
        public Decimal Udefine3 { get; set; }


        /// <summary>
        /// 备用4 数字
        /// </summary>
        public Decimal Udefine4 { get; set; }

        /// <summary>
        /// 备用5 数字
        /// </summary>
        public Decimal Udefine5 { get; set; }

        /// <summary>
        /// 备用6 数字
        /// </summary>
        public Decimal Udefine6 { get; set; }

        /// <summary>
        /// 备用7 数字
        /// </summary>
        public Decimal Udefine7 { get; set; }

        /// <summary>
        /// 备用8 数字
        /// </summary>
        public Decimal Udefine8 { get; set; }

        /// <summary>
        /// 备用9 数字
        /// </summary>
        public Decimal Udefine9 { get; set; }

        /// <summary>
        /// 备用10 数字
        /// </summary>
        public Decimal Udefine10 { get; set; }

        #endregion
    }
}
