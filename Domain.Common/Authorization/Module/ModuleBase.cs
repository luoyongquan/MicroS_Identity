using Core.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Authorization.Module
{
    /// <summary>
    /// 模块
    /// </summary>
    [Table("_Module")]
    public abstract class ModuleBase : FullAuditedEntity<long>
    {
        
        [Display(Name = "模块代码")]
        [Required(ErrorMessage = "模块代码必填")]
        [StringLength(50, ErrorMessage = "模块代码长度不能超过50个字符")]
        public virtual string Code { get; set; }

        [Display(Name = "模块名称")]
        [Required(ErrorMessage = "模块名称必填")]
        [StringLength(50, ErrorMessage = "模块名称长度不能超过50个字符")]
        public virtual string Name { get; set; }

        [Display(Name = "模块请求地址")]
        public virtual string ActionUrl { get; set; }

        [Display(Name = "模块图标")]
        public virtual string Image { get; set; }

        [Display(Name = "排序")]
        public virtual int OrderSort { get; set; }

        //[Display(Name = "父模块")]
        public long? ParentId { get; set; }



    }
}