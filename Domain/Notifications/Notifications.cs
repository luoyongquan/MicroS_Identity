using Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    [Table("_Notification")]
    public class NotificationBase : FullAuditedEntity<long>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        [StringLength(50)]
        public string Type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(2000)]
        public string Content { get; set; }
        /// <summary>
        /// 跳转的url地址
        /// </summary>
        [StringLength(250)]
        public string Url { get; set; }
        public bool IsRead { get; set; } = false;
        /// <summary>
        /// 发送人
        /// </summary>
        public long? PushUserId { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public long ReceiveUserId { get; set; }
    }
}
