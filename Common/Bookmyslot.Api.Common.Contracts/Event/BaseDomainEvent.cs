using MediatR;
using System;

namespace Bookmyslot.Api.Common.Contracts.Event
{
    public abstract class BaseDomainEvent : INotification
    {
        private readonly DateTime dateOccurred;
        public BaseDomainEvent()
        {
            dateOccurred = DateTime.UtcNow;
        }

        public DateTime DateOccurred
        {
            get
            {
                return this.dateOccurred;
            }
        }
    }
}
