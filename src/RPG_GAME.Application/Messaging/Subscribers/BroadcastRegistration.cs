namespace RPG_GAME.Application.Messaging.Subscribers
{
    public sealed class BroadcastRegistration
    {
        public Type ReceiverType { get; }
        public Func<object, Task> Action { get; }
        public string Key => ReceiverType.Name;

        public BroadcastRegistration(Type receiverType, Func<object, Task> action)
        {
            ReceiverType = receiverType;
            Action = action;
        }
    }
}
