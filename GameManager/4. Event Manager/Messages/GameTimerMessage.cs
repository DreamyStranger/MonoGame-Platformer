namespace MonogameExamples
{
    /// <summary>
    /// Indicates that Game Timer was created.
    /// Implements the IMessage interface for use with the <see cref ="MessageBus"/>.
    /// </summary>
    public class GameTimerMessage : IMessage
    {
        /// <summary>
        /// Entity that holds the timer
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Timer's Value, in seconds
        /// </summary>
        public float Timer { get; }

        /// <summary>
        /// Timer's Position
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTimerMessage"/> class.
        /// </summary>
        /// <param name="entity">entity that holds the timer.</param>
        public GameTimerMessage(Entity entity, float timer, Vector2 position)
        {
            Entity = entity;
            Timer = timer;
            Position = position;
        }
    }
}
