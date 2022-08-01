﻿namespace RPG_GAME.Application.DTO
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HeroDto Hero { get; set; }
        public int Level { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public Guid UserId { get; set; }
    }
}