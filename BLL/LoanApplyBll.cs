using BLL.Common;
using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZManageAPI.DTO;

namespace BLL
{
    public class LoanApplyBll : BaseBll
    {

        public Result GetApplyList(LoginDataDto userInfo,dynamic dy,int curr,int pageSize,out int totalCount )
        {
            #region 查询参数
            string billNo = dy.FBillNo;
            
            #endregion 
        }

    }
}
