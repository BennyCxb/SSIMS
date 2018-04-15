using BLL;
using BT.Manage.Frame.Base;
using BT.Manage.Tools;
using FLow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZBaseFrame;
using TZManage.Model;
using BT.Manage.Core;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Result result = new Result();

            //result.@object = MenuBll.ReturnJson(1);
            //CacheHelper.CacheSet<Result>("1234", result, 1);
            //Result info = CacheHelper.CacheGet<Result>("1234");

            //FlowModel flowModel = new FlowModel();
            //flowModel.FBillTypeID = 1000011;
            //flowModel.FCurrentLevel = 10;
            //flowModel.FlowMessage = "通过";
            ////flowModel.FFlowResult = 1;
            //flowModel.FID = 45;
            //flowModel.UserID = 1;
            //result = DoFlow.DoAdopt(flowModel);

            //result= UploadFiles.UploadFilesForQiNiu("C:\\Users\\admin\\Desktop\\config设置1.png", "config设置1.png");
            //result = UploadFiles.GetQiNiuPrivateUrl("下载.jpg");

            //result = UploadFiles.GetQiNiuPrivateUrl("下载.jpg");

            //LogService.Default.Fatal("111");

            var dt = ModelOpretion.SearchDataRetunDataTable(@" select ba.FName '行政区划'
                ,SUM(case when FBillTypeID=1000011 and FStatus=0 then 1 end ) '省级问题|待整改'
                ,SUM(case when FBillTypeID=1000011 and FStatus=1 then 1 end ) '省级问题|待审核'
                ,SUM(case when FBillTypeID=1000011 and FStatus=2 then 1 end ) '省级问题|审核完成'
                ,SUM(case when FBillTypeID=1000012 and FStatus=0 then 1 end ) '市级问题|待整改'
                ,SUM(case when FBillTypeID=1000012 and FStatus=1 then 1 end ) '市级问题|待审核'
                ,SUM(case when FBillTypeID=1000012 and FStatus=2 then 1 end ) '市级问题|审核完成'
                ,SUM(case when FBillTypeID=1000013 and FStatus=0 then 1 end ) '县级问题|待整改'
                ,SUM(case when FBillTypeID=1000013 and FStatus=1 then 1 end ) '县级问题|待审核'
                ,SUM(case when FBillTypeID=1000013 and FStatus=2 then 1 end ) '县级问题|审核完成'
                from t_Loan_Apply a
                inner join t_Base_Agency ba on ba.FValue=a.FAgencyValue
                group by A.FAgencyValue,ba.FName
                 ", null);

            NPOIHelper.ExportExcel(dt, "");



            Console.ReadKey();
        }
    }
}
