using System;
using System.Collections.Generic;
using MediatR;

namespace Delivery.Domain.Common
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents;
        private int? _requestedHashCode;

        public virtual Guid Id { get; protected set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        ///     Checks if this entity is transient (not persisted to database and it has not an Id)
        /// </summary>
        public bool IsTransient()
        {
            return Id == default;
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public override bool Equals(object obj)
        {
            //TODO: Check EF Proxy entityies equality
            if (obj == null || !(obj is Entity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity) obj;

            if (item.IsTransient() || IsTransient())
                return false;
            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (IsTransient()) return base.GetHashCode();

            if (_requestedHashCode.HasValue)
                return _requestedHashCode.Value;

            _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution
            return _requestedHashCode.Value;
        }
    }
}