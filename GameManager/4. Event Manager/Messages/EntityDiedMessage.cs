namespace MonogameExamples
{
    /// <summary>
    /// Messages that given Entity has died.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class EntityDiedMessage : IMessage
    {
        /// <summary>
        /// Gets the entity that died.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDiedMessage"/> class.
        /// </summary>
        /// <param name="entity">The entity that died.</param>
        public EntityDiedMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}
