using System;

namespace Order.Domain.Common
{
    public abstract class Entity
    {
        int? _requestedHashCode;
        Guid _id;
        public virtual Guid Id
        {
            get => _id;
            protected set => _id = value;
        }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an Id)
        /// </summary>
        public bool IsTransient() => this.Id == default(Guid);


        public override bool Equals(object obj)
        {
            //TODO: Check EF Proxy entityies equality
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (IsTransient())
            {
                return base.GetHashCode();
            }
            else
            {
                if (_requestedHashCode.HasValue)
                    return _requestedHashCode.Value;

                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution
                return _requestedHashCode.Value;
            }
        }
    }
}
