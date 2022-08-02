using RPG_GAME.Application.Time;

namespace RPG_GAME.Infrastructure.Time
{
    internal sealed class Clock : IClock
    {
        public DateTime CurrentDate()
        {
            return DateTime.UtcNow;
        }
    }
}
