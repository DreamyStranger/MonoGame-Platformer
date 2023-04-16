using System.Collections.Generic;

namespace MyGame
{
    public enum LevelID
    {
        Level1,
        // Add more level IDs here
    }

    public class Level
    {
        public LevelID Id { get; }
        public LevelInitializer Initializer { get; }
     
        public Level(LevelID id, LevelInitializer initializer)
        {
            Id = id;
            Initializer = initializer;
        }
    }

    public interface LevelInitializer
    {
        List<Entity> GetObjects();
    }
}
