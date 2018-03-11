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
    // 文件名：BaseAgency.cs
    // 文件功能描述：【行政规划】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/3/12 0:22:12
    //-----------------------------------*/
    [TableName("t_Base_Agency")]
    public class BaseAgencyInfo  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }
            /// <summary>
            /// 行政区划代码
            /// </summary>
            [Display(Name = @"行政区划代码")]
            public string FValue{ get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            [Display(Name = @"名称")]
            public string FName{ get; set; }
            /// <summary>
            /// 新增用户ID
            /// </summary>
            [Display(Name = @"新增用户ID")]
            public int? FAddUserID{ get; set; }
            /// <summary>
            /// 新增时间
            /// </summary>
            [Display(Name = @"新增时间")]
            public System.DateTime? FAddUserTime{ get; set; }
            /// <summary>
            /// 修改用户ID
            /// </summary>
            [Display(Name = @"修改用户ID")]
            public int? FModifyUserID{ get; set; }
            /// <summary>
            /// 修改时间
            /// </summary>
            [Display(Name = @"修改时间")]
            public System.DateTime? FModifyUserTime{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public string FShortName{ get; set; }
    }
}