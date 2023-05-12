namespace MonogameExamples
{
    public class AddEntityMessage : IMessage
    {
        public Entity Entity { get; }

        public AddEntityMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}