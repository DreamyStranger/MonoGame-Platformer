namespace ECS_Framework
{
    public class DestroyEntityMessage : IMessage
    {
        public Entity Entity { get; }

        public DestroyEntityMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}