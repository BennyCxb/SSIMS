using BLL.Common;
using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZManageAPI.DTO;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;
using BT.Manage.Core;

namespace BLL
{
    public class LoanOldCityBll : BaseBll
    {
        /// <summary>
        /// 获取老旧城区列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="dy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static DataTable GetList(LoginDataDto userInfo, dynamic dy, int pageIndex, int pageSize, out int totalCount)
        {
            #region 查询参数
            string FStatus = dy.FStatus;
            string FAgencyValue = dy.FAgencyValue;
            string FTownValue = dy.FTownValue;
            string FCityChangeType = dy.FCityChangeType;
            string FTownChangeType = dy.FTownChangeType;
            string FAfterChange = dy.FAfterChange;
            string FChangeBeginDate = dy.FChangeBeginDate;
            string FChangeEndDate = dy.FChangeEndDate;
            string FAreaName = dy.FAreaName;
            string FYear = dy.FYear;
            string FMonth = dy.FMonth;
            string FDemonstration = dy.FDemonstration;

            string FBillTypeID = dy.FBillTypeID;
            string strSortFiled = dy.SortFiled;
            string strSortType = dy.SortType;

            #endregion


            #region sql
            string sql = @"select o.FID,es.FName FStatus,o.FAgencyName,YEAR(o.FChangeBeginDate) FYear,MONTH(o.FChangeBeginDate) FMonth
                ,o.FTownName,O.FAreaName
                ,CONVERT(nvarchar(32), o.FChangeBeginDate,23) FChangeBeginDate,convert(nvarchar(32), o.FChangeEndDate,23) FChangeEndDate
                ,ect.FName FCityChangeType,O.FTownChangeType,
                eaf.FName FAfterChange,
                (case when o.FDemonstration='1' then '是' 
					else '否' end 
                ) FDemonstration,
                 (case when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=1 then '已启动' 
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=2 then '已签约' 
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=3 then '已拆除'
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=4 then '已开工'
	                when (o.FCityChangeType=1 or o.FCityChangeType=2) and o.FChangeStatus=5 then '已完工'
                    when o.FCityChangeType=3 and   isnull(o3.FTotalCount,0)=0   then '无'
	                when o.FCityChangeType=3 and   isnull(o3.FFinishedCount,0)<isnull(o3.FTotalCount,0)  then '拆除/整治中'
	                when o.FCityChangeType=3 and o3.FFinishedCount=o3.FTotalCount and o3.FTotalCount>0 then '已拆除/整治完成'
	                else '无'
	                end ) FProgress
                from t_loan_OldCity o
                left join 
                ( select et.FName AS FETName,ev.FValue,ev.FName 
			                from t_Base_EnumType et 
			                left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID 
                ) ect on ect.FValue=O.FCityChangeType and ect.FETName='按台州市办法分类'
                left join 
                ( select et.FName AS FETName,ev.FValue,ev.FName 
			                from t_Base_EnumType et 
			                left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID 
                ) eaf on eaf.FValue=o.FAfterChange and eaf.FETName='改造后用途'
                left join 
                ( select et.FName AS FETName,ev.FValue,ev.FName 
			                from t_Base_EnumType et 
			                left join t_Base_EnumValue ev on ev.FEnumTypeID=et.FID 
                ) es on es.FValue=o.FStatus and es.FETName='审核状态'
                left join 
                (
	                select FLoanID,sum(case when isnull(FStatus,0)=2 then 1 else 0 end) FFinishedCount ,COUNT(0) FTotalCount from t_Loan_OldCityExtend3
                    where ISNULL(FIsDeleted,0)=0
	                group by FLoanID 
                ) o3 on o3.FLoanID=o.FID

";


            var qu = ModelOpretion.PageData(sql);
            qu.Where(" isnull(o.FIsDeleted,0)=0 and o.FBillTypeID=@FBillTypeID ", new { FBillTypeID = FBillTypeID });

            #endregion

            #region 查询条件
            //审核状态
            if(!string.IsNullOrWhiteSpace(FStatus))
            {
                qu.Where(" o.FStatus=@FStatus " ,new{ FStatus = FStatus });
            }
            //行政区划
            if(!string.IsNullOrWhiteSpace(FAgencyValue))
            {
                qu.Where(" o.FAgencyValue=@FAgencyValue ", new { FAgencyValue = FAgencyValue });
            }
            //乡镇街道
            if(!string.IsNullOrWhiteSpace(FTownValue))
            {
                qu.Where(" o.FTownValue=@FTownValue ", new { FTownValue = FTownValue });
            }
            //市改造方式
            if(!string.IsNullOrWhiteSpace(FCityChangeType))
            {
                qu.Where(" o.FCityChangeType=@FCityChangeType ", new { FCityChangeType = FCityChangeType });
            }
            //县级改造方式
            if(!string.IsNullOrWhiteSpace(FTownChangeType))
            {
                qu.Where(" o.FTownChangeType like '%'+ @FTownChangeType +'%' ", new { FTownChangeType = FTownChangeType });
            }
            //改造后用途
            if(!string.IsNullOrWhiteSpace(FAfterChange))
            {
                qu.Where(" o.FAfterChange=@FAfterChange ", new { FAfterChange = FAfterChange });
            }
            //拟开始时间
            if(!string.IsNullOrWhiteSpace(FChangeBeginDate))
            {
                qu.Where(" convert(nvarchar(32),o.FChangeBeginDate,23)>=@FChangeBeginDate ", new { FChangeBeginDate = FChangeBeginDate });
            }
            //拟结束时间
            if(!string.IsNullOrWhiteSpace(FChangeEndDate))
            {
                qu.Where(" convert(nvarchar(32),o.FChangeEndDate,23)<=@FChangeEndDate ", new { FChangeEndDate = FChangeEndDate });
            }
            //区块名称
            if(!string.IsNullOrWhiteSpace(FAreaName))
            {
                qu.Where(" o.FAreaName like '%'+@FAreaName+'%' ", new { FAreaName = FAreaName });
            }
            //年度
            if(!string.IsNullOrWhiteSpace(FYear))
            {
                qu.Where(" YEAR(o.FChangeBeginDate)=@FYear ", new { FYear = FYear });
            }
            //月度
            if (!string.IsNullOrWhiteSpace(FMonth))
            {
                qu.Where(" MONTH(o.FChangeBeginDate)=@FMonth ", new { FMonth = FMonth });
            }
            //是否示范项目
            if(!string.IsNullOrWhiteSpace(FDemonstration))
            {
                qu.Where(" o.FDemonstration=@FDemonstration ", new { FDemonstration = FDemonstration });
            }


            #endregion

            #region 权限相关

            //市级只看上报完成的
            if(userInfo.FLevel==2)
            {
                qu.Where(" o.FStatus>=1 ", new {});
            }

            if (userInfo.FLevel == 3 || userInfo.FLevel == 4)
            {
                qu.Where(@" o.FAgencyValue=@FAgencyValueOp    ", new { FAgencyValueOp = userInfo.FAgencyValue });
            }

            #endregion 

            #region 排序
            if (!string.IsNullOrEmpty(strSortFiled) && !string.IsNullOrEmpty(strSortType))
            {
                if (strSortType.ToLower() == "desc")
                { qu.ThenDESC("o." + strSortFiled); }
                else
                { qu.ThenASC("o." + strSortFiled); }
            }
            else
            {
                qu.ThenDESC("o.FID");
            }
            #endregion

            totalCount = qu.TotalCount;
            DataTable dt1 = qu.Take(pageSize).Skip(pageIndex).ToDataTable();
            totalCount = qu.TotalCount;
            return dt1;
        }
    }
}
