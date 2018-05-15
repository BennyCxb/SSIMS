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
using Newtonsoft.Json.Linq;
using BT.Manage.Model;
using AutoMapper;

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

            //result = UploadFiles.UploadFilesForQiNiu("C:\\Users\\admin\\Desktop\\config设置1.png", "config设置1.png");
            //result = UploadFiles.GetQiNiuPrivateUrl("下载.jpg");

            result = UploadFiles.GetQiNiuPrivateUrl("下载.jpg");

            //LogService.Default.Fatal("111");

            //var dt = ModelOpretion.SearchDataRetunDataTable(@" select ba.FName '行政区划'
            //    ,SUM(case when FBillTypeID=1000011 and FStatus=0 then 1 end ) '省级问题|待整改'
            //    ,SUM(case when FBillTypeID=1000011 and FStatus=1 then 1 end ) '省级问题|待审核'
            //    ,SUM(case when FBillTypeID=1000011 and FStatus=2 then 1 end ) '省级问题|审核完成'
            //    ,SUM(case when FBillTypeID=1000012 and FStatus=0 then 1 end ) '市级问题|待整改'
            //    ,SUM(case when FBillTypeID=1000012 and FStatus=1 then 1 end ) '市级问题|待审核'
            //    ,SUM(case when FBillTypeID=1000012 and FStatus=2 then 1 end ) '市级问题|审核完成'
            //    ,SUM(case when FBillTypeID=1000013 and FStatus=0 then 1 end ) '县级问题|待整改'
            //    ,SUM(case when FBillTypeID=1000013 and FStatus=1 then 1 end ) '县级问题|待审核'
            //    ,SUM(case when FBillTypeID=1000013 and FStatus=2 then 1 end ) '县级问题|审核完成'
            //    from t_Loan_Apply a
            //    inner join t_Base_Agency ba on ba.FValue=a.FAgencyValue
            //    group by A.FAgencyValue,ba.FName
            //     ", null);

            //NPOIHelper.ExportExcel(dt, "");

            //IList<OldCityExtend3DTO> list = new List<OldCityExtend3DTO>();
            //OldCityExtend3DTO d = new OldCityExtend3DTO() { FID = 1, FLoanID=1 };
            //list.Add(d);
            //d = new OldCityExtend3DTO() { FID = 2, FLoanID = 2 };
            //list.Add(d);

            //Console.WriteLine(JsonConvert.SerializeObject(list));

            //JArray arr = new JArray();
            //var d1 = new JObject() { { "FID","1" }, { "FStatus","1" }, { "FTime", "2018-04-22" } };
            //var d2 = new JObject() { { "FID", "2" }, { "FStatus", "2" }, { "FTime", "2018-04-22" } };
            //arr.Add(d1);
            //arr.Add(d2);

            //IList<OldCityExtend12DTO> l= arr.ToObject<List<OldCityExtend12DTO>>();


            Mapper.Initialize(cfg =>
            {
                //老旧城区改造方式1或2 
                cfg.CreateMap<OldCityExtend12DTO, LoanOldCityExtend12Info>();
                cfg.CreateMap<LoanOldCityExtend12Info, OldCityExtend12DTO>();
                cfg.CreateMap<LoanOldCityExtend12Info, OldCityExtend12DTOShow>();
                //老旧城区改造进度（改造方式3）
                cfg.CreateMap<OldCityExtend3DTO, LoanOldCityExtend3Info>();
                cfg.CreateMap<LoanOldCityExtend3Info, OldCityExtend3DTO>();
            }
                );
            List<LoanOldCityExtend12Info> list = ModelOpretion.ModelList<LoanOldCityExtend12Info>(p => p.FLoanID == 2);

            IList<OldCityExtend12DTOShow> listDTO = Mapper.Map<List<LoanOldCityExtend12Info>, List<OldCityExtend12DTOShow>>(list);

            //获取主表改造进度
            LoanOldCityInfo oldInfo = ModelOpretion.FirstOrDefault<LoanOldCityInfo>(2);


            


            Console.ReadKey();
        }


        public class OldCityExtend12DTO
        {

            /// <summary>
            /// 主键ID
            /// </summary>
            public int FID { get; set; }

            /// <summary>
            /// 表单主键ID
            /// </summary>
            public int FLoanID { get; set; }

            /// <summary>
            /// 进度
            /// </summary>
            public int FStatus { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            public DateTime? FTime { get; set; }

            /// <summary>
            /// 面积1
            /// </summary>
            public decimal? FArea1 { get; set; }

            /// <summary>
            /// 面积2
            /// </summary>
            public decimal? FArea2 { get; set; }


        }



        public class OldCityExtend3DTO
        {

            /// <summary>
            /// 主键ID 自增
            /// </summary>
            public int FID { get; set; }

            /// <summary>
            /// 主表单ID
            /// </summary>
            public int FLoanID { get; set; }
            /// <summary>
            /// 企业名
            /// </summary>
            public string FCompanyName { get; set; }
            /// <summary>
            /// 企业名称
            /// </summary>
            public int? FReadyType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public decimal? FReadyArea { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int? FDoingType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public System.DateTime? FDoingTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int? FDoneType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public System.DateTime? FDoneTime { get; set; }


        }

        public class OldCityExtend12DTOShow
        {
            /// <summary>
            /// 主键ID
            /// </summary>
            public int FID { get; set; }

            /// <summary>
            /// 表单主键ID
            /// </summary>
            public int FLoanID { get; set; }

            /// <summary>
            /// 进度
            /// </summary>
            public int FStatus { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            public DateTime? FTime { get; set; }

            /// <summary>
            /// 面积1
            /// </summary>
            public decimal? FArea1 { get; set; }

            /// <summary>
            /// 面积2
            /// </summary>
            public decimal? FArea2 { get; set; }

            /// <summary>
            /// 提交状态
            /// </summary>
            public int? FSubmitStatus { get; set; }
        }
    }
}
