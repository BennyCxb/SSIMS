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
using Newtonsoft.Json;

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
        [BtLog]
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

                if (dt.Rows.Count == 0)
                {
                    LogService.Default.Debug("没有获取到列表的记录:"+UserInfo.FAgencyValue+"入参:"+JsonConvert.SerializeObject(dy));
                }

                result.code = 1;
                result.@object = dt;
                result.page = new page(curr,pageSize,totalCount);
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal(ex,"获取省级问题列表出错",ex.Message);
                result.code = 0;
                result.message = "获取省级问题列表出错";
            }

            return result;
        }


        /// <summary>
        /// 获取核准单实体
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetApplyInfo(int FID)
        {
            Result result = new Result();
            LoanApplyInfo applyInfo= ModelOpretion.FirstOrDefault<LoanApplyInfo>(FID);

            if(applyInfo.FID>0)
            {
                result.code = 1;
                result.@object = applyInfo;
            }
            else
            {
                result.code = 0;
                result.message = "没有找到该问题！";
            }
            

            return result;
        }

        /// <summary>
        /// 保存省级问题表单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result SaveSJApply([FromBody]LoanApplyInfo info)
        {
            Result result = new Result();
            result.code = 0;
            //if(string.IsNullOrWhiteSpace(info.FBillNo))
            //{
            //    result.code = 0;
            //    result.message = "请检查表单数据！";
            //    return result;
            //}
            
            try
            {
                //修改
                if (info.FID > 0)
                {
                    LoanApplyInfo apply= ModelOpretion.FirstOrDefault<LoanApplyInfo>(info.FID);

                    //只有保存状态可以修改数据
                    if (apply.FStatus != 0)
                    {
                        result.code = 0;
                        result.message = "当前状态不允许修改！";
                        return result;
                    }

                    apply.FAgencyValue = info.FAgencyValue;
                    apply.FAgencyName = ModelOpretion.FirstOrDefault<BaseAgencyInfo>(p => p.FValue == info.FAgencyValue).FName;
                    apply.FGPS = info.FGPS;
                    apply.FLineName = info.FLineName;
                    apply.FMileage = info.FMileage;
                    apply.FModifyTime = DateTime.Now;
                    apply.FModifyUserID= UserInfo.UserId;
                    apply.FMonth = info.FMonth;
                    apply.FPerimeter = info.FPerimeter;
                    apply.FProbDescribe = info.FProbDescribe;
                    apply.FProbTypeID = info.FProbTypeID;
                    apply.FRemark = info.FRemark;
                    apply.FTwon = info.FTwon;
                    apply.FYear = info.FYear;
                    //问题编号有修改
                    if(info.FBillNo!=apply.FBillNo)
                    {
                        //检验问题编号是否已存在
                        bool exBillInfo = ModelOpretion.ScalarDataExist(@" select * from t_Loan_Apply
                                                        where FBillNo = @FBillNo AND FBillTypeID=@FBillTypeID AND ISNULL(FIsDeleted,0)=0 ", new { FBillNo = info.FBillNo, FBillTypeID = info.FBillTypeID });
                        if (exBillInfo)
                        {
                            result.code = 0;
                            result.message = "该问题编号已存在！";
                            return result;
                        }
                    }
                    apply.FBillNo = info.FBillNo;


                    int k = apply.SaveOnSubmit();
                    if (k > 0)
                    {
                        result.code = 1;
                        result.@object = apply.FID;
                        result.message = "修改成功";
                    }
                }//新增
                else
                {
                    //检验问题编号是否已存在
                    bool exBillInfo = ModelOpretion.ScalarDataExist(@" select * from t_Loan_Apply
                                                        where FBillNo = @FBillNo AND FBillTypeID=@FBillTypeID AND ISNULL(FIsDeleted,0)=0  ", new { FBillNo = info.FBillNo, FBillTypeID = info.FBillTypeID });
                    if (exBillInfo)
                    {
                        result.code = 0;
                        result.message = "该问题编号已存在！";
                        return result;
                    }
                    info.FAddUserID = UserInfo.UserId;
                    info.FAddTime = DateTime.Now;
                    info.FStatus = 0;
                    info.FAgencyName = ModelOpretion.FirstOrDefault<BaseAgencyInfo>(p=>p.FValue==info.FAgencyValue).FName;
                    //info.FBillNo= BillNo.GetBillNo(info.FBillTypeID, info.FAgencyValue);
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
                LogService.Default.Fatal(ex,"保存问题表单出错：" +ex.Message);
                result.code = 0;
                result.message = "保存问题表单出错";
            }

            return result;
        }

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="FID"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result DeleteApply(int FID)
        {
            Result result = new Result();
            result.code = 1;
            LoanApplyInfo applyInfo = ModelOpretion.FirstOrDefault<LoanApplyInfo>(FID);
            if (FID<=0)
            {
                result.code = 0;
                result.message = "该问题不存在或已删除！";
                return result;
            }

            if(applyInfo.FStatus.ToSafeInt32(0)!=0)
            {
                result.code = 0;
                result.message = "当前状态不允许删除！";
                return result;
            }
            
            applyInfo.FIsDeleted = 1;
            applyInfo.SaveOnSubmit();
            result.code = 1;
            return result;
        }

       

       


        /// <summary>
        /// 获取桥下利用空间等列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result GetQXList([FromBody]JObject obj)
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
                DataTable dt = LoanApplyBll.GetQXApplyList(UserInfo, dy, curr, pageSize, out totalCount);

                result.code = 1;
                result.@object = dt;
                result.page = new page(curr, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("获取列表出错", ex.Message, ex);
                result.code = 0;
                result.message = "获取列表出错";
            }

            return result;
        }



        /// <summary>
        /// 保存桥下利用空间等表单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result SaveQXApply([FromBody]LoanApplyInfo info)
        {
            Result result = new Result();
            result.code = 0;

            try
            {
                //修改
                if (info.FID > 0)
                {
                    LoanApplyInfo apply = ModelOpretion.FirstOrDefault<LoanApplyInfo>(info.FID);

                    //只有保存状态可以修改数据
                    if (apply.FStatus != 0)
                    {
                        result.code = 0;
                        result.message = "当前状态不允许修改！";
                        return result;
                    }

                    apply.FAgencyValue = info.FAgencyValue;
                    apply.FAgencyName = ModelOpretion.FirstOrDefault<BaseAgencyInfo>(p => p.FValue == info.FAgencyValue).FName;
                    apply.FGPS = info.FGPS;
                    apply.FMileage = info.FMileage;
                    apply.FModifyTime = DateTime.Now;
                    apply.FModifyUserID = UserInfo.UserId;
                    apply.FMonth = info.FMonth;
                    apply.FRemark = info.FRemark;
                    apply.FTwon = info.FTwon;
                    apply.FYear = info.FYear;
                    apply.FSynopsis = info.FSynopsis;
                    apply.FPorjectName = info.FPorjectName;
                    apply.FProjectTypeID = info.FProjectTypeID;
                    apply.FInvestment = info.FInvestment;
                    apply.FPlanDate = info.FPlanDate;
                    apply.FLength = info.FLength;
                    apply.FAcreage = info.FAcreage;
                    apply.FAccountabilityUnit = info.FAccountabilityUnit;
                    apply.FLiablePerson = info.FLiablePerson;
                    apply.FMobile = info.FMobile;
                    apply.FPurpose = info.FPurpose;
                    apply.FIsSpecialProject = info.FIsSpecialProject;
                    //问题编号有修改
                    if (info.FBillNo != apply.FBillNo)
                    {
                        //检验问题编号是否已存在
                        bool exBillInfo = ModelOpretion.ScalarDataExist(@" select * from t_Loan_Apply
                                                        where FBillNo = @FBillNo AND FBillTypeID=@FBillTypeID AND ISNULL(FIsDeleted,0)=0  ", new { FBillNo = info.FBillNo, FBillTypeID = info.FBillTypeID });
                        if (exBillInfo)
                        {
                            result.code = 0;
                            result.message = "该问题编号已存在！";
                            return result;
                        }
                    }
                    apply.FBillNo = info.FBillNo;

                    int k = apply.SaveOnSubmit();
                    if (k > 0)
                    {
                        result.code = 1;
                        result.@object = apply.FID;
                        result.message = "修改成功";
                    }
                }//新增
                else
                {
                    //检验问题编号是否已存在
                    bool exBillInfo = ModelOpretion.ScalarDataExist(@" select * from t_Loan_Apply
                                                        where FBillNo = @FBillNo AND FBillTypeID=@FBillTypeID AND ISNULL(FIsDeleted,0)=0  ", new { FBillNo = info.FBillNo, FBillTypeID= info.FBillTypeID });
                    if(exBillInfo)
                    {
                        result.code = 0;
                        result.message = "该问题编号已存在！";
                        return result;
                    }
                    info.FAddUserID = UserInfo.UserId;
                    info.FAddTime = DateTime.Now;
                    info.FStatus = 0;
                    info.FAgencyName = ModelOpretion.FirstOrDefault<BaseAgencyInfo>(p => p.FValue == info.FAgencyValue).FName;
                    //info.FBillNo = BillNo.GetBillNo(info.FBillTypeID, info.FAgencyValue);
                    int id = info.SaveOnSubmit();
                    if (id > 0)
                    {
                        result.code = 1;
                        result.@object = id;
                        result.message = "添加成功！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("保存表单出错：", ex.Message, ex);
                result.code = 0;
                result.message = "保存表单出错";
            }

            return result;
        }
        
    }
}