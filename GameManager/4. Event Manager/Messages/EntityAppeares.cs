namespace MonogameExamples
{
    /// <summary>
    /// Messages that given Entity has died.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class EntityReAppearsMessage : IMessage
    {
        /// <summary>
        /// Gets the entity that should re-appear.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityReAppearsMessage"/> class.
        /// </summary>
        /// <param name="entity">entity that should re-appear.</param>
        public EntityReAppearsMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}
