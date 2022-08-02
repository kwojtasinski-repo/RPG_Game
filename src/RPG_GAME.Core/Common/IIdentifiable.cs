namespace RPG_GAME.Core.Entities.Common
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
    }
}
