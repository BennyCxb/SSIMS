using BT.Manage.Tools.Attributes;
using BT.Manage.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Manage.Model
{
    /*-----------------------------------
    // Copyright (C) 2017 备胎 版权所有。
    //
    // 文件名：LoanOldCityExtend12.cs
    // 文件功能描述：【整治方式12的扩展】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/4/22 13:53:04
    //-----------------------------------*/
    [TableName("t_Loan_OldCityExtend12")]
    public class LoanOldCityExtend12Info  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Display(Name =@"业务类型")]
        public int? FBillTypeID { get; set; }
        /// <summary>
        /// 表单主键ID
        /// </summary>
        [Display(Name =@"表单主键ID")]
        public int FLoanID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FStatus{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public System.DateTime? FTime{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public decimal? FArea1{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public decimal? FArea2{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FAddUserID{ get; set; }
            private System.DateTime _FAddTime = System.DateTime.Now;
            /// <summary>
            /// 添加时间
            /// </summary>
            [Display(Name = @"添加时间")]
            public System.DateTime FAddTime { get { return _FAddTime; } set { _FAddTime = value; } }        
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FModifyUser{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public System.DateTime? FModifyTime{ get; set; }

        /// <summary>
        /// 提交状态
        /// </summary>
        [Display(Name ="提交状态")]
        public int? FSubmitStatus { get; set; }
    }
}