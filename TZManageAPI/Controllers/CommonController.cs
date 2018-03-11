using BLL;
using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TZBaseFrame.Attributes;
using TZManageAPI.Base;

namespace TZManageAPI.Controllers
{
    public class CommonController : BaseController
    {
        /// <summary>
        /// 获取行政区划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthActionFilter]
        public Result GetAgencyList()
        {
            Result result = new Result();
            result.code = 0;


            DataTable dt= BaseAgencyBll.GetAgencyList();

            if(dt.Rows.Count>0)
            {
                result.code = 1;
                result.@object = dt;
            }

            return result;
        }



    }
}