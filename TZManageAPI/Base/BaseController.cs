using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TZManageAPI.DTO;
namespace TZManageAPI.Base
{
    public class BaseController : ApiController
    {
        /// <summary>
        /// 获取中怡康保全授权信息
        /// </summary>
        public string GetRequestToken
        {
            get
            {
                var request = HttpContext.Current.Request;
                var headerItems = request.Headers;
                var token = string.IsNullOrEmpty(headerItems["Authorization"])
                    ? string.Empty
                    : headerItems["Authorization"];
                if (string.IsNullOrEmpty(token))
                    return string.Empty;
                return token.Replace("Bearer ", "");
            }
        }



        /// <summary>
        /// 基础控制类 在请求头中的Token中得到当前请求用户的UserId
        /// </summary>
        /// <returns>返回登录结果</returns>
        public LoginDataDto UserInfo { get { return LoginBll.GetUserInfo(GetRequestToken); } }


    }
}