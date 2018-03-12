using BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Core;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;
using BT.Manage.Model;

namespace BLL
{
    public class BaseAgencyBll : BaseBll
    {
        /// <summary>
        /// 获取行政区划列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAgencyList()
        {
            DataTable dt= ModelOpretion.SearchDataRetunDataTable(" select FValue,FName from t_Base_Agency ", null);

            return dt;
        }

        /// <summary>
        /// 根据ID获取行政区划详情
        /// </summary>
        /// <param name="FAgencyID"></param>
        /// <returns></returns>
        public static BaseAgencyInfo GetAgencyByID(string FAgencyValue)
        {
            BaseAgencyInfo info = ModelOpretion.FirstOrDefault<BaseAgencyInfo>(p=>p.FValue== FAgencyValue);
            return info;
        }
        
    }
}
