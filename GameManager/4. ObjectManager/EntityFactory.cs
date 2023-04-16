
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MyGame{
    struct EntityFactory
    {
    //Parallax Background
        public static Entity CreateParallaxBackground()
        {
            Entity background = new Entity();
            
            //Define the desired area of parallax
            int viewX = GameConstants.SCREEN_WIDTH;
            int viewY =  GameConstants.SCREEN_HEIGHT;
            
            //Define the direction and speed of Parallax
            Vector2 velocity = new Vector2(0, 50); // top-down, change it and see what happens c:
            //Parallax
            background.AddComponent(new ParallaxComponent("background_green", velocity, Vector2.Zero, viewX, viewY));
            return background;
        }

    //Player
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
