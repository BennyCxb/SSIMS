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
            try
            {
                var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
 select * from 
 (
 select ag.FName '县（市区）'
                    ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Task{0}Type1',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Task{0}Type2',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Task{0}Type3'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Task{1}Type1',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Task{1}Type2',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Task{1}Type3'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Task{2}Type1',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Task{2}Type2',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Task{2}Type3'
                    ,'0' as FSort
                    from t_Base_Agency ag
                    left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0 
                    group by ag.FValue,ag.FName
                    
union 

select '合计',SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Task{0}Type1',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Task{0}Type2',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) 'Task{0}Type3'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Task{1}Type1',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Task{1}Type2',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) 'Task{1}Type3'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Task{2}Type1',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Task{2}Type2',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) 'Task{2}Type3'
                    ,'1' as FSort
from t_Base_Agency ag
                    left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0 
  )a
  order by a.FSort           
                 ", FYear, FYear + 1, FYear + 2), new { One = FYear, Two = FYear + 1, Three = FYear + 2 });

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
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
 select [县（市区）] 
,[{0}年|整体（或大部分）拆除退出工业用途]
,[{0}年|整体（或大部分）拆除重建用于工业]
,[{0}年|综合整治（含部分拆除）用于产业提升或转型]
,[{1}年|整体（或大部分）拆除退出工业用途]
,[{1}年|整体（或大部分）拆除重建用于工业]
,[{1}年|综合整治（含部分拆除）用于产业提升或转型]
,[{2}年|整体（或大部分）拆除退出工业用途]
,[{2}年|整体（或大部分）拆除重建用于工业]
,[{2}年|综合整治（含部分拆除）用于产业提升或转型] from 
 (
 select ag.FName '县（市区）'
                    ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) '{0}年|整体（或大部分）拆除退出工业用途',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) '{0}年|整体（或大部分）拆除重建用于工业',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) '{0}年|综合整治（含部分拆除）用于产业提升或转型'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) '{1}年|整体（或大部分）拆除退出工业用途',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) '{1}年|整体（或大部分）拆除重建用于工业',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) '{1}年|综合整治（含部分拆除）用于产业提升或转型'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) '{2}年|整体（或大部分）拆除退出工业用途',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) '{2}年|整体（或大部分）拆除重建用于工业',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) '{2}年|综合整治（含部分拆除）用于产业提升或转型'
                    ,'0' as FSort
                    from t_Base_Agency ag
                    left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0 
                    group by ag.FValue,ag.FName
                    
union 

