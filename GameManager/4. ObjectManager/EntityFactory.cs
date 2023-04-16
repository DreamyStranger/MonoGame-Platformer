
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MyGame{
    struct EntityFactory
    {
    //Parallax Background
        public static Entity CreateParallaxBackground(string texture, Vector2 velocity)
        {
            Entity background = new Entity();
            
            //Define the desired area of parallax
            int viewX = GameConstants.SCREEN_WIDTH;
            int viewY =  GameConstants.SCREEN_HEIGHT;
            
            //Parallax
            background.AddComponent(new ParallaxComponent(texture, velocity, Vector2.Zero, viewX, viewY));
            return background;
        }

    //Player
        public static Entity CreatePlayer(Vector2 position)
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
            var Transform = new MovementComponent(position);
            player.AddComponent(Transform);

        // Collisions
            player.AddComponent(new CollisionBoxComponent(position, 32, 32, 8, 0, 4 ,6));

            return player;
        }
    }
}
