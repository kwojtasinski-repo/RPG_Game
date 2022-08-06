namespace RPG_GAME.Infrastructure.Messaging.Clients
{
    internal interface IMessageSerializer
    {
        byte[] Serialize<T>(T value);
        T Deserialize<T>(byte[] value);
        object Deserialize(byte[] value, Type type);
    }
}
