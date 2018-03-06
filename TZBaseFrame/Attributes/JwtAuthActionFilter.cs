using BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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
            if (actionContext.Request.Method == HttpMethod.Options)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted);
                return;
            }
            AuthToken.VerifyToken(actionContext);
            base.OnActionExecuting(actionContext);
        }
    }
}
