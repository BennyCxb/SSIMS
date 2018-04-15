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
    }
}