using System.Collections.Generic;

namespace ECS_Framework
{
    /// <summary>
    /// Represents the IDs for all levels in the game.
    /// </summary>
    public enum LevelID
    {
        Level1,
        Level2,
        // Add more level IDs here
    }

    /// <summary>
    /// Represents a level in the game.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// The ID of the level.
        /// </summary>
        public LevelID Id { get; }
        /// <summary>
        /// The initializer for the level, which provides the objects and their positions.
        /// </summary>
        public LevelInitializer Initializer { get; }

        /// <summary>
        /// Initializes a new instance of the Level class.
        /// </summary>
        /// <param name="id">The ID of the level.</param>
        /// <param name="initializer">The initializer for the level.</param>
        public Level(LevelID id, LevelInitializer initializer)
        {
            Id = id;
            Initializer = initializer;
        }
    }

    /// <summary>
    /// Provides an interface for level initializers, which can provide the objects for a level.
    /// </summary>
    public interface LevelInitializer
    {
        /// <summary>
        /// Gets a list of entities that will be placed in the level.
        /// </summary>
        /// <returns>A list of entities to be placed in the level.</returns>
        List<Entity> GetObjects();
    }
}
