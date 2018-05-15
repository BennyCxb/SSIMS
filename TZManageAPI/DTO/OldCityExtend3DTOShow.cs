using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TZManageAPI.DTO
{
    public class OldCityExtend3DTOShow
    {
        /// <summary>
        /// 主键ID 自增
        /// </summary>
        public int FID { get; set; }

        /// <summary>
        /// 主表单ID
        /// </summary>
        public int FLoanID { get; set; }
        /// <summary>
        /// 企业名
        /// </summary>
        public string FCompanyName { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public int? FReadyType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? FReadyArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? FDoingType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? FDoingTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? FDoneType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? FDoneTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? FStatus { get; set; }
    }
}