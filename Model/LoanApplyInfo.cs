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
    // 文件名：LoanApply.cs
    // 文件功能描述：【四边三化核准单】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/3/11 20:38:44
    //-----------------------------------*/
    [TableName("t_Loan_Apply")]
    public class LoanApplyInfo  : BaseModel
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
            /// 状态
            /// </summary>
            [Display(Name = @"状态")]
            public int? FStatus{ get; set; }
            /// <summary>
            /// 行政区划ID
            /// </summary>
            [Display(Name = @"行政区划Value")]
            public int? FAgencyValue { get; set; }
            /// <summary>
            /// 行政区划名
            /// </summary>
            [Display(Name = @"行政区划名")]
            public string FAgencyName{ get; set; }
            /// <summary>
            /// 年度
            /// </summary>
            [Display(Name = @"年度")]
            public int? FYear{ get; set; }
            /// <summary>
            /// 月份
            /// </summary>
            [Display(Name = @"月份")]
            public int? FMonth{ get; set; }
            /// <summary>
            /// 四边
            /// </summary>
            [Display(Name = @"四边")]
            public int? FPerimeter{ get; set; }
            /// <summary>
            /// 问题编号
            /// </summary>
            [Display(Name = @"问题编号")]
            public string FBillNo{ get; set; }
            /// <summary>
            /// 线路名称
            /// </summary>
            [Display(Name = @"线路名称")]
            public string FLineName{ get; set; }
            /// <summary>
            /// 里程桩号
            /// </summary>
            [Display(Name = @"里程桩号")]
            public string FMileage{ get; set; }
            /// <summary>
            /// 问题类型
            /// </summary>
            [Display(Name = @"问题类型")]
            public int? FProbTypeID{ get; set; }
            /// <summary>
            /// 问题描述
            /// </summary>
            [Display(Name = @"问题描述")]
            public string FProbDescribe{ get; set; }
            /// <summary>
            /// 地图定位
            /// </summary>
            [Display(Name = @"地图定位")]
            public string FGPS{ get; set; }
            /// <summary>
            /// 当前审核级次
            /// </summary>
            [Display(Name = @"当前审核级次")]
            public int? FCheckLevel{ get; set; }
            /// <summary>
            /// 当前审核级次名
            /// </summary>
            [Display(Name = @"当前审核级次名")]
            public string FCheckName{ get; set; }
            /// <summary>
            /// 下级审核级次
            /// </summary>
            [Display(Name = @"下级审核级次")]
            public int? FNextCheckLevel{ get; set; }
    }
}