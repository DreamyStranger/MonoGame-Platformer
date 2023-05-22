namespace MonogameExamples
{
    /// <summary>
    /// Represents a message that indicates the game should exit.
    /// Implements the IMessage interface for use with the MessageBus.
    /// </summary>
    public class ExitGameMessage : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitGameMessage"/> class.
        /// </summary>
        public ExitGameMessage()
        {
        }
    }
}