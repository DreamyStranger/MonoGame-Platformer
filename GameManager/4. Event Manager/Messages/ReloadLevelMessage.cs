namespace ECS_Framework
{
    /// <summary>
    /// Represents a message that indicates the current game level should be reloaded.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class ReloadLevelMessage : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the ReloadLevelMessage class.
        /// </summary>
        public ReloadLevelMessage()
        {
        }
    }
}
