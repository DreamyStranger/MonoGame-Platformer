namespace EC_Framework
{
    /// <summary>
    /// Represents a message that indicates the game should proceed to the next level.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class NextLevelMessage : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the NextLevelMessage class.
        /// </summary>
        public NextLevelMessage()
        {
        }
    }
}
