using BLL.Common;
using BT.Manage.Frame.Base;
using BT.Manage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TZManageAPI.DTO;

namespace TZBaseFrame.Attributes
{
    /// <summary>
    /// Token拦截器 验证Token的有效性
    /// </summary>
    public class JwtAuthActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 后台系统Token拦截器 验证Token的有效性
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Method == HttpMethod.Options)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted);
                    return;
                }
                //验证有效性
                AuthToken.VerifyToken(actionContext);


                //验证是否登录
                var request = actionContext.Request;
                var headerItems = request.Headers;
                var token = string.IsNullOrEmpty(headerItems.Authorization.ToString())
                    ? string.Empty
                    : headerItems.Authorization.ToString();

                token = token.Replace("Bearer ", "");
                var item = CacheHelper.CacheGet<LoginDataDto>(token);
                if (item == null)
                {
                    var result = new Result();
                    result.code = 2;
                    result.message = "登录过期，请重新登录";
                    var response = actionContext.Request.CreateResponse(result);
                    actionContext.Response = response;
                }
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("验证token出错:"+ex.ToString());
            }
            

            base.OnActionExecuting(actionContext);
        }
    }
}
