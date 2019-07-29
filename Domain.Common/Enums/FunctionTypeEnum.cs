using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Enums
{
    public enum FunctionTypeEnum
    {
        [Display(Name = "通用")]
        Common = 0,
        [Display(Name = "模块自定义")]
        Module = 1,
    }
}
