using RPG_GAME.Application.Time;

namespace RPG_Game.Infrastructure.Time
{
    internal sealed class Clock : IClock
    {
        public DateTime CurrentDate()
        {
            return DateTime.UtcNow;
        }
    }
}
