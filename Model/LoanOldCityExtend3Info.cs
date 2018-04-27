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
    // 文件名：LoanOldCityExtend3.cs
    // 文件功能描述：【老旧城区改造进度（方式为3）】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/4/22 17:40:27
    //-----------------------------------*/
    [TableName("t_Loan_OldCityExtend3")]
    public class LoanOldCityExtend3Info  : BaseModel
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
            [Display(Name = @"业务类型")]
            public int? FBillTypeID{ get; set; }
            /// <summary>
            /// 主表单ID
            /// </summary>
            [Display(Name = @"主表单ID")]
            public int FLoanID{ get; set; }
            /// <summary>
            /// 企业名
            /// </summary>
            [Display(Name = @"企业名")]
            public string FCompanyName{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FReadyType{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public decimal? FReadyArea{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FDoingType{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public System.DateTime? FDoingTime{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FDoneType{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public System.DateTime? FDoneTime{ get; set; }
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
            public int? FAddUserID{ get; set; }
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
        /// 是否删除
        /// </summary>
        public int? FIsDeleted { get; set; }
    }
}