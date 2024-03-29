﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Reflection;
using Core.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.EntityFrameworkCore
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class EFCoreDbContext : DbContext
    {
        ///// <summary>
        ///// Used to get current session values.
        ///// </summary>
        //public ISession Session { get; set; }

        ///// <summary>
        ///// Used to trigger entity change events.
        ///// </summary>
        //public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        ///// <summary>
        ///// Reference to the logger.
        ///// </summary>
        //public ILogger Logger { get; set; }

        ///// <summary>
        ///// Reference to the event bus.
        ///// </summary>
        //public IEventBus EventBus { get; set; }

        ///// <summary>
        ///// Reference to GUID generator.
        ///// </summary>
        //public IGuidGenerator GuidGenerator { get; set; }

        ///// <summary>
        ///// Reference to the current UOW provider.
        ///// </summary>
        //public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        ///// <summary>
        ///// Reference to multi tenancy configuration.
        ///// </summary>
        //public IMultiTenancyConfig MultiTenancyConfig { get; set; }

        /// <summary>
        /// Can be used to suppress automatically setting TenantId on SaveChanges.
        /// Default: false.
        /// </summary>
        public virtual bool SuppressAutoSetTenantId { get; set; }

        protected virtual int? CurrentTenantId => null;// GetCurrentTenantIdOrNull();

        protected virtual bool IsSoftDeleteFilterEnabled => true;// CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(DataFilters.SoftDelete) == true;

        protected virtual bool IsMayHaveTenantFilterEnabled => true;//CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(DataFilters.MayHaveTenant) == true;

        protected virtual bool IsMustHaveTenantFilterEnabled => true;// CurrentTenantId != null && CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(DataFilters.MustHaveTenant) == true;

        private static MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(EFCoreDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Constructor.
        /// </summary>
        protected EFCoreDbContext(DbContextOptions options)
            : base(options)
        {
            InitializeDbContext();
        }

        private void InitializeDbContext()
        {
            SetNullsForInjectedProperties();
        }

        private void SetNullsForInjectedProperties()
        {
            //Logger = NullLogger.Instance;
            //Session = NullSession.Instance;
            //EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            //GuidGenerator = SequentialGuidGenerator.Instance;
            //EventBus = NullEventBus.Instance;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null && ShouldFilterEntity<TEntity>(entityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    if (entityType.IsQueryType)
                    {
                        modelBuilder.Query<TEntity>().HasQueryFilter(filterExpression);
                    }
                    else
                    {
                        modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                    }
                }
            }
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            //if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    return true;
            //}

            //if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    return true;
            //}

            return false;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted;
                expression = expression == null ? softDeleteFilter : CombineExpressions(expression, softDeleteFilter);
            }

            //if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId;
            //    expression = expression == null ? mayHaveTenantFilter : CombineExpressions(expression, mayHaveTenantFilter);
            //}

            //if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId;
            //    expression = expression == null ? mustHaveTenantFilter : CombineExpressions(expression, mustHaveTenantFilter);
            //}

            return expression;
        }

        public override int SaveChanges()
        {
            try
            {
                //var changeReport = ApplyConcepts();
                var result = base.SaveChanges();
                //EntityChangeEventHelper.TriggerEvents(changeReport);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;   //throw new DbConcurrencyException(ex.Message, ex);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //var changeReport = ApplyConcepts();
                var result = await base.SaveChangesAsync(cancellationToken);
                //await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
                //throw new DbConcurrencyException(ex.Message, ex);
            }
        }

        //protected virtual EntityChangeReport ApplyConcepts()
        //{
        //    var changeReport = new EntityChangeReport();

        //    var userId = GetAuditUserId();

        //    foreach (var entry in ChangeTracker.Entries().ToList())
        //    {
        //        if (entry.State != EntityState.Modified && entry.CheckOwnedEntityChange())
        //        {
        //            Entry(entry.Entity).State = EntityState.Modified;
        //        }

        //        ApplyConcepts(entry, userId, changeReport);
        //    }

        //    return changeReport;
        //}

        //protected virtual void ApplyConcepts(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            ApplyConceptsForAddedEntity(entry, userId, changeReport);
        //            break;
        //        case EntityState.Modified:
        //            ApplyConceptsForModifiedEntity(entry, userId, changeReport);
        //            break;
        //        case EntityState.Deleted:
        //            ApplyConceptsForDeletedEntity(entry, userId, changeReport);
        //            break;
        //    }

        //    AddDomainEvents(changeReport.DomainEvents, entry.Entity);
        //}

        //protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    CheckAndSetId(entry);
        //    CheckAndSetMustHaveTenantIdProperty(entry.Entity);
        //    CheckAndSetMayHaveTenantIdProperty(entry.Entity);
        //    SetCreationAuditProperties(entry.Entity, userId);
        //    changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Created));
        //}

        //protected virtual void ApplyConceptsForModifiedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    SetModificationAuditProperties(entry.Entity, userId);
        //    if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
        //    {
        //        SetDeletionAuditProperties(entry.Entity, userId);
        //        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        //    }
        //    else
        //    {
        //        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Updated));
        //    }
        //}

        //protected virtual void ApplyConceptsForDeletedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        //{
        //    if (IsHardDeleteEntity(entry))
        //    {
        //        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        //        return;
        //    }

        //    CancelDeletionForSoftDelete(entry);
        //    SetDeletionAuditProperties(entry.Entity, userId);
        //    changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
        //}

        //protected virtual bool IsHardDeleteEntity(EntityEntry entry)
        //{
        //    if (CurrentUnitOfWorkProvider?.Current?.Items == null)
        //    {
        //        return false;
        //    }

        //    if (!CurrentUnitOfWorkProvider.Current.Items.ContainsKey(UnitOfWorkExtensionDataTypes.HardDelete))
        //    {
        //        return false;
        //    }

        //    var hardDeleteItems = CurrentUnitOfWorkProvider.Current.Items[UnitOfWorkExtensionDataTypes.HardDelete];
        //    if (!(hardDeleteItems is HashSet<string> objects))
        //    {
        //        return false;
        //    }

        //    var currentTenantId = GetCurrentTenantIdOrNull();
        //    var hardDeleteKey = EntityHelper.GetHardDeleteKey(entry.Entity, currentTenantId);
        //    return objects.Contains(hardDeleteKey);
        //}

        //protected virtual void AddDomainEvents(List<DomainEventEntry> domainEvents, object entityAsObj)
        //{
        //    var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
        //    if (generatesDomainEventsEntity == null)
        //    {
        //        return;
        //    }

        //    if (generatesDomainEventsEntity.DomainEvents.IsNullOrEmpty())
        //    {
        //        return;
        //    }

        //    domainEvents.AddRange(generatesDomainEventsEntity.DomainEvents.Select(eventData => new DomainEventEntry(entityAsObj, eventData)));
        //    generatesDomainEventsEntity.DomainEvents.Clear();
        //}

        //protected virtual void CheckAndSetId(EntityEntry entry)
        //{
        //    //Set GUID Ids
        //    var entity = entry.Entity as IEntity<Guid>;
        //    if (entity != null && entity.Id == Guid.Empty)
        //    {
        //        var idPropertyEntry = entry.Property("Id");

        //        if (idPropertyEntry != null && idPropertyEntry.Metadata.ValueGenerated == ValueGenerated.Never)
        //        {
        //            entity.Id = GuidGenerator.Create();
        //        }
        //    }
        //}

        //protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
        //{
        //    if (SuppressAutoSetTenantId)
        //    {
        //        return;
        //    }

        //    //Only set IMustHaveTenant entities
        //    if (!(entityAsObj is IMustHaveTenant))
        //    {
        //        return;
        //    }

        //    var entity = entityAsObj.As<IMustHaveTenant>();

        //    //Don't set if it's already set
        //    if (entity.TenantId != 0)
        //    {
        //        return;
        //    }

        //    var currentTenantId = GetCurrentTenantIdOrNull();

        //    if (currentTenantId != null)
        //    {
        //        entity.TenantId = currentTenantId.Value;
        //    }
        //    else
        //    {
        //        throw new CoreException("Can not set TenantId to 0 for IMustHaveTenant entities!");
        //    }
        //}

        //protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
        //{
        //    if (SuppressAutoSetTenantId)
        //    {
        //        return;
        //    }

        //    //Only works for single tenant applications
        //    if (MultiTenancyConfig?.IsEnabled ?? false)
        //    {
        //        return;
        //    }

        //    //Only set IMayHaveTenant entities
        //    if (!(entityAsObj is IMayHaveTenant))
        //    {
        //        return;
        //    }

        //    var entity = entityAsObj.As<IMayHaveTenant>();

        //    //Don't set if it's already set
        //    if (entity.TenantId != null)
        //    {
        //        return;
        //    }

        //    entity.TenantId = GetCurrentTenantIdOrNull();
        //}

        //protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
        //{
        //    EntityAuditingHelper.SetCreationAuditProperties(MultiTenancyConfig, entityAsObj, Session.TenantId, userId);
        //}

        //protected virtual void SetModificationAuditProperties(object entityAsObj, long? userId)
        //{
        //    EntityAuditingHelper.SetModificationAuditProperties(MultiTenancyConfig, entityAsObj, Session.TenantId, userId);
        //}

        //protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        //{
        //    if (!(entry.Entity is ISoftDelete))
        //    {
        //        return;
        //    }

        //    entry.Reload();
        //    entry.State = EntityState.Modified;
        //    entry.Entity.As<ISoftDelete>().IsDeleted = true;
        //}

        //protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
        //{
        //    if (entityAsObj is IHasDeletionTime)
        //    {
        //        var entity = entityAsObj.As<IHasDeletionTime>();

        //        if (entity.DeletionTime == null)
        //        {
        //            entity.DeletionTime = Clock.Now;
        //        }
        //    }

        //    if (entityAsObj is IDeletionAudited)
        //    {
        //        var entity = entityAsObj.As<IDeletionAudited>();

        //        if (entity.DeleterUserId != null)
        //        {
        //            return;
        //        }

        //        if (userId == null)
        //        {
        //            entity.DeleterUserId = null;
        //            return;
        //        }

        //        //Special check for multi-tenant entities
        //        if (entity is IMayHaveTenant || entity is IMustHaveTenant)
        //        {
        //            //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
        //            if ((entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == Session.TenantId) ||
        //                (entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == Session.TenantId))
        //            {
        //                entity.DeleterUserId = userId;
        //            }
        //            else
        //            {
        //                entity.DeleterUserId = null;
        //            }
        //        }
        //        else
        //        {
        //            entity.DeleterUserId = userId;
        //        }
        //    }
        //}

        //protected virtual long? GetAuditUserId()
        //{
        //    if (Session.UserId.HasValue &&
        //        CurrentUnitOfWorkProvider != null &&
        //        CurrentUnitOfWorkProvider.Current != null &&
        //        CurrentUnitOfWorkProvider.Current.GetTenantId() == Session.TenantId)
        //    {
        //        return Session.UserId;
        //    }

        //    return null;
        //}

        //protected virtual int? GetCurrentTenantIdOrNull()
        //{
        //    if (CurrentUnitOfWorkProvider != null &&
        //        CurrentUnitOfWorkProvider.Current != null)
        //    {
        //        return CurrentUnitOfWorkProvider.Current.GetTenantId();
        //    }

        //    return Session.TenantId;
        //}

        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            return ExpressionCombiner.Combine(expression1, expression2);
        }
    }
}
