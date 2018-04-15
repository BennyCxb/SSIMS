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
    /// <summary>
    /// 街道相关业务层
    /// </summary>
    public class BaseStreetBll : BaseBll
    {
        /// <summary>
        /// 获取街道列表
        /// </summary>
        /// <param name="AgencyValue">行政区划编号</param>
        /// <returns></returns>
        public static DataTable GetStreetListByAgency(string AgencyValue)
        {
            DataTable dt= ModelOpretion.SearchDataRetunDataTable(@"select FValue,FName 
                        from t_Base_Street
                        where FAgencyValue=@FAgencyValue ", new { FAgencyValue = AgencyValue });

            return dt;
        }

        /// <summary>
        /// 获取街道
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static BaseStreetInfo GetStreetInfoByValue(string Value)
        {
            BaseStreetInfo info = ModelOpretion.FirstOrDefault<BaseStreetInfo>(p=>p.FValue==Value);

            return info;
        }
    }
}
