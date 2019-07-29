using Core.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Authorization.Module
{
    [Table("_RoleModule")]
    public abstract class RoleModuleBase : FullAuditedEntity<long>
    {
        [Display(Name = "用户角色"), Required]

        public long RoleId { get; set; }

        [Display(Name = "模块"), Required]

        public long ModuleId { get; set; }

        //[Display(Name = "是否展示模块")]
        //public bool Display { get; set; }
    }
}