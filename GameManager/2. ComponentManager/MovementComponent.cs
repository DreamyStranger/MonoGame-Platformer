using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// The MovementComponent class represents the component responsible for handling the motion of an entity in the game.
    /// </summary>
    /// <remarks>
    /// This component contains properties for the position, last position, velocity, acceleration, and flags related to horizontal movement.
    /// </remarks>
    public class MovementComponent : Component
    {
        //Position
        private Vector2 _position;
        private Vector2 _lastPosition;

        //Motion
        private Vector2 _velocity;
        private Vector2 _acceleration;

        // Flags
        private int _horizontalDirection = 1;
        private bool _canMoveLeft;
        private bool _canMoveRight;

        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector2 Position { get => _position; set => _position = value; }

        /// <summary>
        /// Gets or sets the last position of the entity.
        /// </summary>
        public Vector2 LastPosition { get => _lastPosition; set => _lastPosition = value; }

        /// <summary>
        /// Gets or sets the velocity of the entity.
        /// </summary>
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        /// <summary>
        /// Gets or sets the acceleration of the entity.
        /// </summary>
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }

        /// <summary>
        /// Gets or sets the horizontal direction of the entity.
        /// </summary>
        public int HorizontalDirection { get => _horizontalDirection; set => _horizontalDirection = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity can move to the left.
        /// </summary>
        public bool CanMoveLeft { get => _canMoveLeft; set => _canMoveLeft = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity can move to the right.
        /// </summary>
        public bool CanMoveRight { get => _canMoveRight; set => _canMoveRight = value; }

        /// <summary>
        /// Initializes a new instance of the MovementComponent class with the specified initial position.
        /// </summary>
        /// <param name="initialPosition">The initial position of the entity.</param>
        public MovementComponent(Vector2 initialPosition)
        {
            this._position = initialPosition;
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
            _canMoveRight = true;
            _canMoveLeft = true;
        }
    }
}
