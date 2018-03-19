using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TZManageAPI.DTO
{
    /// <summary>
    /// 附件相关
    /// </summary>
    public class FileDTO
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string FileUrl { get; set; }
    }
}