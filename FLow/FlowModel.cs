using BT.Manage.DataAccess.SqlClient;
using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLow
{
    /// <summary>
    /// 流程泛型类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlowModel
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        public string KeyFiledName
        {
            get;
            set;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// 流程记录表名
        /// </summary>
        public string FCheckTable
        {
            get;
            set;
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int FBillTypeID
        {
            get;
            set;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int FID
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string FlowMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 当前流程
        /// </summary>
        public int FCurrentLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 下级流程
        /// </summary>
        public int FNextLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            get;
            set;
        }

        
    }
}
