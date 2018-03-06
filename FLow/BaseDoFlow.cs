using BT.Manage.Frame.Base;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;
using System.Data;
using BT.Manage.Core;

namespace FLow
{
    public enum DealEnum
    {
        提交,
        审核,
        驳回,
    }
    public class BaseDoFlow
    {
        /// <summary>
        /// 完善流程类
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        protected static FlowModel ConsummateFlowModel(FlowModel flowModel)
        {
            tsysFlowInfo flowInfo = ModelOpretion.FirstOrDefault<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID && p.FLevel == flowModel.FCurrentLevel);
            if (flowInfo.FID > 0)
            {
                flowModel.TableName = flowInfo.FTableName;
                flowModel.FCheckTable = flowInfo.FCheckTable;
                flowModel.KeyFiledName = flowInfo.FKeyName;
                flowModel.FNextLevel = flowInfo.FNextLevel.ToSafeInt32(0);
            }
            return flowModel;
        }

        /// <summary>
        /// 验证流程权限
        /// </summary>
        /// <returns></returns>
        protected static Result CheckStatus(FlowModel flowModel, int FStatus, DealEnum dealEnum)
        {
            Result result = new Result();
            result.code = 1;

            try
            {
                switch (dealEnum)
                {
                    case DealEnum.提交:
                        result = CheckSubmit(flowModel, FStatus, dealEnum);
                        break;
                    case DealEnum.审核:
                        result = CheckAdopt(flowModel, FStatus, dealEnum);
                        break;
                    case DealEnum.驳回:
                        result = CheckReject(flowModel, FStatus, dealEnum);
                        break;
                }
            }
            catch (Exception ex)
            {
                result.code = 0;
                result.message = "验证流程权限报错" + ex.Message;
                //记录日志
                //todo
            }


            return result;
        }

        
        /// <summary>
        /// 提交时流程权限验证
        /// </summary>
        /// <param name="flowModel"></param>
        /// <param name="FStatus"></param>
        /// <param name="dealEnum"></param>
        /// <returns></returns>
        private static Result CheckSubmit(FlowModel flowModel, int FStatus, DealEnum dealEnum)
        {
            Result result = new Result();
            result.code = 1;

            if (FStatus != 0)
            {
                result.code = 0;
                result.message = "当前状态不允许提交";
                return result;
            }

            //验证是否有流程 
            int FirstNode = GetFirstNode(flowModel);
            if (FirstNode == 0)
            {
                result.code = 0;
                result.message = "流程配置不存在。";
            }

            return result;
        }

        /// <summary>
        /// 审核时流程权限验证
        /// </summary>
        /// <param name="flowModel"></param>
        /// <param name="FStatus"></param>
        /// <param name="dealEnum"></param>
        /// <returns></returns>
        private static Result CheckAdopt(FlowModel flowModel, int FStatus, DealEnum dealEnum)
        {
            Result result = new Result();
            result.code = 1;

            if (FStatus != 1)
            {
                result.code = 0;
                result.message = "当前状态不允许审核。";
                return result;
            }
            
            //验证下级流程的准确性
            tsysFlowInfo flowInfo = ModelOpretion.FirstOrDefault<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID && p.FLevel == flowModel.FNextLevel);
            //有下级流程标识 数据库里却不存在
            if (flowInfo.FID == 0 && flowModel.FNextLevel != 0)
            {
                result.code = 0;
                result.message = "流程配置存在问题。";
            }

            return result;
        }

        /// <summary>
        /// 驳回时流程权限验证
        /// </summary>
        /// <param name="flowModel"></param>
        /// <param name="FStatus"></param>
        /// <param name="dealEnum"></param>
        /// <returns></returns>
        private static Result CheckReject(FlowModel flowModel, int FStatus, DealEnum dealEnum)
        {
            Result result = new Result();
            result.code = 1;
            if (FStatus != 1)
            {
                result.code = 0;
                result.message = "当前状态不允许驳回";
                return result;
            }

            return result;
        }

        /// <summary>
        /// 获取流程首节点
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        protected static int GetFirstNode(FlowModel flowModel)
        {
            int firstNode = 0;
            DataTable dt = ModelOpretion.SearchDataRetunDataTable(@" Select Top 1 From t_sys_Flow Where FBillTypeID=@TBillTypeID Order By FLevel ", new { TBillTypeID = flowModel.FBillTypeID });
            if (dt.Rows.Count > 0)
            {
                firstNode = dt.Rows[0][0].ToSafeInt32(0);
            }

            return firstNode;
        }
    }
}
