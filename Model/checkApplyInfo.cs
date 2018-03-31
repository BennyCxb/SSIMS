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
    // 文件名：checkApply.cs
    // 文件功能描述：【核准单流程记录表】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/3/31 19:26:03
    //-----------------------------------*/
    [TableName("t_check_Apply")]
    public class checkApplyInfo  : BaseModel
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
            public int FBillTypeID{ get; set; }
            /// <summary>
            /// 主表主键ID
            /// </summary>
            [Display(Name = @"主表主键ID")]
            public int? FBillID{ get; set; }
            /// <summary>
            /// 审核级次名
            /// </summary>
            [Display(Name = @"审核级次名")]
            public string FLevelName{ get; set; }
            /// <summary>
            /// 审核级次
            /// </summary>
            [Display(Name = @"审核级次")]
            public int FLevel{ get; set; }
            /// <summary>
            /// 下级审核级次
            /// </summary>
            [Display(Name = @"下级审核级次")]
            public int? FNextLevel{ get; set; }
            /// <summary>
            /// 下级审核级次名
            /// </summary>
            [Display(Name = @"下级审核级次名")]
            public string FNextLevelName{ get; set; }
            /// <summary>
            /// 审核结果
            /// </summary>
            [Display(Name = @"审核结果")]
            public int? FResult{ get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            [Display(Name = @"备注")]
            public string FRemark{ get; set; }
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