namespace Shared.Messaging.Events
{
    public record IntegrationEvents
    {
        public Guid EventId = Guid.NewGuid();
        public DateTime OccurredOn => DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
