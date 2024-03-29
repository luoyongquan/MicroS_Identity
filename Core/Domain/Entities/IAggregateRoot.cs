using System.Collections.Generic;
//using Core.Events.Bus;

namespace Core.Domain.Entities
{
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {

    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {

    }

    public interface IGeneratesDomainEvents
    {
        //ICollection<IEventData> DomainEvents { get; }
    }
}