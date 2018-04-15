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
            //string FBillNo = dy.FBillNo;
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
            string sql = @"
";


            var qu = ModelOpretion.PageData(sql);
            qu.Where(" isnull(a.FIsDeleted,0)=0 and a.FBillTypeID=@FBillTypeID ", new { FBillTypeID = FBillTypeID });

            #endregion

            #region 查询条件

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
    }
}
