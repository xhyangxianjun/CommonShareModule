using Chaint.Data.SqlSugar.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Data.SqlSugar
{
    public class RackManager : DbContext<RackLocationInfo>
    {
        //分页
            //var Bom_page = manage.Db.Queryable<StudentEntity>().GroupBy(x => x.Name).Skip(2 * 2).Take(2).ToList();
        //聚合
        //var Bom_sum = Bom_list.GroupBy(x => x.Name).Skip(2 * 2).Take(2).ToList().Select(x => new { name = x.Key, count = x.Count() }).ToList();
        public SimpleClient<CellGroup> CellGroupDb { get { return new SimpleClient<CellGroup>(Db); } }//用来处理CellGroup表的常用操作
        public AAResultInfo SearchLanewayByCellGroupOID(int stationAreaType, long cellGroupOID, int[] availableLaneWays)
        {
            AAResultInfo retInfo = new AAResultInfo();
            retInfo.RetCode = "-1";

            //long ProductGroupOID = CellGroupDb.AsQueryable().Where(it=>it.OID==cellGroupOID).First().ProductGroupOID;
            long ProductGroupOID = CellGroupDb.AsQueryable().InSingle(cellGroupOID).ProductGroupOID;

            string strFilterRule = GetSqlFilterStringByFilterRule(new int[2] { 1,2}, cellGroupOID);

            var ExcludeLocations = Db.Queryable<RackLocationInfo, RackCellInfo, RackContainerInfo>((location, cell, container) => new object[] {
              JoinType.Inner,location.RackCellOID==cell.OID,
              JoinType.Left,location.OID==container.LocationOID
            }).Where((location,cell, container) => location.Location == 1 && SqlFunc.IsNullOrEmpty(container.LocationOID))
            .GroupBy((location, cell, container) => cell.OID)
            .Select((location, cell, container) => new ExcludeLocationsInfo { CellOID = cell.OID, IsExistsProduct = "1" });

            var availableRackAreaTemp = Db.Queryable<RackLocationInfo, RackCellInfo, RackBunchInfo, RackSideInfo, RackAreaInfo, 
                RackContainerInfo, CellGroup, Enable_RackAreaInfo, Enable_RackArea_InfeedInfo, Match_RackCell_SearchInfeedInfo>(
                (st, st1, st2, st3, st4, st5, st6, st7, st8, st9) => new object[] {
              JoinType.Inner,st.RackCellOID==st1.OID,
              JoinType.Inner,st1.RackBunchOID==st2.OID,
              JoinType.Inner,st2.RackSideOID==st3.OID,
              JoinType.Inner,st3.RackAreaOID==st4.OID,
              JoinType.Left,st.OID==st5.LocationOID,
              JoinType.Left,st5.CellGroupOID==st6.OID,
              JoinType.Left,st4.OID==st7.RackAreaOID,
              JoinType.Left,st4.OID==st8.RackAreaOID,
              JoinType.Left,st.OID==st9.LocationOID
            }).Where((st, st1, st2, st3, st4, st5, st6, st7, st8, st9) => SqlFunc.IsNull(st5.LocationOID, 0) == 0 && SqlFunc.IsNull<bool>(st7.IsLocked, false) == false 
            && SqlFunc.IsNull<bool>(st8.IsLocked, false) == false && SqlFunc.IsNull(st9.LocationOID, 0) == 0 && availableLaneWays.Contains(st4.LaneWay))
            .Select((st, st1, st2, st3, st4, st5, st6, st7, st8, st9) => new AvailableRackAreaTempInfo {RackCellOID=st.RackCellOID,RackAreaOID = st4.OID});

            var availableRackArea = Db.Queryable(availableRackAreaTemp, ExcludeLocations, JoinType.Left, (availableTemp, excude) => availableTemp.RackCellOID == excude.CellOID)
                .GroupBy((availableTemp, excude) => availableTemp.RackAreaOID).Select(
                (availableTemp, excude) => new AvailableRackAreaInfo { RackAreaOID = availableTemp.RackAreaOID, AvailLocationCount = SqlFunc.AggregateCount(availableTemp.RackAreaOID) });

            var ProductGroupCounter = Db.Queryable<Match_RackArea_ProductGroupCounterInfo>().Where(it => it.ProductGroupOID == ProductGroupOID).Select(it => new MatchProductGroupCounterInfo
            { ProductGroupOID = it.ProductGroupOID, RackAreaOID = it.RackAreaOID, AllocateCount = it.AllocateCount, RemainCount = it.RemainCount, ProductAllocTime = it.RackAreaAllocTime,
              ProductAllocSpanTime = Convert.ToString(it.RackAreaAllocTime) });

            var matchRackAreaTemp= Db.Queryable(availableRackArea, ProductGroupCounter, JoinType.Left, (available, ProductGroup) => available.RackAreaOID == ProductGroup.RackAreaOID).Select(
                (available, ProductGroup) =>new MatchRackAreaInfeedInfo{ RackAreaOID= available.RackAreaOID, AvailLocationCount= available.AvailLocationCount, TotalAllocateCount= ProductGroup.AllocateCount,
                    ProductAllocTime = ProductGroup.ProductAllocTime,ProductAllocSpanTime= ProductGroup.ProductAllocSpanTime });

            var matchRackAreaInfeedTemp = Db.Queryable< RackAreaInfo, RackStationInfo, RackStation_AreaInfo, Enable_RackAreaInfo, Enable_RackArea_InfeedInfo, Match_RackArea_CounterInfo, Match_RackArea_WeightInfo,
                Match_MCTTaskCounterInfo >((info, info1, info2, info3, info4, counter,weighter, info5) => new object[] {
              JoinType.Left,info.OID==info1.RackAreaOID,
              JoinType.Left,info1.StationAreaOID==info2.OID,
              JoinType.Left,info3.RackAreaOID==info.OID,
              JoinType.Left,info4.RackAreaOID==info.OID,
              JoinType.Left,counter.RackAreaOID==info.OID,
              JoinType.Left,weighter.RackAreaOID==info.OID,
              JoinType.Left,info5.MCTID==info.LaneWay
            }).Select((info, info1, info2, info3, info4, counter, weighter, info5) =>new MatchRackAreaInfeedTempInfo {RackAreaOID=info.OID, AreaWeightValue=weighter.AreaWeightValue ,CurrentTaskAmount=info5.CurrentTaskAmount,
            KPLValue=weighter.KPLValue,RackAreaAllocTime=counter.AlternateTime,AreaPriority=weighter.AreaPriority,Station=info1.Station,PLCDestValue=info1.PLCValue,AreaIsLock=info3.IsLocked,AreaInfeedIsLock=info4.IsLocked,
            StationArea=info2.StationArea});

            var matchRackAreaInfeed = Db.Queryable(matchRackAreaTemp, matchRackAreaInfeedTemp, JoinType.Left, (j1, j2) => j1.RackAreaOID == j2.RackAreaOID).
                Where((j1,j2) => j1.AvailLocationCount > 0 && SqlFunc.IsNull<bool>(j2.AreaIsLock, false) == false && SqlFunc.IsNull<bool>(j2.AreaInfeedIsLock, false) == false && j2.StationArea == stationAreaType)
                .Select((j1,j2)=>new MatchRackAreaInfeedInfo{
                RackAreaOID=j1.RackAreaOID,Station=j2.Station,AreaIsLock=j2.AreaIsLock,AreaInfeedIsLock=j2.AreaInfeedIsLock,StationArea=j2.StationArea}).ToList();
            
            //string strCMDSQL = SearchCellSQL.GenerateMatchAreaCommandSQL(ProductGroupOID, strFilterRule, strOrder, availableLaneWays, stationAreaType);

            //if (findRackAreaRow == null)
            //{
            //    retInfo.RetCode = "03";
            //    retInfo.RetMessage = string.Format("No available roadway, please check,CellGroupOID:{0}", cellGroupOID);
            //}
            //else
            //{
            //    retInfo.RetCode = "00";
            //    retInfo.RackAreaOID = DataTypeConverter.GetIntValue(findRackAreaRow["RackAreaOID"]);
            //    // retInfo.RetDest = ServiceContext.Get_SP_PLCDest(retInfo.RackAreaOID);
            //    retInfo.RetDest = DataTypeConverter.GetIntValue(findRackAreaRow["PLCDestValue"]);
            //    retInfo.Station = DataTypeConverter.GetIntValue(findRackAreaRow["Station"]);
            //    retInfo.CellGroupOID = cellGroupOID;
            //    retInfo.RetMessage = string.Format("Allocate a new roadway,CellGroupOID({0}),Roadway ID({1}),PlcDest({2}),Station({3})", cellGroupOID, retInfo.RackAreaOID, retInfo.RetDest, retInfo.Station);

            //    //保存巷道分配结果
            //    DBAccess.Instance.Match_RackArea_SearchInfeed_InsertOrUpdate(retInfo.RackAreaOID, cellGroupOID);

            //    //保存巷道分配记录
            //    DBAccess.Instance.Match_RackArea_ProductGroupCounter_InsertOrUpdate(retInfo.RackAreaOID, ProductGroupOID);

            //    //更新巷道分配时间
            //    DBAccess.Instance.Update_Match_RackArea_Counter(retInfo.RackAreaOID);

            //    //增加某种分类的数量
            //    DBAccess.Instance.UpdateMatchRackAreaProductGroupCountByRackAreaAndProductGroupOID(retInfo.RackAreaOID, ProductGroupOID);

            //}
            return retInfo;
        }
        public string GetSqlFilterStringByFilterRule(int[] rackOIDs, long cellGroupOID)
        {
            return "";
        }
    }
}
