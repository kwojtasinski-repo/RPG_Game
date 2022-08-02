namespace RPG_GAME.Infrastructure.Mongo.Documents
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
    }
}
