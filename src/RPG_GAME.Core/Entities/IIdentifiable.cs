namespace RPG_GAME.Core.Entities
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
    }
}
