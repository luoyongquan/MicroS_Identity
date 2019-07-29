using Core.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Authorization.Module
{
    [Table("_RoleModuleFunction")]
    public class RoleModuleFunctionBase : FullAuditedEntity<long>
    {
        [Display(Name = "用户角色"), Required]
       
        public long RoleId { get; set; }

        [Display(Name = "模块功能"), Required]
     
        public long ModuleFunctionId { get; set; }

     
    }
}