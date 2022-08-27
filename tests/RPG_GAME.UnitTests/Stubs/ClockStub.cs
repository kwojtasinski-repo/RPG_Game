using RPG_GAME.Application.Time;
using System;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class ClockStub : IClock
    {
        public DateTime CurrentDate()
        {
            return new DateTime(2022, 8, 27, 11, 10, 20);
        }
    }
}
