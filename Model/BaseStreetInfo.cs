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
    // 文件名：BaseStreet.cs
    // 文件功能描述：【街道】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/4/15 13:13:54
    //-----------------------------------*/
    [TableName("t_Base_Street")]
    public class BaseStreetInfo  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }
            /// <summary>
            /// 街道编号
            /// </summary>
            [Display(Name = @"街道编号")]
            public string FValue{ get; set; }
            /// <summary>
            /// 街道名
            /// </summary>
            [Display(Name = @"街道名")]
            public string FName{ get; set; }
            /// <summary>
            /// 行政区划号
            /// </summary>
            [Display(Name = @"行政区划号")]
            public string FAgencyValue{ get; set; }
            /// <summary>
            /// 新增用户ID
            /// </summary>
            [Display(Name = @"新增用户ID")]
            public int? FAddUserID{ get; set; }
            private System.DateTime _FAddTime = System.DateTime.Now;
            /// <summary>
            /// 添加时间
            /// </summary>
            [Display(Name = @"添加时间")]
            public System.DateTime FAddTime { get { return _FAddTime; } set { _FAddTime = value; } }        
            /// <summary>
            /// 修改用户ID
            /// </summary>
            [Display(Name = @"修改用户ID")]
            public int? FModifyUserID{ get; set; }
            /// <summary>
            /// 修改时间
            /// </summary>
            [Display(Name = @"修改时间")]
            public System.DateTime? FModifyTime{ get; set; }
    }
}