using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Core;
using Model;
using BT.Manage.Tools.Utils;
using System.Data;

namespace FLow
{
    public class DoFlow : BaseDoFlow
    {
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public static Result DoAdopt(FlowModel flowModel)
        {
            Result result = new Result();
            //当前状态
            int FStatus = 0;
            try
            {
                //获取流程自定义业务逻辑
                IFlow flow = FlowFactory.GetFlowClass(flowModel.FBillTypeID);
                if (flow == null)
                {
                    result.code = 0;
                    result.message = "流程自定义业务逻辑未配置。";
                    return result;
                }

                if (flowModel != null)
                {
                    //完善流程类
                    flowModel = ConsummateFlowModel(flowModel);
                    //验证数据权限  数据库level和前端level是否一致
                    //todo

                    //验证流程权限
                    result = CheckStatus(flowModel, FStatus, DealEnum.审核);
                    if (result.code == 0)
                    {
                        return result;
                    }
                    //执行审核流程前自定义业务流程
                    result= flow.BeforeAdopt(flowModel);
                    if (result.code == 0)
                    {
                        return result;
                    }

                    //审核
                    //todo

                    //审核完成
                    //todo
                }
            }
            catch (Exception ex)
            {
                result.code = 0;
                result.message ="审核报错："+ ex.Message;
                //记录日志
                //todo
            }

            return result;
            
        }

        
    }
}
