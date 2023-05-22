namespace MonogameExamples
{
    /// <summary>
    /// Represents a message for adding an entity.
    /// </summary>
    public class AddEntityMessage : IMessage
    {
        /// <summary>
        /// Gets the entity to be added.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntityMessage"/> class.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public AddEntityMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}
