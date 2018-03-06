using BT.Manage.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Tools.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// 流程表实体
    /// </summary>
    [TableName("t_sys_Flow")]
    public class tsysFlowInfo : BaseModel
    {
        [Key]
        [Identity]
        [Display(Name = @"ID")]
        public int FID { get; set; }

        [Display(Name = @"单据类型")]
        public int? FBillTypeID { get; set; }

        [Display(Name = @"审批级次名称")]
        public string FName { get; set; }

        [Display(Name = @"审批级次")]
        public int FLevel { get; set; }

        [Display(Name = @"下级审批级次")]
        public int? FNextLevel { get; set; }

        [Display(Name = @"进入条件（非必要）")]
        public string FCondition { get; set; }

        [Display(Name = @"备注")]
        public string FRemark { get; set; }

        [Display(Name = @"是否删除")]
        public int? FIsDeleted { get; set; }

        [Display(Name = @"新增时间")]
        public DateTime? FAddTime { get; set; }

        [Display(Name = @"新增用户ID")]
        public int? FAddUserID { get; set; }

        [Display(Name = @"修改时间")]
        public DateTime? FModifyTime { get; set; }

        [Display(Name = @"修改用户ID")]
        public int? FModifyUserID { get; set; }

        [Display(Name=@"流程记录表名")]
        public string FCheckTable { get; set; }

        [Display(Name=@"主键名")]
        public string FKeyName { get; set; }

        [Display(Name=@"主表名")]
        public string FTableName { get; set; }
    }
}
