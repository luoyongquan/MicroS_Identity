using Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace Domain.Configuration
{
    /// <summary>
    /// 系统参数
    /// </summary>
    [Table("_ApplicationSettings")]
    public class ApplicationSettings : FullAuditedEntity<long>
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [Display(Name = "参数名称")]

        [Required(ErrorMessage = "请输入参数名称")]
        [StringLength(50)]
        public string ConfigName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Display(Name = "参数值")]
        [Required(ErrorMessage = "请输入参数值")]
        [StringLength(50)]
        public string ConfigValue { get; set; }
        [Display(Name = "排序")]
        public int OrderSort { get; set; }


    }
}