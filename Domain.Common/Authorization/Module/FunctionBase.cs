using Core.Domain.Entities.Auditing;
using Domain.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Authorization.CommonFunctionBase
{
    /// <summary>
    /// 方法
    /// </summary>
    [Table("_Function")]
    public abstract class FunctionBase : FullAuditedEntity<long>
    {

        [Display(Name = "动作")]
        [StringLength(50)]
        public string Action { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "样式名")]
        public string Classes { get; set; }

        [Display(Name = "图标")]
        public string LayUiIcon { get; set; }

        [Display(Name = "类型")]
        [DefaultValue((int)FunctionTypeEnum.Common)]
        public FunctionTypeEnum Type { get; set; }

        [Display(Name = "模块ID")]
        public long? ModuleId { get; set; }
        [Display(Name = "排序")]
        public int OrderSort { get; set; }

    }
}