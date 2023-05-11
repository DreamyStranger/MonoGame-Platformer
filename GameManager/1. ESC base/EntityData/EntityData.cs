using Microsoft.Xna.Framework;
namespace EC_Framework
{
    /// <summary>
    /// A struct that contains the most commonly used components.
    /// </summary>
    struct EntityData
    {
        public Entity Entity;
        public MovementComponent Movement;
        public StateComponent State;
        public CollisionBoxComponent CollisionBox;
        public AnimatedComponent Animations;
    }
}