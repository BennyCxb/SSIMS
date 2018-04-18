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
using FLow;
using TZManageAPI.Common;
using BT.Manage.Tools.Utils;
using BT.Manage.Verification;

namespace TZManageAPI.Controllers
{
    public class FlowController : BaseController
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result SubmitApply([FromBody]FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 0;
            flowModel.UserID = UserInfo.UserId;
            result = DoFlow.DoSubmit(flowModel);

            return result;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result AdoptApply([FromBody]FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 0;
            flowModel.UserID = UserInfo.UserId;
            result = DoFlow.DoAdopt(flowModel);

            return result;
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result RejectApply([FromBody]FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 0;
            flowModel.UserID = UserInfo.UserId;
            result = DoFlow.DoReject(flowModel);

            return result;
        }


        /// <summary>
        /// 获取审核情况列表
        /// </summary>
        /// <param name="FLoanID">问题主键ID</param>
        /// <param name="FBillTypeID">单据类型</param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetCheckList(int FLoanID, int FBillTypeID)
        {
            Result result = new Result();
            result.code = 0;

            try
            {
                DataTable dt = LoanApplyBll.GetCheckList(FLoanID, FBillTypeID);
                result.code = 1;
                result.@object = dt;
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(@"获取审核情况错误", ex.Message, ex);
                result.code = 0;
                result.message = string.Format("获取审核情况错误");
            }

            return result;

        }
    }
}