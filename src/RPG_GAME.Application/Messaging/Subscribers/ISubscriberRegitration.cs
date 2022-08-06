namespace RPG_GAME.Application.Messaging.Subscribers
{
    public interface ISubscriberRegitration
    {
        IEnumerable<BroadcastRegistration> GetBroadcastRegistrations(string key);
        void AddBroadcastAction(Type requestType, Func<object, Task> action);
    }
}
