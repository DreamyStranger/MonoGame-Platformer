using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ECS_Framework
{

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
        public SystemManager(LevelID levelID)
        {
            systems = new List<System>();
            systems.Add(new ParallaxSystem());
            systems.Add(new PlayerInputSystem());
            systems.Add(new MovementSystem());
            systems.Add(new ObstacleCollisionSystem(levelID));
            systems.Add(new PlayerEntityCollisionSystem());
            systems.Add(new RenderSystem());
            systems.Add(new DeathAnimationSystem());
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
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public void Remove(Entity entity)
        {
            foreach (System system in systems)
            {
                system.RemoveEntity(entity);
            }
        }


        /// <summary>
        /// Updates all the systems with the specified <see cref="GameTime"/>.
        /// </summary>
        /// <param name="gameTime">The time since the last update.</param>
        public void Update(GameTime gameTime)
        {
            foreach (System system in systems)
            {
                system.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all the entities managed by the systems with the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw the entities.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (System system in systems)
            {
                system.Draw(spriteBatch);
            }
        }
    }
}
