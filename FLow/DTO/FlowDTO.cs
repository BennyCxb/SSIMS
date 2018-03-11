using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLow.DTO
{
    public class FlowDTO
    {
        /// <summary>
        /// 当前流程
        /// </summary>
        public int FLevel { get; set; }

        /// <summary>
        /// 当前流程名
        /// </summary>
        public string FLevelName { get; set; }

        /// <summary>
        /// 下级流程
        /// </summary>
        public int FNextLevel { get; set; }

        /// <summary>
        /// 主表名
        /// </summary>
        public string FTableName { get; set; }

        /// <summary>
        /// 流程记录表名
        /// </summary>
        public string FCheckTableName { get; set; }

        /// <summary>
        /// 主键名
        /// </summary>
        public string FKeyName { get; set; }
        
    }
}
