using System.Collections.Generic;

namespace MyGame
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
            objects.Add(EntityFactory.CreateParallaxBackground("bg_green", new Vector2(0, 50)));
            objects.Add(EntityFactory.CreatePlayer(new Vector2(320, 180)));
            return objects;
        }
    }
}