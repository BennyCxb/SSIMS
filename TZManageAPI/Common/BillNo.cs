using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BT.Manage.Core;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;
using BT.Manage.Model;
using System.Data;

namespace TZManageAPI.Common
{
    public class BillNo
    {
        public static string GetBillNo(int FBillTypeID,string FAgencyValue)
        {
            string billNo = string.Empty;
            BaseAgencyInfo agencyInfo= ModelOpretion.FirstOrDefault<BaseAgencyInfo>(p=>p.FValue==FAgencyValue);
            string middleString = agencyInfo.FShortName;

            //县级问题编号里没有缩写（时间问题写死）
            if (FBillTypeID== 1000013)
            {
                middleString = string.Empty;
            }
            
            DateTime date= DateTime.Now;
            DataTable dt= ModelOpretion.SearchDataRetunDataTable(" exec [dbo].[Prc_GetBillNo] @FBillTypeID,@FDate,@FMiddlefix  ",new { FBillTypeID = FBillTypeID , FDate =date, FMiddlefix = middleString });
            if(dt.Rows.Count>0)
            {
                billNo = dt.Rows[0][0].ToSafeString();
            }

            if(string.IsNullOrWhiteSpace(billNo))
            {
                throw new Exception("获取问题编号失败！");
            }

            return billNo;
        }
    }
}