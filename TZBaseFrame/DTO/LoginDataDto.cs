using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TZManageAPI.DTO
{
    public class LoginDataDto
    {
        /// <summary>
        ///     用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     角色名称
        /// </summary>
        public string UserRoleName { get; set; }

        /// <summary>
        ///     角色ID
        /// </summary>
        public string UserRoleIds { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///     所拥有的菜单
        /// </summary>
        public List<Menu> MenuJson { get; set; }

        /// <summary>
        ///     新加的分机号
        /// </summary>
        public string User_Agent { get; set; }
        /// <summary>
        ///     新加的用户部门ID
        /// </summary>
        public string User_FDeptID { get; set; }
        /// <summary>
        /// 数据权限
        /// </summary>
        public string User_FDataAt { get; set; }

        /// <summary>
        /// 行政规划等级  1.超级管理员 2.市级 3.县级一级  4.县级二级
        /// </summary>
        public int FLevel { get; set; }

        /// <summary>
        /// 行政区划
        /// </summary>
        public string FAgencyValue { get; set; }

    }

    public class LoginDto
    {
        /// <summary>
        ///     用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string UserPwd { get; set; }
    }


    public class PayLoad
    {
        public string id { get; set; }
        public string name { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
    }
}