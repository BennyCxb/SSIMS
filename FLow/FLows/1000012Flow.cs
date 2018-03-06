using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLow.FLows
{
    public class _1000012Flow : IFlow
    {
        /// <summary>
        /// 省级问题提交前
        /// </summary>
        /// <returns></returns>
        public Result BeforeSubmit(FlowModel flowModel)
        {
            return null;
        }

        /// <summary>
        /// 省级问题提交后
        /// </summary>
        /// <returns></returns>
        public Result AfterSubmit(FlowModel flowModel)
        {
            Result result = new Result();

            return null;
        }

        /// <summary>
        /// 省级问题审核前
        /// </summary>
        /// <returns></returns>
        public Result BeforeAdopt(FlowModel flowModel)
        {
            Result result = new Result();

            return null;
        }

        /// <summary>
        /// 省级问题审核后
        /// </summary>
        /// <returns></returns>
        public Result AfterAdopt(FlowModel flowModel)
        {
            Result result = new Result();

            return null;
        }

        /// <summary>
        /// 省级问题驳回前
        /// </summary>
        /// <returns></returns>
        public Result BeforeReject(FlowModel flowModel)
        {
            Result result = new Result();

            return null;
        }

        /// <summary>
        /// 省级问题驳回后
        /// </summary>
        /// <returns></returns>
        public Result AfterReject(FlowModel flowModel)
        {
            Result result = new Result();

            return null;
        }
    }
}
