namespace RPG_GAME.Core.NewEntities
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
    }
}
