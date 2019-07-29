using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auditing
{
    /// <summary>
    /// AuditType 字段的枚举
    /// </summary>
    public enum AuditingTypeEnum
    {
        [Display(Name = "系统")]
        Default = 0,
        [Display(Name = "程序")]
        Programer = 1,
    }
}
