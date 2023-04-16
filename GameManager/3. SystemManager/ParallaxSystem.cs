using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// A system that manages parallax components and their related entities.
    /// </summary>
    public class ParallaxSystem : System
    {
        private List<Entity> entities;
        private List<ParallaxComponent> parallaxes;

        /// <summary>
        /// Initializes a new instance of the ParallaxSystem class.
        /// </summary>
        public ParallaxSystem()
        {
            entities = new List<Entity>();
            parallaxes = new List<ParallaxComponent>();
        }

        /// <summary>
        /// Adds an entity with a ParallaxComponent to the system.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public override void AddEntity(Entity entity)
        {
            ParallaxComponent parallax = entity.GetComponent<ParallaxComponent>();
            if (parallax == null)
            {
                return;
            }

            entities.Add(entity);
            parallaxes.Add(parallax);
        }

        /// <summary>
        /// Removes an entity and its associated ParallaxComponent from the system.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public override void RemoveEntity(Entity entity)
        {
            int index = entities.IndexOf(entity);
            if (index != -1)
            {
                entities.RemoveAt(index);
                parallaxes.RemoveAt(index);
            }
        }
        
        /// <summary>
        /// Updates the ParallaxComponents in the system based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < parallaxes.Count; i++)
            {
                parallaxes[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the ParallaxComponents using the provided SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < parallaxes.Count; i++)
            {
                parallaxes[i].Draw(spriteBatch);
            }
        }
    }
}