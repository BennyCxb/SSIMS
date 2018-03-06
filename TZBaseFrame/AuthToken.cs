using Jose;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZManageAPI.DTO;
using System.Web.Http.Controllers;
using BT.Manage.Frame.Base;
using System.Net;
using System.Net.Http;

namespace BLL.Common
{
    public class AuthToken
    {
        /// <summary>
        ///     创建Token
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreatToken(LoginDataDto data)
        {
            var secret = ConfigurationManager.AppSettings["TokenSecret"];
            var payload = new
            {
                id = data.UserId,
                name = data.UserName.Trim().ToLower(),
                iss = "TZManage.Api",
                aud = "www.liaoyu.com",
                sub = "TZManage.Web"
            };
            return JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256);
        }
        /// <summary>
        ///     验证Token合法性
        /// </summary>
        /// <param name="actionContext"></param>
        public static void VerifyToken(HttpActionContext actionContext)
        {
            var result = new Result();
            var jwtObject = new LoginDataDto();
            var secret = ConfigurationManager.AppSettings["TokenSecret"];
            if (actionContext.Request.Headers.Authorization == null ||
                actionContext.Request.Headers.Authorization.Scheme != "Bearer" ||
                actionContext.Request.Headers.Authorization.Parameter == "undefined")
            {
                result.code = 0;
                result.message = "Token不能为空";
                setErrorResponse(actionContext, result);
            }
            else
            {
                try
                {
                    PayLoad payLoad = DecodeToken(secret, actionContext.Request.Headers.Authorization.Parameter);
                    if (int.Parse(payLoad.id) > 0)
                    {
                        //验证通过不处理
                    }
                    else
                    {
                        result.code = 0;
                        result.message = "Token验证无效";
                        setErrorResponse(actionContext, result);
                    }
                }
                catch (Exception ex)
                {
                    result.code = 0;
                    result.message = ex.Message;
                    setErrorResponse(actionContext, result);
                }
            }
        }
        private static void setErrorResponse(HttpActionContext actionContext, Result resutl)
        {
            var response = actionContext.Request.CreateResponse(resutl);
            actionContext.Response = response;
        }

        public static PayLoad DecodeToken(string secret, string token)
        {
            return JWT.Decode<PayLoad>(token, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256);
        }
    }
}
