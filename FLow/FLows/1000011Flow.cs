using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Model;
using BT.Manage.Core;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;

namespace FLow.FLows
{
    public class _1000011Flow : IFlow
    {
        /// <summary>
        /// 省级问题提交前
        /// </summary>
        /// <returns></returns>
        public Result BeforeSubmit(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;
            return result;
        }

        /// <summary>
        /// 省级问题提交后
        /// </summary>
        /// <returns></returns>
        public Result AfterSubmit(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;

            return result;
        }

        /// <summary>
        /// 省级问题审核前
        /// </summary>
        /// <returns></returns>
        public Result BeforeAdopt(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;
            
            return result;
        }

        /// <summary>
        /// 省级问题审核后
        /// </summary>
        /// <returns></returns>
        public Result AfterAdopt(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;

            return result;
        }

        /// <summary>
        /// 省级问题驳回前
        /// </summary>
        /// <returns></returns>
        public Result BeforeReject(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;
            
            return result;
        }

        /// <summary>
        /// 省级问题驳回后
        /// </summary>
        /// <returns></returns>
        public Result AfterReject(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;
            try
            {
                LoanApplyInfo info = ModelOpretion.FirstOrDefault<LoanApplyInfo>(flowModel.FID);
                info.FStatus = 0;
                info.FCheckLevel = 0;
                info.FNextCheckLevel = 0;
                info.FCheckName = string.Empty;
                info.FChangeStatus = 0;
                info.Update().Submit();

                checkApplyInfo chkInfo = new checkApplyInfo();
                chkInfo.FBillTypeID = flowModel.FBillTypeID;
                chkInfo.FBillID = flowModel.FID;
                chkInfo.FLevelName = "驳回";
                chkInfo.FLevel = 0;
                chkInfo.FNextLevel = 0;
                chkInfo.FNextLevelName = string.Empty;
                chkInfo.FRemark = flowModel.FlowMessage;
                chkInfo.FAddTime = DateTime.Now;
                chkInfo.FAddUserID = flowModel.UserID;
                chkInfo.SaveOnSubmit();
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("省级问题驳回出错",ex.Message,ex);
                result.code = 0;
                result.message = string.Format("省级问题驳回出错");
            }
            
            return result;
        }
    }
}
