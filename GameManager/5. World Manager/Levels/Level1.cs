using System.Collections.Generic;

namespace MyGame
{
    public class Level1Initializer : LevelInitializer
    {
        public List<Entity> GetObjects()
        {
            List<Entity> objects = new List<Entity>();
            objects.Add(EntityFactory.CreateParallaxBackground());
            objects.Add(EntityFactory.CreatePlayer());
            return objects;
        }
    }
}