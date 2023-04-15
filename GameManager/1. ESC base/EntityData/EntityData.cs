using Microsoft.Xna.Framework;
namespace MyGame
{
    struct EntityData
    {
        public Entity Entity;
        public MovementComponent Movement;
        public StateComponent State;
        public CollisionBoxComponent CollisionBox;
        public AnimatedComponent Animations;
    }
}