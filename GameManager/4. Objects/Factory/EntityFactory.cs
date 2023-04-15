
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MyGame{
    struct EntityFactory
    {
        public static Entity CreatePlayer()
        {
        
        //Empty Player
            Entity player = new Entity();

        // Animations
            AnimatedComponent animation =  new AnimatedComponent();
            animation.AddAnimation("player_idle", "idle", 1, 11, 20);
            animation.AddAnimation("player_walking", "walking", 1, 12, 20);
            animation.AddAnimation("player_jump", "jump", 1, 1, GameConstants.AnimationFPS);
            animation.AddAnimation("player_double_jump", "double_jump",  1, 6, GameConstants.AnimationFPS);
            animation.AddAnimation("player_fall", "fall", 1, 1, GameConstants.AnimationFPS);
            animation.AddAnimation("player_slide", "slide", 1, 5, GameConstants.AnimationFPS);
            player.AddComponent(animation);

        // States
            player.AddComponent(new StateComponent());
            player.AddComponent(new InputComponent());

        // Position and transforms
            var Transform = new MovementComponent(new Vector2(320, 180));
            player.AddComponent(Transform);

        // Collisions
            player.AddComponent(new CollisionBoxComponent(new Vector2(320, 180), 32, 32));

            return player;
        }
    }
}
