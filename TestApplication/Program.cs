using BLL;
using BT.Manage.Frame.Base;
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

            result.@object= MenuBll.ReturnJson(1);

            CacheHelper.CacheSet<Result>("1234",result,1);

            Result info = CacheHelper.CacheGet<Result>("1234");
            
            

            Console.ReadKey();
        }
    }
}
