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
using TZManageAPI.DTO;
using AutoMapper;
using Newtonsoft.Json;

namespace TZManageAPI.Controllers
{
    /// <summary>
    /// 老旧城区控制器
    /// </summary>
    [JwtAuthActionFilter]
    public class OldCityController : BaseController
    {
        /// <summary>
        /// 获取老旧城区列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result GetList([FromBody]JObject obj)
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
                DataTable dt = LoanOldCityBll.GetList(UserInfo, dy, curr, pageSize, out totalCount);

                result.code = 1;
                result.@object = dt;
                result.page = new page(curr, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(ex,"获取老旧城区列表出错"+ ex.Message);
                result.code = 0;
                result.message = "获取列表出错";
            }

            return result;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="FID"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCity(int FID)
        {
            Result result = new Result();
            LoanOldCityInfo oldCityInfo = ModelOpretion.FirstOrDefault<LoanOldCityInfo>(FID);

            if (oldCityInfo.FID > 0)
            {
                result.code = 1;
                result.@object = oldCityInfo;
            }
            else
            {
                result.code = 0;
                result.message = "没有找到该项目！";
            }
            return result;
        }


        /// <summary>
        /// 保存老旧城区基本信息表单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result SaveOldCity([FromBody]LoanOldCityInfo info)
        {
            Result result = new Result();
            result.code = 0;

            try
            {
                //修改
                if (info.FID > 0)
                {
                    LoanOldCityInfo oldCity = ModelOpretion.FirstOrDefault<LoanOldCityInfo>(info.FID);

                    //只有保存状态可以修改数据
                    //if (oldCity.FStatus != 0)
                    //{
                    //    result.code = 0;
                    //    result.message = "当前状态不允许修改！";
                    //    return result;
                    //}

                    oldCity.FAgencyValue = info.FAgencyValue;
                    oldCity.FAgencyName = BaseAgencyBll.GetAgencyByID(info.FAgencyValue).FName;
                    oldCity.FAreaName = info.FAreaName;
                    oldCity.FOccupy = info.FOccupy;
                    oldCity.FRespLeader = info.FRespLeader;
                    oldCity.FGPS = info.FGPS;
                    oldCity.FIndustry = info.FIndustry;
                    oldCity.FTownValue = info.FTownValue;
                    oldCity.FTownName = BaseStreetBll.GetStreetInfoByValue(info.FTownValue).FName;
                    oldCity.FTotalAcreage = info.FTotalAcreage;
                    oldCity.FLinkMan = info.FLinkMan;
                    oldCity.FLinkMobile = info.FLinkMobile;
                    oldCity.FEntrepreneurCount = info.FEntrepreneurCount;
                    oldCity.FPosition = info.FPosition;
                    oldCity.FNonConBuildingArea = info.FNonConBuildingArea;
                    oldCity.FRemark = info.FRemark;
                    oldCity.FCityChangeType = info.FCityChangeType;
                    oldCity.FTownChangeType = info.FTownChangeType;
                    oldCity.FAfterChange = info.FAfterChange;
                    oldCity.FTotalInvestAmount = info.FTotalInvestAmount;
                    oldCity.FAfterChangeArea = info.FAfterChangeArea;
                    oldCity.FChangeBeginDate = info.FChangeBeginDate;
                    oldCity.FChangeEndDate = info.FChangeEndDate;
                    oldCity.FChangeRemark = info.FChangeRemark;
                    oldCity.FDemonstration = info.FDemonstration;
                    oldCity.FChangeYear = info.FChangeYear;


                    oldCity.FModifyTime = DateTime.Now;
                    oldCity.FModifyUserID = UserInfo.UserId;
                    
                    int k = oldCity.Update().Submit();
                    
                    //改造方式1或2的新增5条改造进度
                    if (oldCity.FCityChangeType == 1 || oldCity.FCityChangeType == 2)
                    {
                        //判断是否已存在改造进度，不存在则新增
                        bool haveExtend = ModelOpretion.ScalarDataExist(@" select * from t_Loan_OldCityExtend12 
                                    where FLoanID = @FLoanID ", new { FLoanID = oldCity.FID });
                        if(!haveExtend)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                LoanOldCityExtend12Info extendInfo = new LoanOldCityExtend12Info()
                                {
                                    FAddTime = DateTime.Now,
                                    FAddUserID = UserInfo.UserId,
                                    FBillTypeID = 2000011,
                                    FLoanID = oldCity.FID,
                                    FStatus = i
                                };
                                extendInfo.SaveOnSubmit();
                            }
                        }
                    }

                    if (k > 0)
                    {
                        result.code = 1;
                        result.@object = oldCity.FID;
                        result.message = "修改成功";
                    }
                }//新增
                else
                {
                    LoanOldCityInfo oldCity = new LoanOldCityInfo();
                    
                    oldCity.FAgencyValue = info.FAgencyValue;
                    oldCity.FAgencyName = BaseAgencyBll.GetAgencyByID(info.FAgencyValue).FName;
                    oldCity.FAreaName = info.FAreaName;
                    oldCity.FOccupy = info.FOccupy;
                    oldCity.FRespLeader = info.FRespLeader;
                    oldCity.FGPS = info.FGPS;
                    oldCity.FIndustry = info.FIndustry;
                    oldCity.FTownValue = info.FTownValue;
                    oldCity.FTownName = BaseStreetBll.GetStreetInfoByValue(info.FTownValue).FName;
                    oldCity.FTotalAcreage = info.FTotalAcreage;
                    oldCity.FLinkMan = info.FLinkMan;
                    oldCity.FLinkMobile = info.FLinkMobile;
                    oldCity.FEntrepreneurCount = info.FEntrepreneurCount;
                    oldCity.FPosition = info.FPosition;
                    oldCity.FNonConBuildingArea = info.FNonConBuildingArea;
                    oldCity.FRemark = info.FRemark;
                    oldCity.FCityChangeType = info.FCityChangeType;
                    oldCity.FTownChangeType = info.FTownChangeType;
                    oldCity.FAfterChange = info.FAfterChange;
                    oldCity.FTotalInvestAmount = info.FTotalInvestAmount;
                    oldCity.FAfterChangeArea = info.FAfterChangeArea;
                    oldCity.FChangeBeginDate = info.FChangeBeginDate;
                    oldCity.FChangeEndDate = info.FChangeEndDate;
                    oldCity.FChangeRemark = info.FChangeRemark;
                    oldCity.FDemonstration = info.FDemonstration;
                    oldCity.FChangeYear = info.FChangeYear;

                    oldCity.FBillTypeID = info.FBillTypeID;
                    oldCity.FIsDeleted = 0;
                    oldCity.FAddUserID = UserInfo.UserId;
                    oldCity.FAddTime = DateTime.Now;
                    oldCity.FStatus = 0;
                    int id = oldCity.SaveOnSubmit();
                    if (id > 0)
                    {
                        //改造方式1或2的新增5条改造进度
                        if (oldCity.FCityChangeType == 1|| oldCity.FCityChangeType==2)
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                LoanOldCityExtend12Info extendInfo = new LoanOldCityExtend12Info()
                                {
                                    FAddTime = DateTime.Now,
                                    FAddUserID = UserInfo.UserId,
                                    FBillTypeID = 2000011,
                                    FLoanID = id,
                                    FStatus = i
                                };
                                extendInfo.SaveOnSubmit();
                            }
                        }

                        result.code = 1;
                        result.@object = id;
                        result.message = "添加成功！";
                    }
                }


            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(ex,"保存老旧城区表单出错：" + ex.Message );
                result.code = 0;
                result.message = "保存老旧城区表单出错";
            }

            return result;
        }

        ///// <summary>
        ///// 改造方式1或2保存（弃用）
        ///// </summary>
        ///// <param name="arr"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[BtLog]
        //public Result SaveOldCityExtend12(JArray arr)
        //{
        //    Result result = new Result() { code = 1 };

        //    try
        //    {
        //        IList<OldCityExtend12DTO> list = arr.ToObject<List<OldCityExtend12DTO>>();

        //        foreach (OldCityExtend12DTO d in list)
        //        {
        //            LoanOldCityExtend12Info extendInfo = Mapper.Map<OldCityExtend12DTO, LoanOldCityExtend12Info>(d);
        //            extendInfo.FModifyTime = DateTime.Now;
        //            extendInfo.FModifyUser = UserInfo.UserId;
        //            extendInfo.Update().Submit();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        LogService.Default.Fatal(ex, "改造进度保存失败:" + ex.Message);
        //        result.code = 0;
        //        result.message = "改造进度保存失败";
        //    }
        //    return result;
        //}


        /// <summary>
        /// 改造方式1或2保存
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result SaveOldCityExtend12(JObject obj)
        {
            Result result = new Result() { code = 1 };

            try
            {
                OldCityExtend12DTO dto = obj.ToObject<OldCityExtend12DTO>();
                LoanOldCityExtend12Info extendInfo = Mapper.Map<OldCityExtend12DTO, LoanOldCityExtend12Info>(dto);
                extendInfo.FModifyTime = DateTime.Now;
                extendInfo.FModifyUser = UserInfo.UserId;
                extendInfo.Update(p=>new object[]
                {
                    p.FArea1,
                    p.FArea2,
                    p.FModifyTime,
                    p.FModifyUser,
                    p.FTime
                }).Submit();
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal(ex, "改造进度保存失败:" + ex.Message);
                result.code = 0;
                result.message = "改造进度保存失败";
            }
            return result;
        }




        /// <summary>
        /// 获取改造方式1或2 实体
        /// </summary>
        /// <param name="FLoanID">表单主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityExtend12List(int FLoanID)
        {
            Result result = new Result() { code = 1 };
            
            try
            {
                List<LoanOldCityExtend12Info> list = ModelOpretion.ModelList<LoanOldCityExtend12Info>(p => p.FLoanID == FLoanID).OrderBy(p=>p.FStatus).ToList();
                
                IList<OldCityExtend12DTOShow> listDTO = Mapper.Map<List<LoanOldCityExtend12Info>, List<OldCityExtend12DTOShow>>(list);

                //获取主表改造进度
                LoanOldCityInfo oldInfo = ModelOpretion.FirstOrDefault<LoanOldCityInfo>(FLoanID);


                result.@object = listDTO;
                result.message = oldInfo.FChangeStatus.ToSafeString();
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal(ex,"获取改造方式1或2 失败"+ex.Message);
                result.code = 0;
                result.message = "获取改造进度（方式1或2） 失败";
            }
            
            return result;
        }


        /// <summary>
        /// 上报改造方式1或2
        /// </summary>
        /// <param name="FID">改造方式1或2实体ID</param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result SubmitOldCityExtend12(int FID)
        {
            Result result = new Result() { code = 1 };

            try
            {
                LoanOldCityExtend12Info info = ModelOpretion.FirstOrDefault<LoanOldCityExtend12Info>(FID);
                if(info.FID>0)
                {
                    if(info.FSubmitStatus==1)
                    {
                        result.code = 0;
                        result.message = "该状态不允许上报";
                    }
                    else
                    {
                        info.FSubmitStatus = 1;
                        info.FModifyTime = DateTime.Now;
                        info.Update(p => new object[]
                        {
                            p.FSubmitStatus,
                            p.FModifyTime
                        }).Submit();

                        //修改主表改造进度
                        LoanOldCityInfo oldInfo = ModelOpretion.FirstOrDefault<LoanOldCityInfo>(info.FLoanID);
                        oldInfo.FChangeStatus = info.FStatus;
                        oldInfo.FChangeStatusTimeNow = DateTime.Now;
                        oldInfo.Update(p => new object[]
                        {
                            p.FChangeStatus,
                            p.FChangeStatusTimeNow
                        }).Submit();
                    }
                }
                else
                {
                    result.code = 0;
                    result.message = "该实体不存在";
                }
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("改造方式1或2上报失败："+ex.Message);
                result.code = 0;
                result.message = "上报失败。";
            }


            return result;
        }
       



        /// <summary>
        /// 保存老旧城区改造进度（改造方式为3）
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result SaveOldCityExtend3(JObject obj)
        {
            Result result = new Result() { code = 1 };

            try
            {
                OldCityExtend3DTO dto = obj.ToObject<OldCityExtend3DTO>();
                LoanOldCityExtend3Info info = Mapper.Map<OldCityExtend3DTO, LoanOldCityExtend3Info>(dto);


                //修改
                if (info.FID > 0)
                {
                    info.FModifyTime = DateTime.Now;
                    info.FModifyUser = UserInfo.UserId;
                    info.Update(p=>new object[] 
                    {
                        p.FCompanyName,
                        p.FDoingTime,
                        p.FDoingType,
                        p.FDoneTime,
                        p.FDoneType,
                        p.FModifyTime,
                        p.FModifyUser,
                        p.FReadyArea,
                        p.FReadyType
                    }).Submit();
                }
                else
                {
                    info.FAddTime = DateTime.Now;
                    info.FAddUserID = UserInfo.UserId;
                    info.FBillTypeID = 2000012;
                    info.SaveOnSubmit();
                }

            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("企业改造进度保存失败:" + ex.Message);
                result.code = 0;
                result.message = "企业改造进度保存失败";
            }

            return result;
        }
        /// <summary>
        /// 新增单个老旧城区改造进度（改造方式为3）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result AddSingleOldCityExtend3(JObject obj)
        {
            Result result = new Result() { code = 1 };

            try
            {
                OldCityExtend3DTO dto = obj.ToObject<OldCityExtend3DTO>();
                LoanOldCityExtend3Info extendInfo = Mapper.Map<OldCityExtend3DTO, LoanOldCityExtend3Info>(dto);

                if (extendInfo.FID == 0)
                {
                    extendInfo.FAddTime = DateTime.Now;
                    extendInfo.FAddUserID = UserInfo.UserId;
                    extendInfo.FBillTypeID = 2000012;
                    extendInfo.FStatus = 0;
                    result.@object = extendInfo.SaveOnSubmit();
                }
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("企业新增进度保存失败:" + ex.Message);
                result.code = 0;
                result.message = "企业新增进度保存失败";
            }

            return result;
        }


        /// <summary>
        /// 获取改造进度列表（改造方式为3）
        /// </summary>
        /// <param name="FLoanID">表单主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result GetOldCityExtend3List(int FLoanID)
        {
            Result result = new Result() { code = 1 };

            try
            {
                List<LoanOldCityExtend3Info> list = ModelOpretion.ModelList<LoanOldCityExtend3Info>(p => p.FLoanID == FLoanID && p.FIsDeleted.ToSafeInt32(0)==0);
                
                IList<OldCityExtend3DTOShow> listDTO = Mapper.Map<List<LoanOldCityExtend3Info>, List<OldCityExtend3DTOShow>>(list);

                result.@object = listDTO;
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("获取改造进度列表失败" + ex.Message);
                result.code = 0;
                result.message = "获取改造进度列表失败";
            }

            return result;
        }

        /// <summary>
        /// 上报改造方式3
        /// </summary>
        /// <param name="FID">改造方式1或2实体ID</param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result SubmitOldCityExtend3(int FID)
        {
            Result result = new Result() { code = 1 };

            try
            {
                LoanOldCityExtend3Info info = ModelOpretion.FirstOrDefault<LoanOldCityExtend3Info>(FID);
                if (info.FID > 0)
                {
                    if (info.FStatus == 2)
                    {
                        result.code = 0;
                        result.message = "该状态不允许上报";
                    }
                    else
                    {
                        if (info.FStatus == 1)
                        {
                            info.FStatus = 2;
                        }
                        else
                        {
                            info.FStatus = 1;
                        }
                        
                        info.Update(p => new object[]
                        {
                            p.FStatus
                        }).Submit();
                        
                    }
                }
                else
                {
                    result.code = 0;
                    result.message = "该实体不存在";
                }
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("改造方式3上报失败：" + ex.Message);
                result.code = 0;
                result.message = "上报失败。";
            }


            return result;
        }



        /// <summary>
        /// 删除改造进度
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="FLoanID"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result DeleteCityExtend3(int FID,int FLoanID)
        {
            Result result = new Result() { code = 1 };
            try
            {
                LoanOldCityExtend3Info info = ModelOpretion.FirstOrDefault<LoanOldCityExtend3Info>(p => p.FID == FID && p.FLoanID == FLoanID);
                info.FIsDeleted = 1;
                info.FModifyTime = DateTime.Now;
                info.FModifyUser = UserInfo.UserId;
                info.Update(p => new object[] {
                p.FIsDeleted,
                p.FModifyTime,
                p.FModifyUser
            }).Submit();
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("删除改造进度报错" + ex.Message);
                result.code = 0;
                result.message = "删除改造进度报错";
            }

            return result;
        }

        /// <summary>
        /// 删除老旧城区
        /// </summary>
        /// <param name="FID"></param>
        /// <returns></returns>
        [HttpGet]
        [BtLog]
        public Result DeleteOldCity(int FID)
        {
            Result result = new Result() { code = 1 };
            try
            {
                LoanOldCityInfo info = ModelOpretion.FirstOrDefault<LoanOldCityInfo>(FID);
                info.FIsDeleted = 1;
                info.FModifyTime = DateTime.Now;
                info.FModifyUserID = UserInfo.UserId;
                info.Update(p => new object[] {
                    p.FIsDeleted,
                    p.FModifyUserID,
                    p.FModifyTime
                }).Submit();
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal(" 删除老旧城区表单出错 " + ex.Message);
                result.code = 0;
                result.message = "删除老旧城区表单出错";
            }

            return result;
        }

        /// <summary>
        /// 老旧城区批量退回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BtLog]
        public Result ReturnOldCity(JObject obj)
        {
            Result result = new Result() { code = 1 };
            try
            {
                LogService.Default.Debug(obj);
                List<int> ApplyIds = obj["ApplyIds"].ToObject<List<int>>();
                foreach (int id in ApplyIds)
                {
                    LoanOldCityInfo oldCity= ModelOpretion.FirstOrDefault<LoanOldCityInfo>(id);
                    oldCity.FStatus = 0;
                    oldCity.FChangeStatus = 0;
                    oldCity.Update(p => new object[] {
                        p.FStatus,
                        p.FChangeStatus
                    }).Submit();

                    IList<LoanOldCityExtend12Info> extend12List= ModelOpretion.ModelList<LoanOldCityExtend12Info>(p=>p.FLoanID==oldCity.FID);
                    foreach(LoanOldCityExtend12Info extend12 in extend12List)
                    {
                        extend12.FSubmitStatus = 0;
                        extend12.Update(p => new object[] {
                            p.FSubmitStatus
                        }).Submit();
                    }

                    IList<LoanOldCityExtend3Info> extend3List = ModelOpretion.ModelList<LoanOldCityExtend3Info>(p => p.FLoanID == oldCity.FID);
                    foreach(LoanOldCityExtend3Info extend3 in extend3List)
                    {
                        extend3.FStatus = 0;
                        extend3.Update(p => new object[] {
                            p.FStatus
                        }).Submit();
                    }
                }
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("老旧城区批量退回出错 " + ex.Message);
                result.code = 0;
                result.message = "老旧工业区块批量退回出错";
            }

            return result;
        }

    }
}