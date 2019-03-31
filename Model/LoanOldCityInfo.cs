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
    // 文件名：LoanOldCity.cs
    // 文件功能描述：【老旧城区改造】实体
    // 创建人：LiaoYu
    // 创建标识： 2018/4/14 19:23:51
    //-----------------------------------*/
    [TableName("t_Loan_OldCity")]
    public class LoanOldCityInfo  : BaseModel
    {
            /// <summary>
            /// 主键ID 自增
            /// </summary>
            [Key]
            [Identity]
            [Display(Name = @" 主键ID 自增 ")]
            public int FID { get; set; }
            /// <summary>
            /// 单据类型
            /// </summary>
            [Display(Name = @"单据类型")]
            public int FBillTypeID{ get; set; }
            /// <summary>
            /// 区块名称
            /// </summary>
            [Display(Name = @"区块名称")]
            public string FAreaName{ get; set; }
            /// <summary>
            /// 县市区
            /// </summary>
            [Display(Name = @"县市区")]
            public string FAgencyValue{ get; set; }
            /// <summary>
            /// 县市区名称
            /// </summary>
            [Display(Name = @"县市区名称")]
            public string FAgencyName{ get; set; }
            /// <summary>
            /// 总占地
            /// </summary>
            [Display(Name = @"总占地")]
            public decimal? FOccupy{ get; set; }
            /// <summary>
            /// 责任领导
            /// </summary>
            [Display(Name = @"责任领导")]
            public string FRespLeader{ get; set; }
            /// <summary>
            /// 定位地图
            /// </summary>
            [Display(Name = @"定位地图")]
            public string FGPS{ get; set; }
            /// <summary>
            /// 主要产业
            /// </summary>
            [Display(Name = @"主要产业")]
            public string FIndustry{ get; set; }
            /// <summary>
            /// 乡镇街道
            /// </summary>
            [Display(Name = @"乡镇街道")]
            public string FTownValue{ get; set; }
            /// <summary>
            /// 乡镇街道名称
            /// </summary>
            [Display(Name = @"乡镇街道名称")]
            public string FTownName{ get; set; }
            /// <summary>
            /// 总建筑面积
            /// </summary>
            [Display(Name = @"总建筑面积")]
            public decimal? FTotalAcreage{ get; set; }
            /// <summary>
            /// 联系人
            /// </summary>
            [Display(Name = @"联系人")]
            public string FLinkMan{ get; set; }
            /// <summary>
            /// 联系电话
            /// </summary>
            [Display(Name = @"联系电话")]
            public string FLinkMobile{ get; set; }
            /// <summary>
            /// 企业家数
            /// </summary>
            [Display(Name = @"企业家数")]
            public int? FEntrepreneurCount{ get; set; }
            /// <summary>
            /// 位置
            /// </summary>
            [Display(Name = @"位置")]
            public string FPosition{ get; set; }
            /// <summary>
            /// 违建面积
            /// </summary>
            [Display(Name = @"违建面积")]
            public decimal? FNonConBuildingArea{ get; set; }
            /// <summary>
            /// 改造前区块情况简介
            /// </summary>
            [Display(Name = @"改造前区块情况简介")]
            public string FRemark{ get; set; }
            private System.DateTime _FAddTime = System.DateTime.Now;
            /// <summary>
            /// 添加时间
            /// </summary>
            [Display(Name = @"添加时间")]
            public System.DateTime FAddTime { get { return _FAddTime; } set { _FAddTime = value; } }        
            /// <summary>
            /// 新增用户
            /// </summary>
            [Display(Name = @"新增用户")]
            public int? FAddUserID{ get; set; }
            /// <summary>
            /// 是否删除
            /// </summary>
            [Display(Name = @"是否删除")]
            public int? FIsDeleted{ get; set; }
            /// <summary>
            /// 市级改造方式
            /// </summary>
            [Display(Name = @"市级改造方式")]
            public int? FCityChangeType{ get; set; }
            /// <summary>
            /// 县级改造方式
            /// </summary>
            [Display(Name = @"县级改造方式")]
            public string FTownChangeType{ get; set; }
            /// <summary>
            /// 改造后用途
            /// </summary>
            [Display(Name = @"改造后用途")]
            public int? FAfterChange{ get; set; }
            /// <summary>
            /// 拟投资总金额
            /// </summary>
            [Display(Name = @"拟投资总金额")]
            public decimal? FTotalInvestAmount{ get; set; }
            /// <summary>
            /// 改造后总建筑面积
            /// </summary>
            [Display(Name = @"改造后总建筑面积")]
            public decimal? FAfterChangeArea{ get; set; }
            /// <summary>
            /// 拟启动日期
            /// </summary>
            [Display(Name = @"拟启动日期")]
            public System.DateTime? FChangeBeginDate{ get; set; }
            /// <summary>
            /// 拟完成日期
            /// </summary>
            [Display(Name = @"拟完成日期")]
            public System.DateTime? FChangeEndDate{ get; set; }
            /// <summary>
            /// 改造方案简介
            /// </summary>
            [Display(Name = @"改造方案简介")]
            public string FChangeRemark{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FStatus{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FCheckLevel{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public string FCheckName{ get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Display(Name = @"")]
            public int? FNextCheckLevel{ get; set; }

        /// <summary>
        /// 修改用户ID
        /// </summary>
        [Display(Name =@"修改用户ID")]
        public int? FModifyUserID { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name =@"修改时间")]
        public DateTime? FModifyTime { get; set; }

        /// <summary>
        /// 改造进度
        /// </summary>
        [Display(Name =@"改造进度")]
        public int? FChangeStatus { get; set; }

        /// <summary>
        /// 十大老旧工业区块改造示范点
        /// </summary>
        [Display(Name = @"十大老旧工业区块改造示范点")]
        public string FDemonstration { get; set; }

        /// <summary>
        /// 年份（用于统计筛选）
        /// </summary>
        [Display(Name = @"年份（用于统计筛选）")]
        public int? FChangeYear { get; set; }

        [Display(Name =@"最新状态时间")]
        public DateTime? FChangeStatusTimeNow { get; set; }

    }
}