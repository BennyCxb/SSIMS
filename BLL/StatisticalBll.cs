using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Tools.Utils;
using TZManageAPI.DTO;
using BT.Manage.Core;

namespace BLL
{
    public class StatisticalBll
    {
        public static DataTable GetList(string FTime)
        {
            
            #region sql
            string sql = @"select * from t_Loan_ProgressStatisHistory
                        where FAddTime=@FTime
                        order by FSort asc ,AGSort asc
    ";
            var dt= ModelOpretion.SearchDataRetunDataTable(sql, new { FTime = FTime });
            #endregion
            
            
            return dt;
        }


        public static DataTable GetTimeArray(LoginDataDto userInfo, dynamic dy, int pageIndex, int pageSize, out int totalCount)
        {
            

            #region sql
            string sql = @" select FAddTime from ( select FAddTime from t_Loan_ProgressStatisHistory
                        group by FAddTime ) a
                ";

            var qu = ModelOpretion.PageData(sql);

            #endregion

            
            #region 排序
            
            qu.ThenDESC(" FAddTime ");
            
            #endregion

            totalCount = qu.TotalCount;
            DataTable dt1 = qu.Take(pageSize).Skip(pageIndex).ToDataTable();
            totalCount = qu.TotalCount;
            return dt1;
        }
    }
}