select '合计',SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数',
                    sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) '{0}年|整体（或大部分）拆除退出工业用途',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) '{0}年|整体（或大部分）拆除重建用于工业',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@One then 1 else 0 end )) '{0}年|综合整治（含部分拆除）用于产业提升或转型'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) '{1}年|整体（或大部分）拆除退出工业用途',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) '{1}年|整体（或大部分）拆除重建用于工业',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Two then 1 else 0 end )) '{1}年|综合整治（含部分拆除）用于产业提升或转型'
                    ,sum((case when o.FCityChangeType=1 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) '{2}年|整体（或大部分）拆除退出工业用途',
                    sum((case when o.FCityChangeType=2 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) '{2}年|整体（或大部分）拆除重建用于工业',
                    sum((case when o.FCityChangeType=3 and Year(o.FChangeBeginDate)=@Three then 1 else 0 end )) '{2}年|综合整治（含部分拆除）用于产业提升或转型'
                    ,'1' as FSort
from t_Base_Agency ag
                    left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three AND isnull(o.FIsDeleted,0)=0 
  )a
  order by a.FSort           
                 ", FYear, FYear + 1, FYear + 2), new { One = FYear, Two = FYear + 1, Three = FYear + 2 });
            string name = "老旧城区三年改造计划" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";//以当前时间为excel表命名   

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            Stream ms = NPOIHelper.ExportExcel(dt, "全市老旧工业区块改造三年计划表");
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
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 select [县（市区）] 
                ,[三年改造任务数（个）]
                ,[整体（或大部分）拆除退出工业用途|已启动（个）]
                ,[整体（或大部分）拆除退出工业用途|已签约（个）]
                ,[整体（或大部分）拆除退出工业用途|已拆除（个）]
                ,[整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）]
                ,[整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）]
                ,[整体（或大部分）拆除重建用于工业|已启动]
                ,[整体（或大部分）拆除重建用于工业|已签约]
                ,[整体（或大部分）拆除重建用于工业|已拆除]
                ,[整体（或大部分）拆除重建用于工业|已开工]
                ,[整体（或大部分）拆除重建用于工业|已完工]
                ,[综合整治（含部分拆除）用于产业提升或转型|拆除/整治中]
                ,[综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治]
                From 
                (
                select ag.FName '县（市区）'
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已启动（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已签约（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除（个）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已启动'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已签约'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已开工'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已完工'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) then 1 else 0 end )  '综合整治（含部分拆除）用于产业提升或转型|拆除/整治中'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) then 1 else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治'
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                group by ag.FValue,ag.fname

                union 

                select '合计' '县（市区）'
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) '三年改造任务数（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已启动（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已签约（个）'
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除（个）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|已拆除面积（万平方米）'
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) '整体（或大部分）拆除退出工业用途|其中违法建筑面积（万平方米）'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已启动'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已签约'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已拆除'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已开工'
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) '整体（或大部分）拆除重建用于工业|已完工'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) then 1 else 0 end )  '综合整治（含部分拆除）用于产业提升或转型|拆除/整治中'
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) then 1 else 0 end ) '综合整治（含部分拆除）用于产业提升或转型|已拆除/已整治'
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                ) a
                order by a.FSort asc 
          
                 "), new { One = FYear,  Three = FYear + 2 });

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
            Result result = new Result() { code = 1 };
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 select *
                From 
                (
                select ag.FName FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) then 1 else 0 end )  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) then 1 else 0 end ) Change3Status2
                ,'0' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0 and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                group by ag.FValue,ag.fname

                union 

                select '合计' FAgencyName
                ,SUM((case when isnull(o.FID,0)<>0 then 1 else 0 end )) TotalCount
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=1 then 1 else 0 end ) ) Change1Status1
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=2 then 1 else 0 end ) ) Change1Status2
                ,SUM((case when o.FCityChangeType=1 and o.FChangeStatus=3 then 1 else 0 end ) ) Change1Status3
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea1,0) else 0 end ) ) Change1Area1
                ,SUM((case when o.FCityChangeType=1  then ISNULL(o1.FArea2,0) else 0 end ) ) Change1Area2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=1 then 1 else 0 end ) ) Change2Status1
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=2 then 1 else 0 end ) ) Change2Status2
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=3 then 1 else 0 end ) ) Change2Status3
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=4 then 1 else 0 end ) ) Change2Status4
                ,SUM((case when o.FCityChangeType=2 and o.FChangeStatus=5 then 1 else 0 end ) ) Change2Status5
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0) then 1 else 0 end )  Change3Status0
                ,sum( case when o.FCityChangeType=3 and  isnull(o3.FFinishedCount,0)=isnull(o3.FTotalCount,0) then 1 else 0 end ) Change3Status2
                ,'1' as FSort
                from t_Base_Agency ag
                left join t_Loan_OldCity o on o.FAgencyValue=ag.FValue AND isnull(o.FIsDeleted,0)=0  and Year(o.FChangeBeginDate)>=@One and Year(o.FChangeBeginDate)<=@Three
                left join t_Loan_OldCityExtend12 o1 on o1.FLoanID=o.FID and o1.FStatus=3 and o1.FSubmitStatus=1
                left join 
                (
                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 
                ) a
                order by a.FSort asc 
          
                 "), new { One = FYear, Three = FYear + 2 });

            result.@object = dt;

            return result;
        }

        /// <summary>
        /// 全市老旧工业区块汇总表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityAllData()
        {
            Result result = new Result() { code = 1 };
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                 with enum as 
                (
	                select ev.FValue,ev.FName as FEVName,et.FName from t_Base_EnumType et 
	                left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID
                )

                select ROW_NUMBER() over(ORDER BY o.FID asc) rowID,o.FAreaName,isnull(o.FIndustry,0) FIndustry
                ,o.FAreaName,isnull(o.FOccupy,0) FOccupy,isnull(o.FTotalAcreage,0) FTotalAcreage
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
	                when o.FCityChangeType=3 and o3.FFinishedCount<o3.FTotalCount then '拆除/整治中'
	                when o.FCityChangeType=3 and o3.FFinishedCount<o3.FTotalCount then '已拆除/整治完成'
	                else '无'
	                end ) FProgress
                from t_Loan_OldCity o
                left join  enum enc on enc.FName='按台州市办法分类' and enc.FValue=o.FCityChangeType
                left join  enum ena on ena.FName='改造后用途' and ena.FValue=o.FCityChangeType
                left join 
                (
	                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
	                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID 


          
                 "), new { });

            result.@object = dt;

            return result;
        }


        /// <summary>
        /// 全市老旧工业区块汇总表Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public HttpResponseMessage GetOldCityAllExcel()
        {
            var dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" 
                        with enum as 
                        (
	                        select ev.FValue,ev.FName as FEVName,et.FName from t_Base_EnumType et 
	                        left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID
                        )

                        select ROW_NUMBER() over(ORDER BY o.FID asc) '序号'
                        ,o.FAreaName '区块名称',isnull(o.FIndustry,0) '主要产业'
                        ,o.FAreaName '所属县市区',isnull(o.FOccupy,0) '总占地'
                        ,isnull(o.FTotalAcreage,0) '总建筑面积'
                        ,isnull(FNonConBuildingArea,0) '其中违建面积'
                        ,enc.FEVName '市定改造方式'
                        ,o.FTownChangeType '县市区自定改造方式'
                        ,ena.FEVName '改造后用途'
                        ,isnull(o.FTotalInvestAmount,0) '拟投资额' 
                        ,isnull(o.FAfterChangeArea,0) '改造后建筑面积'
                        ,CONVERT(nvarchar(32),o.FChangeBeginDate,23) '拟启动时间'
                        ,CONVERT(nvarchar(32),o.FChangeEndDate,23) '拟完成时间'
                         ,(case when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=1 then '已启动' 
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=2 then '已签约' 
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=3 then '已拆除'
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=4 then '已开工'
	                        when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=5 then '已完工'
	                        when o.FCityChangeType=3 and o3.FFinishedCount<o3.FTotalCount then '拆除/整治中'
	                        when o.FCityChangeType=3 and o3.FFinishedCount<o3.FTotalCount then '已拆除/整治完成'
	                        else '无'
	                        end ) '目前进度'
                        from t_Loan_OldCity o
                        left join  enum enc on enc.FName='按台州市办法分类' and enc.FValue=o.FCityChangeType
                        left join  enum ena on ena.FName='改造后用途' and ena.FValue=o.FCityChangeType
                        left join 
                        (
	                        select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
	                        group by FLoanID 
                        ) o3 on o3.FLoanID=o.FID 

                 "), new { });

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

    }
}