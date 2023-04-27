using System.Collections.Generic;

namespace ECS_Framework
{
    /// <summary>
    /// Implements the <see cref="LevelInitializer"/> interface to provide initialization logic for Level 1.
    /// </summary>
    public class Level1Initializer : LevelInitializer
    {
        /// <summary>
        /// Returns a list of entities to be created and added to the level.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public List<Entity> GetObjects()
        {
            List<Entity> objects = new List<Entity>();
            objects.Add(EntityFactory.CreateParallaxBackground("bg_green", new Vector2(50, 50)));
            objects.Add(EntityFactory.CreateApple(new Vector2(100, 100)));
            objects.Add(EntityFactory.CreateApple(new Vector2(524, 100)));
            objects.Add(EntityFactory.CreateSimpleEnemy(new Vector2(160, 240), 110, 200));
            objects.Add(EntityFactory.CreatePlayer(new Vector2(320, 180)));
            return objects;
        }
    }
}