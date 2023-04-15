using System.Collections.Generic;
using System;

namespace MyGame {

// Player
    public struct Player
    {
        private Entity entity;

        public Player(Game1 game)
        {
            entity = EntityFactory.CreatePlayer();
            game.systems.Add(entity);
        }
    }
}
