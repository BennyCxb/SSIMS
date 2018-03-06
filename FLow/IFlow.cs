using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Frame.Base;

namespace FLow
{
    public interface IFlow
    {
        /// <summary>
        /// 提交动作前
        /// </summary>
        /// <returns></returns>
        Result BeforeSubmit(FlowModel flowModel);

        /// <summary>
        /// 提交动作后
        /// </summary>
        /// <returns></returns>
        Result AfterSubmit(FlowModel flowModel);

        /// <summary>
        /// 审核动作前
        /// </summary>
        /// <returns></returns>
        Result BeforeAdopt(FlowModel flowModel);

        /// <summary>
        /// 审核动作后
        /// </summary>
        /// <returns></returns>
        Result AfterAdopt(FlowModel flowModel);

        /// <summary>
        /// 驳回动作前
        /// </summary>
        /// <returns></returns>
        Result BeforeReject(FlowModel flowModel);

        /// <summary>
        /// 驳回动作后
        /// </summary>
        /// <returns></returns>
        Result AfterReject(FlowModel flowModel);
    }
}
