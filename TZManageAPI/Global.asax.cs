using AutoMapper;
using BT.Manage.Model;
using BT.Manage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TZManageAPI.DTO;

namespace TZManageAPI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            #region  automapper初始化

            try
            {
                Mapper.Initialize(cfg =>
                {
                    //老旧城区改造方式1或2 
                    cfg.CreateMap<OldCityExtend12DTO, LoanOldCityExtend12Info>();
                    cfg.CreateMap<LoanOldCityExtend12Info, OldCityExtend12DTO>();
                    //老旧城区改造进度（改造方式3）
                    cfg.CreateMap<OldCityExtend3DTO, LoanOldCityExtend3Info>();
                    cfg.CreateMap< LoanOldCityExtend3Info, OldCityExtend3DTO>();
                }
                );
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(ex,"初始化AutoMapper报错："+ex.Message);
            }
            

            #endregion
        }
    }
}