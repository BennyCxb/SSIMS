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
    // 文件名：BaseAttachmentType.cs
    // 文件功能描述：【附件类型配置】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/3/18 21:27:19
    //-----------------------------------*/
    [TableName("t_Base_AttachmentType")]
    public class BaseAttachmentTypeInfo  : BaseModel
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
            /// 附件类型名
            /// </summary>
            [Display(Name = @"附件类型名")]
            public int? FName{ get; set; }
            /// <summary>
            /// 文件类型配置
            /// </summary>
            [Display(Name = @"文件类型配置")]
            public string FFileType{ get; set; }
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
            public int? FModifyUser{ get; set; }
            /// <summary>
            /// 修改时间
            /// </summary>
            [Display(Name = @"修改时间")]
            public System.DateTime? FModifyTime{ get; set; }
            /// <summary>
            /// 是否删除
            /// </summary>
            [Display(Name = @"是否删除")]
            public int? FIsDeleted{ get; set; }
    }
}