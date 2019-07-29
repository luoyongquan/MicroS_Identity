using Core.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Authorization.Module
{
    [Table("_ModuleFunction")]
    public abstract class ModuleFunctionBase : FullAuditedEntity<long>
    {
        [Display(Name = "模块")]

        public long? ModuleId { get; set; }

        [Display(Name = "方法")]

        public long? FunctionId { get; set; }

        //[Display(Name = "功能")]
        //[StringLength(50)]
        //public string Action { get; set; }

        //[Display(Name = "功能名称")]
        //public string Name { get; set; }

        //[Display(Name = "功能地址")]
        //public string Url { get; set; }

        //public int OrderSort { get; set; }


    }
}