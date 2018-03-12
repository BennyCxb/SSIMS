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
    public class BaseEnumValueBll : BaseBll
    {
        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <param name="TypeName">枚举类型名</param>
        /// <returns></returns>
        public static DataTable GetEnumList(string TypeName)
        {
            DataTable dt= ModelOpretion.SearchDataRetunDataTable(@"select ev.FValue,ev.FName from t_Base_EnumValue ev 
left join t_Base_EnumType et on et.FID=ev.FEnumTypeID
where et.FName=@EnumType
order by ev.FIndex asc ",new { EnumType=TypeName });
            return dt;
        }


        /// <summary>
        /// 根据类型名和枚举值获取枚举信息
        /// </summary>
        /// <param name="TypeName"></param>
        /// <param name="FValue"></param>
        /// <returns></returns>
        public static BaseEnumValueInfo GetEnumInfo(string TypeName,int FValue)
        {
            BaseEnumValueInfo info = DbContext.Sql(@" select ev.* from t_Base_EnumValue ev 
left join t_Base_EnumType et on et.FID=ev.FEnumTypeID
where et.FName=@TypeName and ev.FValue=@FValue ").Parameter("TypeName", TypeName).Parameter("FValue", FValue).QuerySingle<BaseEnumValueInfo>();

            if(info==null)
            {
                info = new BaseEnumValueInfo();
            }

            return info;
        }

    }
}
