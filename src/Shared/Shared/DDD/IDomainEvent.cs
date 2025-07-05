using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;

namespace Shared.DDD
{
    public interface IDomainEvent:INotification
    {
        Guid EventId=>Guid.NewGuid();
        public DateTime OccurredOn=>DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
