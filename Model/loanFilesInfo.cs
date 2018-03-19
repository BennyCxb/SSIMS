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
    // 文件名：loanFiles.cs
    // 文件功能描述：【文件记录表】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/3/18 21:27:19
    //-----------------------------------*/
    [TableName("t_loan_Files")]
    public class loanFilesInfo  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }
            /// <summary>
            /// 是否上传到七牛
            /// </summary>
            [Display(Name = @"是否上传到七牛")]
            public int? FIsQiNiu{ get; set; }
            /// <summary>
            /// 文件名
            /// </summary>
            [Display(Name = @"文件名")]
            public string FFileName{ get; set; }
            /// <summary>
            /// 本地文件地址(非七牛)
            /// </summary>
            [Display(Name = @"本地文件地址(非七牛)")]
            public string FFilePath{ get; set; }
            /// <summary>
            /// 七牛文件key
            /// </summary>
            [Display(Name = @"七牛文件key")]
            public string FQiNiuKey{ get; set; }
            /// <summary>
            /// 单据类型
            /// </summary>
            [Display(Name = @"单据类型")]
            public int? FBillTypeID{ get; set; }
            /// <summary>
            /// 单据主键ID
            /// </summary>
            [Display(Name = @"单据主键ID")]
            public int? FLoanID{ get; set; }
            private System.DateTime _FAddTime = System.DateTime.Now;
            /// <summary>
            /// 添加时间
            /// </summary>
            [Display(Name = @"添加时间")]
            public System.DateTime FAddTime { get { return _FAddTime; } set { _FAddTime = value; } }        
            /// <summary>
            /// 新增用户ID
            /// </summary>
            [Display(Name = @"新增用户ID")]
            public int? FAddUserID{ get; set; }
    }
}