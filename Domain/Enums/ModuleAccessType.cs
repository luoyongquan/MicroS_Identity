using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ModuleAccessType
    {
        /// <summary>
        /// 额外
        /// </summary>
        [Display(Name = "额外")]
        More = 1,

        /// <summary>
        /// 剔除
        /// </summary>
        [Display(Name = "剔除")]
        Less = 2
    }
}
