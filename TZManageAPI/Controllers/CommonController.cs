using BLL;
using BT.Manage.Frame.Base;
using BT.Manage.Verification;
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
        [BtLog]
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

        /// <summary>
        /// 根据枚举类型名获取枚举列表
        /// </summary>
        /// <param name="EnumType"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetEnumList(string EnumType)
        {
            Result result = new Result();
            result.code = 0;

            DataTable dt = BaseEnumValueBll.GetEnumList(EnumType);

            result.code = 1;
            result.@object = dt;
            return result;

        }
             



    }
}