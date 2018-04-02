﻿using BLL;
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

            FlowModel flowModel = new FlowModel();
            flowModel.FBillTypeID = 1000011;
            flowModel.FCurrentLevel = 10;
            flowModel.FlowMessage = "通过";
            //flowModel.FFlowResult = 1;
            flowModel.FID = 45;
            flowModel.UserID = 1;
            result = DoFlow.DoAdopt(flowModel);

            //result= UploadFiles.UploadFilesForQiNiu("C:\\Users\\admin\\Desktop\\config设置1.png", "config设置1.png");
            //result = UploadFiles.GetQiNiuPrivateUrl("下载.jpg");

            //result = UploadFiles.GetQiNiuPrivateUrl("下载.jpg");

            //LogService.Default.Fatal("111");

            Console.ReadKey();
        }
    }
}
