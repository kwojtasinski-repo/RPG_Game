using System;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class ModifiedDateCannotBeBeforeException : DomainException
    {
        public DateTime CurrentModifiedDate { get; }
        public DateTime NextModifiedDate { get; }

        public ModifiedDateCannotBeBeforeException(DateTime currentModifiedDate, DateTime nextModifiedDate) : base($"ModifiedDate: '{nextModifiedDate}' cannot be before: '{currentModifiedDate}'")
        {
            CurrentModifiedDate = currentModifiedDate;
            NextModifiedDate = nextModifiedDate;
        }
    }
}
