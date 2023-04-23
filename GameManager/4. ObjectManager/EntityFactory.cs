using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ECS_Framework
{
    /// <summary>
    /// A factory class for creating entities.
    /// </summary>
    struct EntityFactory
    {
        //Parallax Background
        /// <summary>
        /// Creates a parallax background entity.
        /// </summary>
        /// <param name="texture">The texture to use for the background.</param>
        /// <param name="velocity">The velocity of the background.</param>
        /// <returns>The parallax background entity.</returns>
        public static Entity CreateParallaxBackground(string texture, Vector2 velocity)
        {
            Entity background = new Entity();
            
            //Define the desired area of parallax
            int viewX = GameConstants.SCREEN_WIDTH;
            int viewY = GameConstants.SCREEN_HEIGHT;
            
            //Parallax
            background.AddComponent(new ParallaxComponent(texture, velocity, Vector2.Zero, viewX, viewY));
            return background;
        }

        //Player
        /// <summary>
        /// Creates a player entity.
        /// </summary>
        /// <param name="position">The initial position of the player.</param>
        /// <returns>The player entity.</returns>
        public static Entity CreatePlayer(Vector2 position)
        {
            //Empty Player
            Entity player = new Entity();

            // Animations
            AnimatedComponent animation =  new AnimatedComponent();
            animation.AddAnimation("player_idle", "idle", 1, 11, 20);
            animation.AddAnimation("player_walking", "walking", 1, 12, 20);
            animation.AddAnimation("player_jump", "jump", 1, 1, 20);
            animation.AddAnimation("player_double_jump", "double_jump",  1, 6, 20);
            animation.AddAnimation("player_fall", "fall", 1, 1, 20);
            animation.AddAnimation("player_slide", "slide", 1, 5, 20);
            player.AddComponent(animation);

            // States
            player.AddComponent(new StateComponent());
            player.AddComponent(new InputComponent());

            // Position and transforms
            var Transform = new MovementComponent(position);
            player.AddComponent(Transform);

            // Collisions
            player.AddComponent(new CollisionBoxComponent(
                    position: position, 
                    width: 32, 
                    height: 32, 
                    vertTopOffset: 8,
                    vertBottomOffset: 0, 
                    horLeftOffset: 4,
                    horRightOffset: 6));

            return player;
        }
    }
}
