using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MyGame {

    /// <summary>
/// Manages a collection of systems and provides methods to add, remove, update and draw entities through them.
/// </summary>
    public struct SystemManager
    {
        private List<System> systems = new List<System>();

/// <summary>
/// Initializes a new instance of the <see cref="SystemManager"/> class with a specified level ID.
/// </summary>
/// <param name="LevelID">The ID of the level.</param>
        public SystemManager(string LevelID)
        {
            systems = new List<System>();
            systems.Add(new InputSystem());
            systems.Add(new MovementSystem());
            systems.Add(new ObstacleColliderSystem(LevelID));
            systems.Add(new RenderSystem());
        }

/// <summary>
/// Adds an entity to all the systems.
/// </summary>
/// <param name="entity">The entity to be added.</param>
        public void Add(Entity entity)
        {
            foreach (System system in systems)
            {
                system.AddEntity(entity);
            }
        }
/// <summary>
/// Removes an entity from specified systems.
/// </summary>
/// <param name="entity">The entity to be removed.</param>
        public void Remove(Entity entity, int systemIndex)
        {
            systems[systemIndex].RemoveEntity(entity);
        }
        public void Update(GameTime gameTime)
        {
            foreach (System system in systems)
            {
                system.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (System system in systems)
            {
                system.Draw(spriteBatch);
            }
        }
    }
}
