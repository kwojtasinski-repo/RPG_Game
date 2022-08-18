namespace RPG_GAME.Infrastructure.Mongo.Documents.Battles.Actions
{
    internal class FightActionDocument
    {
        public Guid CharacterId { get; set; }
        public string Name { get; set; }
        public int DamageDealt { get; set; }
        public int Health { get; set; }
        public string AttackInfo { get; set; }
    }
}
