using Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Configuration
{
    /// <summary>
    /// 数据字典
    /// </summary>
    [Table("_CodeMain")]
    public class CodeMain : FullAuditedEntity<long>
    {
   
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(50)]
        public string Remarks { get; set; }

        public virtual ICollection<CodeValue> CodeValue { get; set; } = new List<CodeValue>();
    }
}