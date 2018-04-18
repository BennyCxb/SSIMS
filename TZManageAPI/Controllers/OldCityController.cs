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
                    if (oldCity.FStatus != 0)
                    {
                        result.code = 0;
                        result.message = "当前状态不允许修改！";
                        return result;
                    }

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


                    oldCity.FModifyTime = DateTime.Now;
                    oldCity.FModifyUserID = UserInfo.UserId;
                    


                    int k = oldCity.Update().Submit();
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

                    oldCity.FBillTypeID = info.FBillTypeID;
                    oldCity.FIsDeleted = 0;
                    oldCity.FAddUserID = UserInfo.UserId;
                    oldCity.FAddTime = DateTime.Now;
                    oldCity.FStatus = 0;
                    int id = oldCity.SaveOnSubmit();
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
                LogService.Default.Fatal(ex,"保存老旧城区表单出错：" + ex.Message );
                result.code = 0;
                result.message = "保存老旧城区表单出错";
            }

            return result;
        }


        //public Result SaveOldCityExtend12()
        //{

        //}

        

    }
}