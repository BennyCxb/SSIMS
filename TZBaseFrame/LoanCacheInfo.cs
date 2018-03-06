using BT.Manage.Tools.Attributes;
using BT.Manage.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZManage.Model
{
    /*-----------------------------------
    // Copyright (C) 2017 备胎 版权所有。
    //
    // 文件名：LoanCache.cs
    // 文件功能描述：【伪缓存表】实体
    // 创建人：ly
    // 创建标识： 2018/3/3 星期六 17:33:18
    //-----------------------------------*/
    [TableName("t_Loan_Cache")]
    public class LoanCacheInfo  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }
            /// <summary>
            /// 缓存键
            /// </summary>
            [Display(Name = @"缓存键")]
            public string FCacheKey{ get; set; }
            /// <summary>
            /// 缓存值
            /// </summary>
            [Display(Name = @"缓存值")]
            public string FCacheValue{ get; set; }
            /// <summary>
            /// 缓存持续时间（分钟）
            /// </summary>
            [Display(Name = @"缓存持续时间（分钟）")]
            public int? FCacheTime{ get; set; }
            /// <summary>
            /// 新增时间
            /// </summary>
            [Display(Name = @"新增时间")]
            public System.DateTime FAddTime{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FIsDeleted{ get; set; }
    }
}