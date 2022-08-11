using RPG_GAME.Application.Time;
using System;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class ClockStub : IClock
    {
        public DateTime CurrentDate()
        {
            return DateTime.UtcNow;
        }
    }
}
