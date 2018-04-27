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
using BT.Manage.DataAccess;
using BT.Manage.DataAccess.SqlClient;
using FLow.DTO;
using BT.Manage.Model;

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
        //连接数据库
        private static DatabaseProperty conn = DBSettings.GetDatabaseProperty("AkData");

        /// <summary>
        /// 完善流程类
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        protected static FlowModel ConsummateFlowModel(FlowModel flowModel,DealEnum dealEnum)
        {
            //判断权限
            switch (dealEnum)
            {
                case DealEnum.提交:
                    flowModel = ConsummateFlowModelSubmit(flowModel);
                    break;
                case DealEnum.审核:
                    flowModel = ConsummateFlowModelNomal(flowModel);
                    break;
                case DealEnum.驳回:
                    flowModel = ConsummateFlowModelReject(flowModel);
                    break;
            }

            
            return flowModel;
        }

        /// <summary>
        /// 通用完善flowModel
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        private static FlowModel ConsummateFlowModelNomal(FlowModel flowModel)
        {
            tsysFlowInfo flowInfo = ModelOpretion.FirstOrDefault<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID && p.FLevel == flowModel.FCurrentLevel);
            if (flowInfo.FID > 0)
            {
                flowModel.TableName = flowInfo.FTableName;
                flowModel.FCheckTable = flowInfo.FCheckTable;
                flowModel.KeyFiledName = flowInfo.FKeyName;
                flowModel.FNextLevel = flowInfo.FNextLevel.ToSafeInt32(0);
                flowModel.FFlowFID = flowInfo.FID;
            };

            return flowModel;
        }

        /// <summary>
        /// 提交时完善flowModel
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        private static FlowModel ConsummateFlowModelSubmit(FlowModel flowModel)
        {
            FlowDTO dto = GetFirstNode(flowModel);
            if(dto.FLevel>0)
            {
                flowModel.TableName = dto.FTableName;
                flowModel.FCheckTable = dto.FCheckTableName;
                flowModel.KeyFiledName = dto.FKeyName;
                flowModel.FNextLevel = dto.FNextLevel.ToSafeInt32(0);
            }

            return flowModel;
        }

        /// <summary>
        /// 驳回时完善flowModel
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        private static FlowModel ConsummateFlowModelReject(FlowModel flowModel)
        {
            tsysFlowInfo flowInfo = new tsysFlowInfo();
            //审核完成驳回时取最后一级审核级次当做当前级次  ly  为了应对县级问题在审核完成时做市级退回
            if(flowModel.FCurrentLevel==0)
            {
                flowModel.FCurrentLevel = ModelOpretion.ModelList<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID).OrderByDescending(p => p.FLevel).FirstOrDefault().FLevel;
            }
            flowInfo = ModelOpretion.FirstOrDefault<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID && p.FLevel == flowModel.FCurrentLevel);
            if (flowInfo.FID > 0)
            {
                flowModel.TableName = flowInfo.FTableName;
                flowModel.FCheckTable = flowInfo.FCheckTable;
                flowModel.KeyFiledName = flowInfo.FKeyName;
                flowModel.FFlowFID = flowInfo.FID;
            };

            return flowModel;
        }


        /// <summary>
        /// 验证流程权限
        /// </summary>
        /// <returns></returns>
        protected static Result CheckStatus(FlowModel flowModel, DealEnum dealEnum)
        {
            Result result = new Result();
            result.code = 1;
            int FStatus = 0;
            try
            {
                //获取当前状态
                DataTable dt = ModelOpretion.SearchDataRetunDataTable(string.Format(@" select FStatus from {0} 
where {1} = @FID  ", flowModel.TableName, flowModel.KeyFiledName), new {  FID = flowModel.FID });
                if (dt.Rows.Count > 0)
                {
                    FStatus = dt.Rows[0][0].ToSafeInt32(0);
                }
                //判断权限
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
            FlowDTO FirstNode = GetFirstNode(flowModel);
            if (FirstNode.FLevel == 0)
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

            //数据库level和前端level是否一致
            bool ex = ModelOpretion.ScalarDataExist(string.Format(@" select * from {0}
                where fid=@FID and FCheckLevel=@FCurrentLevel ", flowModel.TableName), new { FID = flowModel.FID, FCurrentLevel = flowModel.FCurrentLevel });

            if(!ex)
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
                return result;
            }

            //验证用户是否有审核权限
            bool haveFlow = ModelOpretion.ScalarDataExist(@"select flow.* from t_sys_Flow flow
                                                    inner join t_sys_RolesForFlow rf on  rf.FFlowID=flow.FID
                                                    inner join t_sys_RolesForUser ru on ru.FRoleID=rf.FRoleID
                                                    inner join t_sys_Users u on u.FID=ru.FUserID
                                                    where flow.FLevel=@FLevel
                                                    and u.FID=@FUserID and ISNULL(u.FIsDeleted,0)=0", new { FLevel = flowModel.FCurrentLevel, FUserID = flowModel.UserID });
            if (!haveFlow)
            {
                result.code = 0;
                result.message = "没有该审核权限。";
                return result;
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
            if (FStatus == 0)
            {
                result.code = 0;
                result.message = "当前状态不允许驳回";
                return result;
            }
            

            //验证用户是否有审核权限
            bool haveFlow = ModelOpretion.ScalarDataExist(@"select flow.* from t_sys_Flow flow
                                                    inner join t_sys_RolesForFlow rf on  rf.FFlowID=flow.FID
                                                    inner join t_sys_RolesForUser ru on ru.FRoleID=rf.FRoleID
                                                    inner join t_sys_Users u on u.FID=ru.FUserID
                                                    where flow.FLevel=@FLevel
                                                    and u.FID=@FUserID and ISNULL(u.FIsDeleted,0)=0", new { FLevel = flowModel.FCurrentLevel, FUserID = flowModel.UserID });
            if (!haveFlow)
            {
                result.code = 0;
                result.message = "没有该审核权限。";
                return result;
            }

            return result;
        }

        /// <summary>
        /// 获取流程首节点
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        protected static FlowDTO GetFirstNode(FlowModel flowModel)
        {
            FlowDTO firstNode = new FlowDTO();
            DataTable dt = ModelOpretion.SearchDataRetunDataTable(@" Select Top 1 * From t_sys_Flow Where FBillTypeID=@TBillTypeID Order By FLevel ", new { TBillTypeID = flowModel.FBillTypeID });
            if (dt.Rows.Count > 0)
            {
                firstNode.FLevel = dt.Rows[0]["FLevel"].ToSafeInt32(0);
                firstNode.FLevelName = dt.Rows[0]["FName"].ToSafeString();
                firstNode.FNextLevel = dt.Rows[0]["FNextLevel"].ToSafeInt32(0);
                firstNode.FTableName = dt.Rows[0]["FTableName"].ToSafeString();
                firstNode.FCheckTableName = dt.Rows[0]["FCheckTable"].ToSafeString();
                firstNode.FKeyName= dt.Rows[0]["FKeyName"].ToSafeString();
            }
            return firstNode;
        }

        /// <summary>
        /// 提交的内置业务逻辑
        /// </summary>
        /// <returns></returns>
        protected static Result DoSubmitBase(FlowModel flowModel)
        {
            SqlDataAccess da = DataAccessFactory.CreateSqlDataAccessWriter(conn);
            da.Open();
            Result result = new Result();
            result.code = 0;
            FlowDTO firstNode= GetFirstNode(flowModel);
            using (IDbTransaction t = da.BeginTransaction())
            {
                try
                {
                    //首先先更改当前记录信息
                    SqlQuery queryUpdate = new SqlQuery();
                    StringBuilder builder = new StringBuilder();
                    builder.Append("UPDATE ");
                    builder.Append(flowModel.TableName);
                    builder.Append(" SET ");
                    builder.Append("FStatus=1, ");
                    builder.Append("FCheckLevel=@FCheckLevel,");
                    builder.Append("FCheckName=@FCheckName,");
                    builder.Append("FNextCheckLevel=@FNextCheckLevel ");
                    builder.Append(" where FID=@FID ");
                    queryUpdate.CommandText = builder.ToString();
                    queryUpdate.Parameters.Add("@FCheckLevel", firstNode.FLevel, SqlDbType.Int, 4);
                    queryUpdate.Parameters.Add("@FCheckName", firstNode.FLevelName, SqlDbType.NVarChar, 100);
                    queryUpdate.Parameters.Add("@FNextCheckLevel", firstNode.FNextLevel, SqlDbType.Int, 4);
                    queryUpdate.Parameters.Add("@FID", flowModel.FID, SqlDbType.Int, 4);

                    SqlQuery queryCheck = new SqlQuery();
                    StringBuilder builderCheck = new StringBuilder();
                    builderCheck.Append(@" INSERT INTO   ");
                    builderCheck.Append(flowModel.FCheckTable);
                    builderCheck.Append(" ( ");
                    builderCheck.Append("[FBillTypeID]");
                    builderCheck.Append(",[FBillID]");
                    builderCheck.Append(",[FLevelName]");
                    builderCheck.Append(",[FLevel]");
                    builderCheck.Append(",[FNextLevel]");
                    builderCheck.Append(",[FNextLevelName]");
                    builderCheck.Append(",[FRemark]");
                    builderCheck.Append(",[FAddTime]");
                    builderCheck.Append(",[FAddUserID]");
                    builderCheck.Append(" ) ");
                    builderCheck.Append(" VALUES ");
                    builderCheck.Append(" ( ");
                    builderCheck.Append(" @FBillTypeID ");
                    builderCheck.Append(" ,@FBillID ");
                    builderCheck.Append(" ,@FLevelName ");
                    builderCheck.Append(" ,@FLevel ");
                    builderCheck.Append(" ,@FNextLevel ");
                    builderCheck.Append(" ,@FNextLevelName ");
                    builderCheck.Append(" ,@FRemark ");
                    builderCheck.Append(" ,@FAddTime ");
                    builderCheck.Append(" ,@FAddUserID ");
                    builderCheck.Append(" ) ");
                    queryCheck.CommandText = builderCheck.ToString();
                    queryCheck.Parameters.Add("@FBillTypeID", flowModel.FBillTypeID, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FBillID", flowModel.FID, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FLevelName", "提交", SqlDbType.NVarChar, 100);
                    queryCheck.Parameters.Add("@FLevel", 0, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FNextLevel", firstNode.FLevel, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FNextLevelName", string.Empty, SqlDbType.NVarChar, 100);
                    queryCheck.Parameters.Add("@FRemark", string.Empty, SqlDbType.NVarChar, 100);
                    queryCheck.Parameters.Add("@FAddTime", DateTime.Now, SqlDbType.DateTime);
                    queryCheck.Parameters.Add("@FAddUserID", flowModel.UserID);

                    if (da.ExecuteNonQuery(queryUpdate) > 0 && da.ExecuteNonQuery(queryCheck) > 0)
                    {

                        result.code = 1;
                        t.Commit();
                    }
                    else
                    {
                        result.code = 0;
                        result.message = "提交失败。";
                        t.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    result.code = 0;
                    result.message = "提交失败。";
                    t.Rollback();
                    throw ex;
                }
                finally
                {
                    da.Close();
                }
            }

            return result;
        }


        protected static Result DoAdoptBase(FlowModel flowModel)
        {
            SqlDataAccess da = DataAccessFactory.CreateSqlDataAccessWriter(conn);
            da.Open();
            Result result = new Result();
            result.code = 0;

            //判断是否审核完成
            tsysFlowInfo flowInfo = ModelOpretion.FirstOrDefault<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID && p.FLevel == flowModel.FCurrentLevel);
            int FNextStatus = 1;
            if (flowInfo.FNextLevel==0)
            {
                FNextStatus = 2;
            }
            tsysFlowInfo nextFlowInfo = ModelOpretion.FirstOrDefault<tsysFlowInfo>(p => p.FBillTypeID == flowModel.FBillTypeID && p.FLevel == flowModel.FNextLevel);

            using (IDbTransaction t = da.BeginTransaction())
            {
                try
                {
                    //首先先更改当前记录信息
                    SqlQuery queryUpdate = new SqlQuery();
                    StringBuilder builder = new StringBuilder();
                    builder.Append("UPDATE ");
                    builder.Append(flowModel.TableName);
                    builder.Append(" SET ");
                    builder.Append("FStatus=@FStatus, ");
                    builder.Append("FCheckLevel=@FCheckLevel,");
                    builder.Append("FCheckName=@FCheckName,");
                    builder.Append("FNextCheckLevel=@FNextCheckLevel ");
                    builder.Append(" where FID=@FID ");
                    queryUpdate.CommandText = builder.ToString();
                    queryUpdate.Parameters.Add("@FStatus",FNextStatus,SqlDbType.Int);
                    queryUpdate.Parameters.Add("@FCheckLevel", nextFlowInfo.FLevel.ToSafeInt32(0), SqlDbType.Int, 4);
                    queryUpdate.Parameters.Add("@FCheckName", nextFlowInfo.FName, SqlDbType.NVarChar, 100);
                    queryUpdate.Parameters.Add("@FNextCheckLevel", nextFlowInfo.FNextLevel, SqlDbType.Int, 4);
                    queryUpdate.Parameters.Add("@FID", flowModel.FID, SqlDbType.Int, 4);

                    SqlQuery queryCheck = new SqlQuery();
                    StringBuilder builderCheck = new StringBuilder();
                    builderCheck.Append(@" INSERT INTO   ");
                    builderCheck.Append(flowModel.FCheckTable);
                    builderCheck.Append(" ( ");
                    builderCheck.Append("[FBillTypeID]");
                    builderCheck.Append(",[FBillID]");
                    builderCheck.Append(",[FLevelName]");
                    builderCheck.Append(",[FLevel]");
                    builderCheck.Append(",[FNextLevel]");
                    builderCheck.Append(",[FNextLevelName]");
                    builderCheck.Append(",[FRemark]");
                    builderCheck.Append(",[FAddTime]");
                    builderCheck.Append(",[FAddUserID]");
                    builderCheck.Append(" ) ");
                    builderCheck.Append(" VALUES ");
                    builderCheck.Append(" ( ");
                    builderCheck.Append(" @FBillTypeID ");
                    builderCheck.Append(" ,@FBillID ");
                    builderCheck.Append(" ,@FLevelName ");
                    builderCheck.Append(" ,@FLevel ");
                    builderCheck.Append(" ,@FNextLevel ");
                    builderCheck.Append(" ,@FNextLevelName ");
                    builderCheck.Append(" ,@FRemark ");
                    builderCheck.Append(" ,@FAddTime ");
                    builderCheck.Append(" ,@FAddUserID ");
                    builderCheck.Append(" ) ");
                    queryCheck.CommandText = builderCheck.ToString();
                    queryCheck.Parameters.Add("@FBillTypeID", flowModel.FBillTypeID, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FBillID", flowModel.FID, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FLevelName", flowInfo.FName, SqlDbType.NVarChar, 100);
                    queryCheck.Parameters.Add("@FLevel", flowInfo.FLevel, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FNextLevel", flowInfo.FNextLevel, SqlDbType.Int);
                    queryCheck.Parameters.Add("@FNextLevelName", string.Empty, SqlDbType.NVarChar, 100);
                    queryCheck.Parameters.Add("@FRemark", flowModel.FlowMessage, SqlDbType.NVarChar, 100);
                    queryCheck.Parameters.Add("@FAddTime", DateTime.Now, SqlDbType.DateTime);
                    queryCheck.Parameters.Add("@FAddUserID", flowModel.UserID);

                    if (da.ExecuteNonQuery(queryUpdate) > 0 && da.ExecuteNonQuery(queryCheck) > 0)
                    {

                        result.code = 1;
                        t.Commit();
                    }
                    else
                    {
                        result.code = 0;
                        result.message = "审核失败。";
                        t.Rollback();
                    }
                }
                catch(Exception ex)
                {
                    result.code = 0;
                    result.message = "审核失败。";
                    t.Rollback();
                    throw ex;
                }
                finally
                {
                    da.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 驳回的内置业务逻辑(不确定有没有通用性，先不写)
        /// </summary>
        /// <param name="flowModel"></param>
        /// <returns></returns>
        protected static Result DoRejectBase(FlowModel flowModel)
        {
            //SqlDataAccess da = DataAccessFactory.CreateSqlDataAccessWriter(conn);
            //da.Open();
            Result result = new Result();
            result.code = 1;

            return result;
        }
    }
}
