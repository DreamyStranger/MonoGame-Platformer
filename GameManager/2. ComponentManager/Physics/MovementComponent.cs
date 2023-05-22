using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> that contains data related to the motion of an entity in the game.
    /// </summary>
    /// <remarks>
    /// This component contains properties for the position, last position, velocity, acceleration, and flags related to horizontal movement.
    /// </remarks>
    public class MovementComponent : Component
    {
        //Position
        private Vector2 _position;

        //Motion
        private Vector2 _velocity;
        private Vector2 _acceleration;

        /// <summary>
        /// Previous position of the entity.
        /// </summary>
        public Vector2 LastPosition { get; private set;}

        /// <summary>
        /// Position of the entity.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set
            {
                LastPosition = _position;
                _position = value;
            }
        }

        /// <summary>
        /// Velocity of the entity.
        /// </summary>
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        /// <summary>
        /// Acceleration of the entity.
        /// </summary>
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }

        /// <summary>
        /// Initializes a new instance of the MovementComponent class with the specified initial position.
        /// </summary>
        /// <param name="initialPosition">The initial position of the entity.</param>
        public MovementComponent(Vector2 initialPosition)
        {
            _position = initialPosition;
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
        }
    }
}
