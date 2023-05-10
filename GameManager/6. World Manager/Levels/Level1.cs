using System.Collections.Generic;
using TiledCS;

namespace ECS_Framework
{
    /// <summary>
    /// Implements the <see cref="LevelInitializer"/> interface to provide initialization logic for Level 1.
    /// </summary>
    public class LevelFromTiledInitializer : LevelInitializer
    {
        /// <summary>
        /// Returns a list of entities to be created and added to the level.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public List<Entity> GetObjects(LevelID level)
        {
            List<Entity> objects = new List<Entity>();
            objects = LevelLoader.GetObjects(level);
            return objects;
        }
    }
}