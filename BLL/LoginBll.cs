using BLL.Common;
using BT.Manage.Frame.Base;
using BT.Manage.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZBaseFrame;
using TZManageAPI.DTO;

namespace BLL
{
    public class LoginBll : BaseBll
    {
        #region 登录

        /// <summary>
        ///  登录
        /// </summary>
        /// <param name="data">登录参数</param>
        /// <returns>返回结果</returns>
        public static Result Login(LoginDto dto)
        {
            var strPwd = EncryptUtils.MD5(dto.UserPwd.Trim().ToLower()).ToLower();

            var itme = DbContext.StoredProcedure("Pro_SysLogin")
                .Parameter("userName", dto.UserName)
                .Parameter("userPwd", strPwd).QuerySingle<LoginDataDto>();
            if (itme != null)
            {
                itme.MenuJson = MenuBll.ReturnJson(itme.UserId);
                var token = AuthToken.CreatToken(itme);
                var dtoItem = CacheHelper.CacheGet<LoginDataDto>(token);
                if (dtoItem == null)
                {
                    if (CacheHelper.CacheSet(token, itme, 720))
                        return new Result { code = 1, message = "登录成功", @object = token };
                    return new Result { code = 0, message = "缓存服务器异常", @object = null };
                }
                return new Result { code = 1, message = "登录成功", @object = token };
            }
            return new Result { code = 0, message = "账户或密码错误", @object = null };
        }
        #endregion


        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <param name="token">会话Token</param>
        /// <returns>返回登录用户信息</returns>
        public static LoginDataDto GetUserInfo(string token)
        {
            return CacheHelper.CacheGet<LoginDataDto>(token);
        }


        #region 退出登录

        /// <summary>
        ///     退出登录
        /// </summary>
        /// <param name="token">当前会话Token</param>
        /// <returns>返回退出结果</returns>
        public static Result LoginOut(string token)
        {
            if (CacheHelper.RemoveCache(token))
                return new Result { code = 1, message = "退出登录，清空缓存", @object = true };
            return new Result { code = 0, message = "退出异常", @object = false };
        }

        #endregion
    }
}
