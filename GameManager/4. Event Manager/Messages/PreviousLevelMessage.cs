namespace MonogameExamples
{
    /// <summary>
    /// Represents a message that indicates the game should revert to the previous level.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class PreviousLevelMessage : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousLevelMessage"/> class.
        /// </summary>
        public PreviousLevelMessage()
        {
        }
    }
}
