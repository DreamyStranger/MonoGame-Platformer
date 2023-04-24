namespace ECS_Framework
{
    /// <summary>
    /// Represents a message that indicates an entity should be destroyed.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class DestroyEntityMessage : IMessage
    {
        /// <summary>
        /// Gets the entity that should be destroyed.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Initializes a new instance of the DestroyEntityMessage class.
        /// </summary>
        /// <param name="entity">The entity that should be destroyed.</param>
        public DestroyEntityMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}
