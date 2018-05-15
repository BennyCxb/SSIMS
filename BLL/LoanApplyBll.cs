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
    public class LoanApplyBll : BaseBll
    {

        public static DataTable GetSJApplyList(LoginDataDto userInfo,dynamic dy,int pageIndex, int pageSize,out int totalCount )
        {
            
            #region 查询参数
            string FBillNo = dy.FBillNo;
            string FStatus = dy.FStatus;
            string FAgencyValue = dy.FAgencyValue;
            string FYear = dy.FYear;
            string FMonth = dy.FMonth;
            string FChangeStatus = dy.FChangeStatus;
            string FBillTypeID = dy.FBillTypeID;
            string strSortFiled = dy.SortFiled;
            string strSortType = dy.SortType;
            //四边
            string FEdge = dy.FEdge;
            //问题类型
            string FProbType = dy.FProbType;

            #endregion

            #region sql
            string sql = @"select a.FID,a.FBillNo,FAgencyName,a.FLineName,a.FMileage,ep.FName as FProbType,es.FName as FStatusName,a.FStatus 
,ec.FName as FChangeStatusName
from t_Loan_Apply a 
left join (select ev.FValue,ev.FName,et.FName as FTypeName from t_Base_EnumValue ev 
					left join t_Base_EnumType et on et.FID=ev.FEnumTypeID ) es on es.FTypeName='审核状态' and es.FValue=a.FStatus
left join (select ev.FValue,ev.FName,et.FName as FTypeName from t_Base_EnumValue ev 
					left join t_Base_EnumType et on et.FID=ev.FEnumTypeID ) ep on ep.FTypeName='问题类型' and ep.FValue=a.FProbTypeID
left join (select ev.FValue,ev.FName,et.FName as FTypeName from t_Base_EnumValue ev 
					left join t_Base_EnumType et on et.FID=ev.FEnumTypeID ) ec on ec.FTypeName='整改状态' and ec.FValue=ISNULL(a.FChangeStatus,0)
";

            var qu = ModelOpretion.PageData(sql);
            qu.Where(" isnull(a.FIsDeleted,0)=0 and a.FBillTypeID=@FBillTypeID ",new { FBillTypeID = FBillTypeID });

            #endregion

            #region 查询条件
            //单据编号
            if(!string.IsNullOrWhiteSpace(FBillNo))
            {
                qu.Where(@" a.FBillNo Like '%'+@FBillNo+'%' ", new { FBillNo = FBillNo });
            }
            //状态
            if(!string.IsNullOrWhiteSpace(FStatus))
            {
                qu.Where(@" a.FStatus =@FStatus ", new { FStatus = FStatus.ToSafeInt32(0) });
            }
            //行政区划
            if (!string.IsNullOrWhiteSpace(FAgencyValue))
            {
                qu.Where(@" a.FAgencyValue=@FAgencyValue ", new { FAgencyValue = FAgencyValue });
            }
            //年
            if(!string.IsNullOrWhiteSpace(FYear))
            {
                qu.Where(@" a.FYear=@FYear ", new { FYear = FYear });
            }
            //月
            if(!string.IsNullOrWhiteSpace(FMonth))
            {
                qu.Where(@" a.FMonth=@FMonth ", new { FMonth = FMonth });
            }
            //整改状态
            if(!string.IsNullOrWhiteSpace(FChangeStatus))
            {
                qu.Where(@" a.FChangeStatus=@FChangeStatus ", new { FChangeStatus = FChangeStatus });
            }
            //四边
            if(FEdge.ToSafeInt32(-1)>-1)
            {
                qu.Where(@" a.FPerimeter=@FPerimeter ", new { FPerimeter = FEdge });
            }
            //问题类型
            if(FProbType.ToSafeInt32(-1)>-1)
            {
                qu.Where(@" a.FProbTypeID=@FProbTypeID ",new { FProbTypeID= FProbType });
            }

            #endregion

            #region 权限相关

            if(userInfo.FLevel==3 || userInfo.FLevel==4)
            {
                qu.Where(@" a.FAgencyValue=@FAgencyValueOp    ", new { FAgencyValueOp = userInfo.FAgencyValue });
            }

            #endregion 


            #region 排序
            if (!string.IsNullOrEmpty(strSortFiled) && !string.IsNullOrEmpty(strSortType))
            {
                if (strSortType.ToLower() == "desc")
                { qu.ThenDESC("a." + strSortFiled); }
                else
                { qu.ThenASC("a." + strSortFiled); }
            }
            else
            {
                qu.ThenDESC("a.FID");
            }
            #endregion

            totalCount = qu.TotalCount;
            DataTable dt1 = qu.Take(pageSize).Skip(pageIndex).ToDataTable();
            totalCount = qu.TotalCount;
            return dt1;
        }


        /// <summary>
        /// 桥下利用空间等获取列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="dy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static DataTable GetQXApplyList(LoginDataDto userInfo, dynamic dy, int pageIndex, int pageSize, out int totalCount)
        {

            #region 查询参数
            string FBillNo = dy.FBillNo;
            string FAgencyValue = dy.FAgencyValue;
            string FYear = dy.FYear;
            string FMonth = dy.FMonth;
            string FBillTypeID = dy.FBillTypeID;
            string FProjectTypeID = dy.FProjectTypeID;
            string strSortFiled = dy.SortFiled;
            string strSortType = dy.SortType;

            #endregion

            #region sql
            string sql = @"select a.FID,a.FBillNo,FAgencyName,a.FPorjectName,a.FMileage,a.FLength,es.FName as FProjectType
                                from t_Loan_Apply a 
                                left join 
                                (select ev.FValue,ev.FName,et.FName as FTypeName from t_Base_EnumValue ev 
					left join t_Base_EnumType et on et.FID=ev.FEnumTypeID ) es on es.FTypeName='项目类型' and es.FValue=a.FProjectTypeID
";

            var qu = ModelOpretion.PageData(sql);
            qu.Where(" isnull(a.FIsDeleted,0)=0 and a.FBillTypeID=@FBillTypeID ", new { FBillTypeID = FBillTypeID });

            #endregion

            #region 查询条件
            //单据编号
            if (!string.IsNullOrWhiteSpace(FBillNo))
            {
                qu.Where(@" a.FBillNo Like '%'+@FBillNo+'%' ", new { FBillNo = FBillNo });
            }
            //行政区划
            if (!string.IsNullOrWhiteSpace(FAgencyValue))
            {
                qu.Where(@" a.FAgencyValue=@FAgencyValue ", new { FAgencyValue = FAgencyValue });
            }
            //年
            if (!string.IsNullOrWhiteSpace(FYear))
            {
                qu.Where(@" a.FYear=@FYear ", new { FYear = FYear });
            }
            //月
            if (!string.IsNullOrWhiteSpace(FMonth))
            {
                qu.Where(@" a.FMonth=@FMonth ", new { FMonth = FMonth });
            }
            //项目类型
            if(!string.IsNullOrWhiteSpace(FProjectTypeID))
            {
                qu.Where(@" a.FProjectTypeID=@FProjectTypeID ", new { FProjectTypeID = FProjectTypeID });
            }

            #endregion

            #region 权限相关

            if (userInfo.FLevel == 3 || userInfo.FLevel == 4)
            {
                qu.Where(@" a.FAgencyValue=@FAgencyValue    ", new { FAgencyValue = userInfo.FAgencyValue });
            }

            #endregion 


            #region 排序
            if (!string.IsNullOrEmpty(strSortFiled) && !string.IsNullOrEmpty(strSortType))
            {
                if (strSortType.ToLower() == "desc")
                { qu.ThenDESC("a." + strSortFiled); }
                else
                { qu.ThenASC("a." + strSortFiled); }
            }
            else
            {
                qu.ThenDESC("a.FID");
            }
            #endregion

            totalCount = qu.TotalCount;
            DataTable dt1 = qu.Take(pageSize).Skip(pageIndex).ToDataTable();
            totalCount = qu.TotalCount;
            return dt1;
        }



        /// <summary>
        /// 获取审核情况列表
        /// </summary>
        /// <param name="FLoanID"></param>
        /// <param name="FBillTypeID"></param>
        /// <returns></returns>
        public static DataTable GetCheckList(int FLoanID,int FBillTypeID)
        {
            DataTable dt = ModelOpretion.SearchDataRetunDataTable(@" 
                                                select c.FLevelName,c.FRemark,CONVERT(nvarchar(32),c.FAddTime,120) FAddTime,u.FName 
                                        from t_check_Apply c
                                        left join t_sys_Users u on u.FID=c.FAddUserID
                                        where FBillID=@FLoanID and FBillTypeID=@FBillTypeID
                                        order by FAddTime asc
        ", new { FLoanID= FLoanID, FBillTypeID= FBillTypeID });

            return dt;

        }

    }
}
