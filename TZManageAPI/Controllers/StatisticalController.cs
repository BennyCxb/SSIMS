using BLL;
using BT.Manage.Frame.Base;
using BT.Manage.Model;
using BT.Manage.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using TZBaseFrame.Attributes;
using TZManageAPI.Base;
using BT.Manage.Core;
using FLow;
using TZManageAPI.Common;
using BT.Manage.Tools.Utils;
using BT.Manage.Verification;
using System.Net.Http;
using TZBaseFrame;
using System.IO;
using System.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace TZManageAPI.Controllers
{
    /// <summary>
    /// 统计控制器
    /// </summary>
    [JwtAuthActionFilter]
    public class StatisticalController : BaseController
    {
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetQuestionStatusByAgency()
        {

            var dt = ModelOpretion.SearchDataRetunDataTable(@" select ba.FName '行政区划'
            ,ISNULL( SUM(case when FBillTypeID=1000011 and FStatus=0 then 1 end ),0) '省级问题|待整改'
            ,ISNULL( SUM(case when FBillTypeID=1000011 and FStatus=1 then 1 end ),0) '省级问题|待审核'
            ,ISNULL( SUM(case when FBillTypeID=1000011 and FStatus=2 then 1 end ),0) '省级问题|审核完成'
            ,ISNULL( SUM(case when FBillTypeID=1000012 and FStatus=0 then 1 end ),0) '市级问题|待整改'
            ,ISNULL( SUM(case when FBillTypeID=1000012 and FStatus=1 then 1 end ),0) '市级问题|待审核'
            ,ISNULL( SUM(case when FBillTypeID=1000012 and FStatus=2 then 1 end ),0) '市级问题|审核完成'
            ,ISNULL( SUM(case when FBillTypeID=1000013 and FStatus=0 then 1 end ),0) '县级问题|待整改'
            ,ISNULL( SUM(case when FBillTypeID=1000013 and FStatus=1 then 1 end ),0) '县级问题|待审核'
            ,ISNULL( SUM(case when FBillTypeID=1000013 and FStatus=2 then 1 end ),0) '县级问题|审核完成'
            from t_Base_Agency ba 
            left join t_Loan_Apply a on ba.FValue=a.FAgencyValue
            group by A.FAgencyValue,ba.FName

                 ", null);
            string name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, "问题点位按县市统计");
            if (ms.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Length", ms.Length.ToString());//文件长度 
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + name);//文件名称
                HttpContext.Current.Response.ContentType = "vnd.ms-excel.numberformat:yyyy-MM-dd ";
                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, num);
                }
                while (num > 0); HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Close();//关闭

            return response;
        }


        /// <summary>
        /// 全市老旧工业区块改造三年计划表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityChangeSchDataByAgency(int FYear)
        {
            Result result = new Result() { code = 1 };

            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue); 

            }

                try
            {
                var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
select * from 
 (
 select ag.FName FAgencyName,ag.FSort as AGSort
                    ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) 'FSumCount',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One   then 1 else 0 end )) 'Type1BeginFirst',
                    sum((case when o.FCityChangeType=1 and YEAR(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type1EndFirst',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type2BeginFirst',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type2EndFirst',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type3BeginFirst'
                    ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type3EndFirst'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type1BeginSecond'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type1EndSecond',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type2BeginSecond',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type2EndSecond',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type3BeginSecond'
                    ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type3EndSecond'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type1BeginThree',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type1EndThree',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type2BeginThree',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type2EndThree',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type3BeginThree'
                    ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type3EndThree'
                    ,'0' as FSort
                    from t_Base_Agency ag
                    left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0  AND o.FStatus>=1
                    Where 1=1 {3}
                    group by ag.FSort,ag.FName
                    
union 

select '合计',0 AGSort,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) 'FSumCount',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One   then 1 else 0 end )) 'Type1BeginFirst',
                    sum((case when o.FCityChangeType=1 and YEAR(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type1EndFirst',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type2BeginFirst',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type2EndFirst',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type3BeginFirst'
                    ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type3EndFirst'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type1BeginSecond'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type1EndSecond',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type2BeginSecond',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type2EndSecond',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type3BeginSecond'
                    ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type3EndSecond'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type1BeginThree',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type1EndThree',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type2BeginThree',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type2EndThree',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type3BeginThree'
                    ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type3EndThree'
                    ,'1' as FSort
from t_Base_Agency ag
                    left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0  AND o.FStatus>=1
                    Where 1=1 {3}
                    
  )a
  order by a.FSort  asc,a.AGSort  asc                       
                 ", FYear, FYear + 1, FYear + 2, formatParam), new { One = FYear, Two = FYear + 1, Three = FYear + 2 });

                result.@object = dt;
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("获取全市老旧工业区块改造三年计划表出错："+ex.Message);
                result.code = 0;
                result.message = "获取全市老旧工业区块改造三年计划表出错";
            }
            
            return result;

        }


        /// <summary>
        /// 全市老旧工业区块改造三年计划表Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetOldCityChangeSchExcelByAgency(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue);

            }

            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
        select [FName],[FSumCount], 
                             [Type1BeginFirst],
                            [Type1EndFirst],
                             [Type2BeginFirst],
                             [Type2EndFirst],
                             [Type3BeginFirst]
                            , [Type3EndFirst]
                            , [Type1BeginSecond]
                            , [Type1EndSecond],
                             [Type2BeginSecond],
                             [Type2EndSecond],
                             [Type3BeginSecond]
                            , [Type3EndSecond]
                            , [Type1BeginThree],
                             [Type1EndThree],
                             [Type2BeginThree],
                             [Type2EndThree],
                             [Type3BeginThree]
                            , [Type3EndThree] from 
         (
         select ag.FName ,ag.FSort as AGSort
                            ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) 'FSumCount',
                            sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One   then 1 else 0 end )) 'Type1BeginFirst',
                            sum((case when o.FCityChangeType=1 and YEAR(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type1EndFirst',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type2BeginFirst',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type2EndFirst',
                            sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type3BeginFirst'
                            ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type3EndFirst'
                            ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type1BeginSecond'
                            ,sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type1EndSecond',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type2BeginSecond',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type2EndSecond',
                            sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type3BeginSecond'
                            ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type3EndSecond'
                            ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type1BeginThree',
                            sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type1EndThree',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type2BeginThree',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type2EndThree',
                            sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type3BeginThree'
                            ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type3EndThree'
                            ,'0' as FSort
                            from t_Base_Agency ag
                            left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0  AND o.FStatus>=1
                            Where 1=1 {3}
                            group by ag.FSort,ag.FName
                    
        union 

        select '合计',0 AGSort,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) 'FSumCount',
                            sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One   then 1 else 0 end )) 'Type1BeginFirst',
                            sum((case when o.FCityChangeType=1 and YEAR(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type1EndFirst',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type2BeginFirst',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type2EndFirst',
                            sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Type3BeginFirst'
                            ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@One then 1 else 0 end )) 'Type3EndFirst'
                            ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type1BeginSecond'
                            ,sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type1EndSecond',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type2BeginSecond',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type2EndSecond',
                            sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Type3BeginSecond'
                            ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Two then 1 else 0 end )) 'Type3EndSecond'
                            ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type1BeginThree',
                            sum((case when o.FCityChangeType=1 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type1EndThree',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type2BeginThree',
                            sum((case when o.FCityChangeType=2 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type2EndThree',
                            sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Type3BeginThree'
                            ,sum((case when o.FCityChangeType=3 and Year(o.FChangeEndDate)=@Three then 1 else 0 end )) 'Type3EndThree'
                            ,'1' as FSort
        from t_Base_Agency ag
                            left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0  AND o.FStatus>=1
                            Where 1=1 {3}
                    
          )a
          order by a.FSort  asc,a.AGSort  asc                       
                         ", FYear, FYear + 1, FYear + 2, formatParam), new { One = FYear, Two = FYear + 1, Three = FYear + 2 });


            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个Sheet  
            ISheet sheet = wk.CreateSheet("Sheet1");
            //title
            IRow TitleRow = sheet.CreateRow(0);
            TitleRow.HeightInPoints = 25;
            TitleRow.CreateCell(0).SetCellValue("全市老旧工业区块改造三年计划表");
            ICellStyle headStyle = wk.CreateCellStyle();
            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont font = wk.CreateFont();
            font.FontHeightInPoints = 20;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            TitleRow.GetCell(0).CellStyle = headStyle;
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));


            IRow dbFirstRow = sheet.CreateRow(1);
            ICell cell = dbFirstRow.CreateCell(0);
            cell.SetCellValue("县（市区）");
            cell = dbFirstRow.CreateCell(1);
            cell.SetCellValue("三年改造任务数");
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 7));
            cell = dbFirstRow.CreateCell(2);
            cell.SetCellValue("2018");
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 8, 13));
            cell = dbFirstRow.CreateCell(8);
            cell.SetCellValue("2019");
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 14, 19));
            cell = dbFirstRow.CreateCell(14);
            cell.SetCellValue("2020");

            IRow dbSecendRow = sheet.CreateRow(2);
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 3));
            cell = dbSecendRow.CreateCell(2);
            cell.SetCellValue("整体（或大部分）拆除退出工业用途");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
            cell = dbSecendRow.CreateCell(4);
            cell.SetCellValue("整体（或大部分）拆除重建用于工业");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 6, 7));
            cell = dbSecendRow.CreateCell(6);
            cell.SetCellValue("综合整治（含部分拆除）用于产业提升或转型");

            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 8, 9));
            cell = dbSecendRow.CreateCell(8);
            cell.SetCellValue("整体（或大部分）拆除退出工业用途");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 10, 11));
            cell = dbSecendRow.CreateCell(10);
            cell.SetCellValue("整体（或大部分）拆除重建用于工业");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 12, 13));
            cell = dbSecendRow.CreateCell(12);
            cell.SetCellValue("综合整治（含部分拆除）用于产业提升或转型");

            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 14, 15));
            cell = dbSecendRow.CreateCell(14);
            cell.SetCellValue("整体（或大部分）拆除退出工业用途");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 16, 17));
            cell = dbSecendRow.CreateCell(16);
            cell.SetCellValue("整体（或大部分）拆除重建用于工业");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 18, 19));
            cell = dbSecendRow.CreateCell(18);
            cell.SetCellValue("综合整治（含部分拆除）用于产业提升或转型");

            IRow dbThreeRow = sheet.CreateRow(3);
            cell = dbThreeRow.CreateCell(2);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(3);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(4);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(5);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(6);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(7);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(8);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(9);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(10);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(11);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(12);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(13);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(14);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(15);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(16);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(17);
            cell.SetCellValue("拟完成");
            cell = dbThreeRow.CreateCell(18);
            cell.SetCellValue("拟启动");
            cell = dbThreeRow.CreateCell(19);
            cell.SetCellValue("拟完成");

            string name = "老旧城区三年改造计划" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, wk, sheet, 4);
            if (ms.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Length", ms.Length.ToString());//文件长度 
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + name);//文件名称
                HttpContext.Current.Response.ContentType = "vnd.ms-excel.numberformat:yyyy-MM-dd ";
                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, num);
                }
                while (num > 0); HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Close();//关闭

            return response;
        }


        /// <summary>
        /// 全市老旧工业区块改造进度表excel
        /// </summary>
        /// <param name="FYear"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetOldCityChangeProgressExcelByAgency(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue);

            }
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 select [县（市区）] 
                ,[三年改造任务数（个）]
                ,[整体（或大部分）拆除退出工业用途|已启动（个）]
                ,[整体（或大部分）拆除退出工业用途|已签约（个）]
                ,[整体（或大部分）拆除退出工业用途|已拆除（个）]
                ,[整体（或大部分）拆除退出工业用途|已开工（个）]
                ,[整体（或大部分）拆除退出工业用途|已完工（个）]
                ,[整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）]
                ,[整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）]
                ,[整体（或大部分）拆除重建用于工业|已启动]
                ,[整体（或大部分）拆除重建用于工业|已签约]
                ,[整体（或大部分）拆除重建用于工业|已拆除]
                ,[整体（或大部分）拆除重建用于工业|已开工]
                ,[整体（或大部分）拆除重建用于工业|已完工]
                ,[整体（或大部分）拆除重建用于工业|已拆除面积（万平方米）]
                ,[整体（或大部分）拆除重建用于工业|其中违法建筑面积（万平方米）]
                ,[综合整治（含部分拆除）用于产业提升或转型|拆除/整治中]
                ,[综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治]
                ,[综合整治（含部分拆除）用于产业提升或转型|已拆除面积（万平方米）]
                ,[综合整治（含部分拆除）用于产业提升或转型|已整治面积（万平方米）]
                From 
                (
                select ag.FSort AGSort,ag.FName '县（市区）'
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已启动（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已签约（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已开工（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已完工（个）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已启动'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已签约'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已开工'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已完工'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|其中违法建筑面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end )  '综合整治（含部分拆除）用于产业提升或转型|拆除/整治中'
                ,sum(  case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已整治面积（万平方米）'
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                group by ag.FSort,ag.fname

                union 

                select 0,'合计' '县（市区）'
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已启动（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已签约（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已开工（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已完工（个）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已启动'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已签约'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已开工'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已完工'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|其中违法建筑面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end )  '综合整治（含部分拆除）用于产业提升或转型|拆除/整治中'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|其中违法建筑面积（万平方米）'
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                ) a
                order by a.FSort asc ,a.AGSort asc
                 ", formatParam), new { One = FYear,  Three = FYear + 2 });

            string name = "全市老旧工业区块改造进度表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, "全市老旧工业区块改造进度表");
            if (ms.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Length", ms.Length.ToString());//文件长度 
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + name);//文件名称
                HttpContext.Current.Response.ContentType = "vnd.ms-excel.numberformat:yyyy-MM-dd ";
                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, num);
                }
                while (num > 0); HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Close();//关闭

            return response;
        }

        /// <summary>
        /// 全市老旧工业区块改造进度表数据
        /// </summary>
        /// <param name="FYear"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityChangeProgressDataByAgency(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue);
            }
            Result result = new Result() { code = 1 };
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 select *
                From 
                (
                select ag.FSort AGSort,ag.FName FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) Change1Status4
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) Change1Status5
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) 'Change2Area1'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) 'Change2Area2'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) ) then 1 else 0 end )  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) Change3Status2
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) Change3Area1
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) Change3Area2
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                group by ag.FSort,ag.fname

                union 

                select 0,'合计' FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) Change1Status4
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) Change1Status5
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) 'Change2Area1'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) 'Change2Area2'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end)  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) Change3Status2
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) Change3Area1
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) Change3Area2
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                ) a
                
                order by a.FSort asc ,a.AGSort asc
          
                 ", formatParam), new { One = FYear, Three = FYear + 2 });

            result.@object = dt;

            return result;
        }

        /// <summary>
        /// 全市老旧工业区块改造进度表excel(新)
        /// </summary>
        /// <param name="FYear"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetOldCityChangeProgressExcelByAgencyNew(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue);

            }
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 select [县（市区）] 
                ,[年度改造任务（个）]
                ,[整体（或大部分）拆除退出工业用途|已启动（个）]
                ,[整体（或大部分）拆除退出工业用途|已签约（个）]
                ,[整体（或大部分）拆除退出工业用途|已拆除（个）]
                ,[整体（或大部分）拆除退出工业用途|已开工（个）]
                ,[整体（或大部分）拆除退出工业用途|已完工（个）]
                ,[整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）]
                ,[整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）]
                ,[整体（或大部分）拆除重建用于工业|已启动]
                ,[整体（或大部分）拆除重建用于工业|已签约]
                ,[整体（或大部分）拆除重建用于工业|已拆除]
                ,[整体（或大部分）拆除重建用于工业|已开工]
                ,[整体（或大部分）拆除重建用于工业|已完工]
                ,[整体（或大部分）拆除重建用于工业|已拆除面积（万平方米）]
                ,[整体（或大部分）拆除重建用于工业|其中违法建筑面积（万平方米）]
                ,[综合整治（含部分拆除）用于产业提升或转型|拆除/整治中]
                ,[综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治]
                ,[综合整治（含部分拆除）用于产业提升或转型|已拆除面积（万平方米）]
                ,[综合整治（含部分拆除）用于产业提升或转型|已整治面积（万平方米）]
                From 
                (
                select ag.FSort AGSort,ag.FName '县（市区）'
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '年度改造任务（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已启动（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已签约（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已开工（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已完工（个）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已启动'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已签约'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已开工'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已完工'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|其中违法建筑面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end )  '综合整治（含部分拆除）用于产业提升或转型|拆除/整治中'
                ,sum(  case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已整治面积（万平方米）'
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and year(o.FChangeBeginDate)=@FChangeYear AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                group by ag.FSort,ag.fname

                union 

                select 0,'合计' '县（市区）'
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '年度改造任务（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已启动（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已签约（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已开工（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已完工（个）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已启动'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已签约'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已开工'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已完工'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除重建用于工业|其中违法建筑面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end )  '综合整治（含部分拆除）用于产业提升或转型|拆除/整治中'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除面积（万平方米）'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|其中违法建筑面积（万平方米）'
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and year(o.FChangeBeginDate)=@FChangeYear AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                ) a
                order by a.FSort asc ,a.AGSort asc
                 ", formatParam), new { FChangeYear = FYear });

            string name = "全市老旧工业区块改造进度表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, "全市老旧工业区块改造进度表");
            if (ms.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Length", ms.Length.ToString());//文件长度 
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + name);//文件名称
                HttpContext.Current.Response.ContentType = "vnd.ms-excel.numberformat:yyyy-MM-dd ";
                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, num);
                }
                while (num > 0); HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Close();//关闭

            return response;
        }

        /// <summary>
        /// 全市老旧工业区块改造进度表数据(新)
        /// </summary>
        /// <param name="FYear"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityChangeProgressDataByAgencyNew(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue);
            }
            Result result = new Result() { code = 1 };
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                  select *
                From 
                (
                select ag.FSort AGSort,ag.FName FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) Change1Status4
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) Change1Status5
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) 'Change2Area1'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) 'Change2Area2'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) ) then 1 else 0 end )  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) Change3Status2
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) Change3Area1
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) Change3Area2
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and year(o.FChangeBeginDate)=@FChangeYear  AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                group by ag.FSort,ag.fname

                union 

                select 0,'合计' FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) Change1Status4
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) Change1Status5
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) 'Change2Area1'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) 'Change2Area2'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end)  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) Change3Status2
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) Change3Area1
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) Change3Area2
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and year(o.FChangeBeginDate)=@FChangeYear AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                ) a
                
                order by a.FSort asc ,a.AGSort asc
          
                 ", formatParam), new { FChangeYear = FYear });

            result.@object = dt;

            return result;
        }

        [HttpGet]
        [BtLog]
        public Result SaveProcessDataStatistical(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND ag.FValue={0} ", UserInfo.FAgencyValue);
            }
            Result result = new Result() { code = 1 };
            try
            {
                var dt = ModelOpretion.ExecuteSqlNoneQuery(string.Format(@" 
                insert into t_Loan_ProgressStatisHistory
                select *,CONVERT( varchar(20),GETDATE(),120)
                From 
                (
                select ag.FSort AGSort,ag.FName FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) Change1Status4
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) Change1Status5
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) 'Change2Area1'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) 'Change2Area2'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) ) then 1 else 0 end )  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) Change3Status2
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) Change3Area1
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) Change3Area2
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and year(o.FChangeBeginDate)=@FChangeYear  AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                group by ag.FSort,ag.fname

                union 

                select 0,'合计' FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=4 then 1 else 0 end ) ) Change1Status4
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=5 then 1 else 0 end ) ) Change1Status5
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea1,0) else 0 end ) ) 'Change2Area1'
                ,SUM((case when o.FCityChangeType=2  then ISNULL(o1.FArea2,0) else 0 end ) ) 'Change2Area2'
                ,sum( case when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  ) then 1 else 0 end)  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then 1 else 0 end ) Change3Status2
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType0Area else 0 end ) Change3Area1
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) and o3.FTotalCount>0 then FType1Area else 0 end ) Change3Area2
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and year(o.FChangeBeginDate)=@FChangeYear AND o.FStatus>=1
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount 
                ,sum(case when FReadyType=0 then isnull(FReadyArea,0) else 0 end) FType0Area
                ,sum(case when FReadyType=1 then isnull(FReadyArea,0) else 0 end) FType1Area
                from t_Loan_OldCityExtend3
                where ISNULL(FIsDeleted,0)=0 
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                Where 1=1 {0}
                ) a
                
          
                 ", formatParam), new { FChangeYear = FYear }).Submit();
            }
            catch(Exception ex)
            {
                result.code = 0;
                result.message = "保存历史进度表报错";
                LogService.Default.Fatal("保存历史进度表报错："+ex.Message);
            }
            

            return result;
        }

        /// <summary>
        /// 获取历史进度时间数组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result GetProcessHistoryTimeArray([FromBody]JObject obj)
        {
            Result result = new Result();
            result.code = 0;
            dynamic dy = obj;
            int curr = dy.curr;
            int pageSize = dy.pageSize;
            int totalCount = 0;

            if (curr < 0 || pageSize < 0)
            {
                result = new Result()
                {
                    code = 0,
                    message = "分页参数配置错误"
                };
            }
            try
            {
                DataTable dt = StatisticalBll.GetTimeArray(UserInfo, dy, curr, pageSize, out totalCount);

                result.code = 1;
                result.@object = dt;
                result.page = new page(curr, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(ex, "获取时间列表出错" + ex.Message);
                result.code = 0;
                result.message = "获取时间列表出错";
            }

            return result;
        }


        /// <summary>
        /// 根据时间获取历史进度列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetProcessHistoryList(string FTime)
        {
            Result result = new Result();
            result.code = 0;
            
            try
            {
                DataTable dt = StatisticalBll.GetList(FTime);

                result.code = 1;
                result.@object = dt;
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(ex, "获取历史进度列表出错" + ex.Message);
                result.code = 0;
                result.message = "获取历史进度列表出错";
            }

            return result;
        }





        /// <summary>
        /// 全市老旧工业区块汇总表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityAllData(int FYear)
        {
            Result result = new Result() { code = 1 };
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND o.FAgencyValue={0} ", UserInfo.FAgencyValue);
            }
            
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 with enum as 
                (
	                select ev.FValue,ev.FName as FEVName,et.FName from t_Base_EnumType et 
	                left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID
                )

                select ROW_NUMBER() over(ORDER BY o.FID asc) rowID,o.FAreaName,isnull(o.FIndustry,'') FIndustry
                ,o.FAgencyName,isnull(o.FOccupy,0) FOccupy,isnull(o.FTotalAcreage,0) FTotalAcreage
                ,isnull(FNonConBuildingArea,0) FNonConBuildingArea
                ,enc.FEVName
                ,o.FTownChangeType
                ,ena.FEVName
                ,isnull(o.FTotalInvestAmount,0) FTotalInvestAmount
                ,isnull(o.FAfterChangeArea,0) FAfterChangeArea
                ,CONVERT(nvarchar(32),o.FChangeBeginDate,23) FChangeBeginDate
                ,CONVERT(nvarchar(32),o.FChangeEndDate,23) FChangeEndDate
                 ,(case when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=1 then '已启动' 
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=2 then '已签约' 
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=3 then '已拆除'
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=4 then '已开工'
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=5 then '已完工'
	                when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  )  then '拆除/整治中'
	                when o.FCityChangeType=3 and o3.FFinishedCount=o3.FTotalCount and o3.FTotalCount>0 then '已拆除/整治完成'
	                else '无'
	                end ) FProgress,
                (case when o.FDemonstration='1' then '是' 
					else '否' end 
                ) FDemonstration
                ,o.FTownName,convert(nvarchar(32), o.FChangeStatusTimeNow,23) FChangeStatusTimeNow,year(o.FChangeBeginDate)
                from t_Loan_OldCity o
                left join  enum enc on enc.FName='按台州市办法分类' and enc.FValue=o.FCityChangeType
                left join  enum ena on ena.FName='改造后用途' and ena.FValue=o.FAfterChange
                left join 
                (
	                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                    where ISNULL(FIsDeleted,0)=0
	                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                where o.FStatus>=1 and  isnull(o.FIsDeleted,0)=0 AND YEAR(FChangeBeginDate)>=@FYear  AND YEAR(FChangeBeginDate)<=@Three  {0}

                 ", formatParam), new { FYear= FYear, Three=FYear+2 });

            result.@object = dt;

            return result;
        }


        /// <summary>
        /// 全市老旧工业区块汇总表Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetOldCityAllExcel(int FYear)
        {
            string formatParam = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND o.FAgencyValue={0} ", UserInfo.FAgencyValue);
            }
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                        with enum as 
                        (
	                        select ev.FValue,ev.FName as FEVName,et.FName from t_Base_EnumType et 
	                        left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID
                        )

                        select ROW_NUMBER() over(ORDER BY o.FID asc) '序号'
                        ,o.FAreaName '区块名称',isnull(o.FIndustry,'') '主要产业'
                        ,o.FAgencyName '所属县市区'
                        ,o.FTownName [乡镇街道],isnull(o.FOccupy,0) '总占地'
                        ,isnull(o.FTotalAcreage,0) '总建筑面积'
                        ,isnull(FNonConBuildingArea,0) '其中违建面积'
                        ,enc.FEVName '市定改造方式'
                        ,o.FTownChangeType '县市区自定改造方式'
                        ,ena.FEVName '改造后用途'
                        ,isnull(o.FTotalInvestAmount,0) '拟投资额' 
                        ,isnull(o.FAfterChangeArea,0) '改造后建筑面积'
                        ,year(o.FChangeBeginDate) [年度]
                        ,CONVERT(nvarchar(32),o.FChangeEndDate,23) '拟完成时间'
                         ,(case when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=1 then '已启动' 
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=2 then '已签约' 
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=3 then '已拆除'
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=4 then '已开工'
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=5 then '已完工'
	                        when o.FCityChangeType=3 and ( isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) )  then '拆除/整治中'
	                        when o.FCityChangeType=3 and o3.FFinishedCount=o3.FTotalCount and o3.FTotalCount>0 then '已拆除/整治完成'
	                        else '无'
	                        end ) '目前进度',
                        convert(nvarchar(32), o.FChangeStatusTimeNow,23) [最新进度时间]
                        ,(case when o.FDemonstration='1' then '是' 
					            else '否' end 
                         ) [是否示范项目]
                        from t_Loan_OldCity o
                        left join  enum enc on enc.FName='按台州市办法分类' and enc.FValue=o.FCityChangeType
                        left join  enum ena on ena.FName='改造后用途' and ena.FValue=o.FAfterChange
                        left join 
                        (
	                        select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                            where ISNULL(FIsDeleted,0)=0 
	                        group by FLoanID 
                        ) o3 on o3.FLoanID=o.FID 
                        where o.FStatus>=1 and  isnull(o.FIsDeleted,0)=0 AND YEAR(FChangeBeginDate)>=@FYear AND YEAR(FChangeBeginDate)<=@Three  {0}
                 ", formatParam), new { FYear = FYear, Three = FYear + 2 });

            string name = "全市老旧工业区块汇总表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, "全市老旧工业区块汇总表");
            if (ms.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Length", ms.Length.ToString());//文件长度 
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + name);//文件名称
                HttpContext.Current.Response.ContentType = "vnd.ms-excel.numberformat:yyyy-MM-dd ";
                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, num);
                }
                while (num > 0); HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Close();//关闭

            return response;
        }

        /// <summary>
        /// 四边三化统计excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetFPerimetersExcel(int FBillTypeID,int FPerimeter)
        {
            string formatParam = string.Empty;
            string formatApply = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND a.FAgencyValue={0} ", UserInfo.FAgencyValue);
            }
            if(FPerimeter>0)
            {
                formatApply = string.Format(" and a.FPerimeter={0} ", FPerimeter);
            }

            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                         
         select 
         a.FName [城市名称],
         [乱搭乱建|目标任务],
         [乱搭乱建|已完成],
         [乱搭乱建|完成比例],
         [乱堆乱放|目标任务],
         [乱堆乱放|已完成],
         [乱堆乱放|完成比例],
         [废品垃圾|目标任务],
         [废品垃圾|已完成],
         [废品垃圾|完成比例],
         [乱采乱挖|目标任务],
         [乱采乱挖|已完成],
         [乱采乱挖|完成比例],
         [广告残留|目标任务],
         [广告残留|已完成],
         [广告残留|完成比例],
         [青山白化|目标任务],
         [青山白化|已完成],
         [青山白化|完成比例],
         [绿化缺失|目标任务],
         [绿化缺失|已完成],
         [绿化缺失|完成比例],
         [赤膊房|目标任务],
         [赤膊房|已完成],
         [赤膊房|完成比例],
         [矿山整治|目标任务],
         [矿山整治|已完成],
         [矿山整治|完成比例],
         [农田管理用房|目标任务],
         [农田管理用房|已完成],
         [农田管理用房|完成比例]
         ,[总计|目标任务]
         ,[总计|已完成]
         ,[总计|完成比例]
         from 
         (
         select ba.FName,ba.FSort as AGSort
         ,ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)  '乱搭乱建|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)  '乱搭乱建|已完成'
         ,case when ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱搭乱建|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)  '乱堆乱放|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)  '乱堆乱放|已完成'
         ,case when ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱堆乱放|完成比例'
         ,ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)  '废品垃圾|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)  '废品垃圾|已完成'
          ,case when ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '废品垃圾|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)  '乱采乱挖|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)  '乱采乱挖|已完成'
           ,case when ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱采乱挖|完成比例'
           ,ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)  '广告残留|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)  '广告残留|已完成'
            ,case when ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '广告残留|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)  '青山白化|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)  '青山白化|已完成'
             ,case when ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '青山白化|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)  '绿化缺失|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)  '绿化缺失|已完成'
              ,case when ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '绿化缺失|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)  '赤膊房|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)  '赤膊房|已完成'
               ,case when ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '赤膊房|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)  '矿山整治|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)  '矿山整治|已完成'
                ,case when ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '矿山整治|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)  '农田管理用房|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)  '农田管理用房|已完成'
                 ,case when ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '农田管理用房|完成比例'
         ,ISNULL( sum(1),0)  [总计|目标任务]
                 ,ISNULL( sum(case when  f.FilesCount>0 then 1 else 0 end ),0)  [总计|已完成]
                         ,case when ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(1),0)*100 as decimal(18,2))  end  [总计|完成比例]
         ,0 FSortSum
         from t_Base_Agency ba 
         left join t_Loan_Apply a on a.FAgencyValue=ba.FValue and a.FBillTypeID=@FBillTypeID  AND a.FStatus=2 and isnull(a.FIsDeleted,0)=0 {1}
         left join (select 
         ff.FLoanID,COUNT(0) FilesCount from t_loan_Files ff 
         left join t_Base_AttachmentType at on at.FID=ff.FAttachmentTypeID    where ff.FBillTypeID=@FBillTypeID and at.FName='整改后照片'   group by  ff.FLoanID ) f on f.FLoanID=a.FID 
         where 1=1  {0}
         group by ba.FValue,ba.FName,ba.FSort
 
 
        union 

        select '合计',0
         ,ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)  '乱搭乱建|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)  '乱搭乱建|已完成'
         ,case when ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱搭乱建|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)  '乱堆乱放|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)  '乱堆乱放|已完成'
         ,case when ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱堆乱放|完成比例'
         ,ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)  '废品垃圾|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)  '废品垃圾|已完成'
          ,case when ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '废品垃圾|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)  '乱采乱挖|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)  '乱采乱挖|已完成'
           ,case when ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱采乱挖|完成比例'
           ,ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)  '广告残留|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)  '广告残留|已完成'
            ,case when ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '广告残留|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)  '青山白化|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)  '青山白化|已完成'
             ,case when ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '青山白化|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)  '绿化缺失|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)  '绿化缺失|已完成'
              ,case when ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '绿化缺失|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)  '赤膊房|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)  '赤膊房|已完成'
               ,case when ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '赤膊房|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)  '矿山整治|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)  '矿山整治|已完成'
                ,case when ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '矿山整治|完成比例'
          ,ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)  '农田管理用房|目标任务'
         ,ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)  '农田管理用房|已完成'
                 ,case when ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	        else CAST( cast(ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '农田管理用房|完成比例'
          ,ISNULL( sum(1),0)  [总计|目标任务]
          ,ISNULL( sum(case when  f.FilesCount>0 then 1 else 0 end ),0)  [总计|已完成]
          ,case when ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(1),0)*100 as decimal(18,2))  end  [总计|完成比例]
         ,1 FSortSum
         from t_Base_Agency ba 
         left join t_Loan_Apply a on a.FAgencyValue=ba.FValue and a.FBillTypeID=@FBillTypeID  AND a.FStatus=2 and isnull(a.FIsDeleted,0)=0 {1}
         left join (select 
         ff.FLoanID,COUNT(0) FilesCount from t_loan_Files ff 
         left join t_Base_AttachmentType at on at.FID=ff.FAttachmentTypeID    where ff.FBillTypeID=@FBillTypeID and at.FName='整改后照片'   group by  ff.FLoanID ) f on f.FLoanID=a.FID 
         where 1=1 {0}
         ) a
         order by a.FSortSum asc,a.AGSort asc
                 ", formatParam, formatApply), new { FBillTypeID = FBillTypeID, FPerimeter = FPerimeter});

            string name = "四边三化统计表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, "四边三化统计表");
            if (ms.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Length", ms.Length.ToString());//文件长度 
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + name);//文件名称
                HttpContext.Current.Response.ContentType = "vnd.ms-excel.numberformat:yyyy-MM-dd ";
                byte[] buffer = new byte[65536];
                ms.Position = 0;
                int num;
                do
                {
                    num = ms.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, num);
                }
                while (num > 0); HttpContext.Current.Response.Flush();
            }
            HttpContext.Current.Response.Close();//关闭

            return response;
        }


        public Result GetPerimeterTable(int FBillTypeID, int FPerimeter)
        {
            Result result = new Result() { code = 1 };
            string formatParam = string.Empty;
            string formatApply = string.Empty;
            //县级用户只能看到自己的数据
            if (UserInfo.FLevel == 3 || UserInfo.FLevel == 4)
            {
                formatParam = string.Format(" AND a.FAgencyValue={0} ", UserInfo.FAgencyValue);
            }
            if (FPerimeter > 0)
            {
                formatApply = string.Format(" and a.FPerimeter={0} ", FPerimeter);
            }
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                  
                 select 
                *
 
                 from 
                 (
                 select ba.FName  as FAgencyName,ba.FSort as AGSort
                 ,ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)  FPerimeter1Count
                 ,ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter1Finish
                 ,case when ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter1Rate
                  ,ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)  FPerimeter2Count
                 ,ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter2Finish
                 ,case when ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter2Rate
                 ,ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)  FPerimeter3Count
                 ,ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter3Finish
                  ,case when ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter3Rate
                  ,ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)  FPerimeter4Count
                 ,ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter4Finish
                   ,case when ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter4Rate
                   ,ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)  FPerimeter5Count
                 ,ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter5Finish
                    ,case when ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter5Rate
                  ,ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)  FPerimeter6Count
                 ,ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter6Finish
                     ,case when ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter6Rate
                  ,ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)  FPerimeter7Count
                 ,ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter7Finish
                      ,case when ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter7Rate
                  ,ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)  FPerimeter8Count
                 ,ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter8Finish
                       ,case when ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter8Rate
                  ,ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)  FPerimeter9Count
                 ,ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter9Finish
                        ,case when ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter9Rate
                  ,ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)  FPerimeter10Count
                 ,ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)  FPerimeter10Finish
                         ,case when ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)*100 as decimal(18,2))  end  FPerimeter10Rate
                 ,ISNULL( sum(1),0)  FAllCount
                 ,ISNULL( sum(case when  f.FilesCount>0 then 1 else 0 end ),0)  FAllFinish
                         ,case when ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(1),0)*100 as decimal(18,2))  end  FAllRate
                 ,0 FSortSum
                 from t_Base_Agency ba 
                 left join t_Loan_Apply a on a.FAgencyValue=ba.FValue and a.FBillTypeID=@FBillTypeID  AND a.FStatus=2 and isnull(a.FIsDeleted,0)=0 {1}
                 left join (select 
                 ff.FLoanID,COUNT(0) FilesCount from t_loan_Files ff 
                 left join t_Base_AttachmentType at on at.FID=ff.FAttachmentTypeID    where ff.FBillTypeID=@FBillTypeID and at.FName='整改后照片'   group by  ff.FLoanID ) f on f.FLoanID=a.FID 
                 where 1=1 {0}
                 group by ba.FValue,ba.FName,ba.FSort
 
 
                union 

                select '合计',0
                 ,ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)  '乱搭乱建|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)  '乱搭乱建|已完成'
                 ,case when ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=1  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=1  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱搭乱建|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)  '乱堆乱放|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)  '乱堆乱放|已完成'
                 ,case when ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=2  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=2  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱堆乱放|完成比例'
                 ,ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)  '废品垃圾|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)  '废品垃圾|已完成'
                  ,case when ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=3  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=3  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '废品垃圾|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)  '乱采乱挖|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)  '乱采乱挖|已完成'
                   ,case when ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=4  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=4  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '乱采乱挖|完成比例'
                   ,ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)  '广告残留|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)  '广告残留|已完成'
                    ,case when ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=5  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=5  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '广告残留|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)  '青山白化|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)  '青山白化|已完成'
                     ,case when ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=6  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=6  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '青山白化|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)  '绿化缺失|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)  '绿化缺失|已完成'
                      ,case when ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=7  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=7  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '绿化缺失|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)  '赤膊房|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)  '赤膊房|已完成'
                       ,case when ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=8  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=8  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '赤膊房|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)  '矿山整治|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)  '矿山整治|已完成'
                        ,case when ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=9  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=9  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '矿山整治|完成比例'
                  ,ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)  '农田管理用房|目标任务'
                 ,ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)  '农田管理用房|已完成'
                         ,case when ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when a.FProbTypeID=10  and f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(case when a.FProbTypeID=10  then 1 else 0 end ),0)*100 as decimal(18,2))  end  '农田管理用房|完成比例'
                 ,ISNULL( sum(1),0)  FAllCount
                 ,ISNULL( sum(case when  f.FilesCount>0 then 1 else 0 end ),0)  FAllFinish
                         ,case when ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0)=0 then 0.00
	                else CAST( cast(ISNULL( sum(case when f.FilesCount>0 then 1 else 0 end ),0) as decimal(18,2))/ISNULL( sum(1),0)*100 as decimal(18,2))  end  FAllRate
                 ,1 FSortSum
                 from t_Base_Agency ba 
                 left join t_Loan_Apply a on a.FAgencyValue=ba.FValue and a.FBillTypeID=@FBillTypeID  AND a.FStatus=2 and isnull(a.FIsDeleted,0)=0 {1}
                 left join (select 
                 ff.FLoanID,COUNT(0) FilesCount from t_loan_Files ff 
                 left join t_Base_AttachmentType at on at.FID=ff.FAttachmentTypeID    where ff.FBillTypeID=@FBillTypeID and at.FName='整改后照片'   group by  ff.FLoanID ) f on f.FLoanID=a.FID 
                 where 1=1 {0}
                 ) a
                 order by a.FSortSum asc,a.AGSort asc

          
                 ", formatParam,formatApply), new { FBillTypeID = FBillTypeID, FPerimeter = FPerimeter });

            result.@object = dt;

            return result;
        }
    }
}