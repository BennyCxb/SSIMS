using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TZManageAPI.DTO
{
    public class OldCityExtend12DTOShow
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int FID { get; set; }

        /// <summary>
        /// 表单主键ID
        /// </summary>
        public int FLoanID { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public int FStatus { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? FTime { get; set; }

        /// <summary>
        /// 面积1
        /// </summary>
        public decimal? FArea1 { get; set; }

        /// <summary>
        /// 面积2
        /// </summary>
        public decimal? FArea2 { get; set; }

        /// <summary>
        /// 提交状态
        /// </summary>
        public int? FSubmitStatus { get; set; }
    }
}