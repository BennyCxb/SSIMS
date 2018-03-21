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
using BT.Manage.Tools;

namespace FLow
{
    public class DoFlow : BaseDoFlow
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        public static Result DoSubmit(FlowModel flowModel)
        {
            Result result = new Result();
            result.code = 1;
            //验证基础参数
            if(flowModel == null)
            {
                result.code = 0;
                result.message = "流程基础参数不可为空。";
            }
            else if (flowModel.FID==0 || flowModel.FBillTypeID==0 ||flowModel.UserID==0)
            {
                result.code = 0;
                result.message = "流程基础参数不可为空。";
            }
            if(result.code == 0)
            {
                return result;
            }

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

                flowModel = ConsummateFlowModel(flowModel, DealEnum.提交);

                //验证提交权限
                result = CheckStatus(flowModel, DealEnum.提交);
                if (result.code == 0)
                {
                    return result;
                }

                //提交前业务逻辑
                result= flow.BeforeSubmit(flowModel);
                if (result.code == 0)
                {
                    return result;
                }

                //提交内置业务逻辑
                result= DoSubmitBase(flowModel);
                if (result.code == 0)
                {
                    return result;
                }

                //提交后业务逻辑
                result = flow.AfterSubmit(flowModel);
                if (result.code == 0)
                {
                    return result;
                }

            }
            catch(Exception ex)
            {
                result.code = 0;
                result.message = "提交报错：" + ex.Message;
                //记录日志
                LogService.Default.Fatal("提交报错：", ex.Message, ex);
            }


            return result;
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public static Result DoAdopt(FlowModel flowModel)
        {
            Result result = new Result();
            //验证基础参数
            if (flowModel == null)
            {
                result.code = 0;
                result.message = "流程基础参数不可为空。";
            }
            else if (flowModel.FID == 0 || flowModel.FBillTypeID == 0 || flowModel.UserID == 0)
            {
                result.code = 0;
                result.message = "流程基础参数不可为空。";
            }
            if (result.code == 0)
            {
                return result;
            }

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
                    flowModel = ConsummateFlowModel(flowModel, DealEnum.审核);
                    //验证数据权限  数据库level和前端level是否一致
                    //todo

                    //验证流程权限
                    result = CheckStatus(flowModel, DealEnum.审核);
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

                    //审核内置业务逻辑
                    result = DoAdoptBase(flowModel);
                    if (result.code == 0)
                    {
                        return result;
                    }

                    //审核完成
                    result = flow.AfterAdopt(flowModel);
                    if (result.code == 0)
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = 0;
                result.message ="审核报错："+ ex.Message;
                //记录日志
                LogService.Default.Fatal("审核报错：",ex.Message,ex);
            }

            return result;
            
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        public static Result DoReject(FlowModel flowModel)
        {
            Result result = new Result();
            //验证基础参数
            if (flowModel == null)
            {
                result.code = 0;
                result.message = "流程基础参数不可为空。";
            }
            else if (flowModel.FID == 0 || flowModel.FBillTypeID == 0 || flowModel.UserID == 0)
            {
                result.code = 0;
                result.message = "流程基础参数不可为空。";
            }
            if (result.code == 0)
            {
                return result;
            }

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
                    flowModel = ConsummateFlowModel(flowModel, DealEnum.驳回);
                    

                    //验证流程权限
                    result = CheckStatus(flowModel, DealEnum.驳回);
                    if (result.code == 0)
                    {
                        return result;
                    }
                    //执行驳回流程前自定义业务流程
                    result = flow.BeforeReject(flowModel);
                    if (result.code == 0)
                    {
                        return result;
                    }

                    //审核内置业务逻辑
                    result = DoRejectBase(flowModel);
                    if (result.code == 0)
                    {
                        return result;
                    }

                    //审核完成
                    result = flow.AfterReject(flowModel);
                    if (result.code == 0)
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = 0;
                result.message = "审核报错：" + ex.Message;
                //记录日志
                LogService.Default.Fatal("审核报错：", ex.Message, ex);
            }

            return result;

        }
    }
}
