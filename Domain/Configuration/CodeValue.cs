using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities.Auditing;


namespace Domain.Configuration
{
    /// <summary>
    /// 数据字典项
    /// </summary>
    [Table("_CodeValue")]
    public class CodeValue : FullAuditedEntity<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 对应值
        /// </summary>
        [StringLength(100)]
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(300)]
        public string Remarks { get; set; }
        
        /// <summary>
        /// 关联字典ID
        /// </summary>
        public long MainId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderSort { get; set; }

        /// <summary>
        /// 关联字典
        /// </summary>
        public virtual CodeMain CodeMain { get; set; }
    }
}