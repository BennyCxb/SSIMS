using BLL;
using BT.Manage.Frame.Base;
using BT.Manage.Model;
using BT.Manage.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using TZBaseFrame.Attributes;
using TZManageAPI.Base;
using BT.Manage.Core;

namespace TZManageAPI.Controllers
{
    /// <summary>
    /// 核准单控制器
    /// </summary>
    [JwtAuthActionFilter]
    public class LoanApplyController : BaseController
    {
        /// <summary>
        /// 获取省级问题列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Result GetSJList([FromBody]JObject obj)
        {
            Result result = new Result();
            result.code = 0;
            dynamic dy = obj;
            int curr = dy.curr;
            int pageSize = dy.pageSize;
            int totalCount = 0;

            if (curr < 0 || pageSize < 0)
            {
                result = new Result()
                {
                    code = 0,
                    message = "分页参数配置错误"
                };
            }
            try
            {
                DataTable dt= LoanApplyBll.GetSJApplyList(UserInfo, dy, curr, pageSize, out totalCount);

                result.code = 1;
                result.@object = dt;
                result.page = new page(curr,pageSize,totalCount);
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("获取省级问题列表出错",ex.Message,ex);
                result.code = 0;
                result.message = "获取省级问题列表出错";
            }

            return result;
        }

        /// <summary>
        /// 保存省级问题表单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public Result SaveSJApply([FromBody]LoanApplyInfo info)
        {
            Result result = new Result();
            result.code = 0;
            if(string.IsNullOrWhiteSpace(info.FBillNo))
            {
                result.code = 0;
                result.message = "请检查表单数据！";
                return result;
            }
            
            try
            {
                //修改
                if (info.FID > 0)
                {
                    //只有保存状态可以修改数据
                    if (info.FStatus != 0)
                    {
                        result.code = 0;
                        result.message = "当前状态不允许修改！";
                        return result;
                    }

                    info.FModifyUserID = UserInfo.UserId;
                    info.FModifyTime = DateTime.Now;
                    int k = info.SaveOnSubmit();
                    if (k > 0)
                    {
                        result.code = 1;
                        result.@object = info.FID;
                        result.message = "修改成功";
                    }
                }//新增
                else
                {
                    info.FAddUserID = UserInfo.UserId;
                    info.FAddTime = DateTime.Now;
                    int id = info.SaveOnSubmit();
                    if (id > 0)
                    {
                        result.code = 1;
                        result.@object = id;
                        result.message = "添加成功！";
                    }
                }
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("保存省级问题表单出错：",ex.Message,ex);
                result.code = 0;
                result.message = "保存省级问题表单出错";
            }

            return result;
        }
             
    }
}