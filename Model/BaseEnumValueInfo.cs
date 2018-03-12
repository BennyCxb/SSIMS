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
    // 文件名：BaseEnumValue.cs
    // 文件功能描述：【枚举值】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/3/12 20:24:15
    //-----------------------------------*/
    [TableName("t_Base_EnumValue")]
    public class BaseEnumValueInfo  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }
            /// <summary>
            /// 枚举值
            /// </summary>
            [Display(Name = @"枚举值")]
            public int FValue{ get; set; }
            /// <summary>
            /// 枚举类型ID
            /// </summary>
            [Display(Name = @"枚举类型ID")]
            public int FEnumTypeID{ get; set; }
            /// <summary>
            /// 枚举名
            /// </summary>
            [Display(Name = @"枚举名")]
            public string FName{ get; set; }
            /// <summary>
            /// 顺序
            /// </summary>
            [Display(Name = @"顺序")]
            public int? FIndex{ get; set; }
            /// <summary>
            /// 是否删除
            /// </summary>
            [Display(Name = @"是否删除")]
            public int? FIsDeleted{ get; set; }
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
    }
}