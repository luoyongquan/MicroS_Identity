using System.Threading.Tasks;
using Core.Dependency;
using Core.Domain.Repositories;
using Core.Auditing;

namespace Domain.Auditing
{
    /// <summary>
    /// Implements <see cref="IAuditingStore"/> to save auditing informations to database.
    /// </summary>
    public class AuditingStore : IAuditingStore
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;

        /// <summary>
        /// Creates  a new <see cref="AuditingStore"/>.
        /// </summary>
        public AuditingStore(IRepository<AuditLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public virtual Task SaveAsync(AuditInfo auditInfo)
        {
            _auditLogRepository.Insert(AuditLog.CreateFromAuditInfo(auditInfo));
            _auditLogRepository.Context.Commit();
            return Task.FromResult(0);
        }
    }
}