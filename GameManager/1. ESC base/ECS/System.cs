using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// An abstract base class for systems in the game.
    /// </summary>
    public abstract class System
    {
        /// <summary>
        /// Adds an entity to the system.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public abstract void AddEntity(Entity entity);

        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public abstract void RemoveEntity(Entity entity);


        /// <summary>
        /// Updates the system.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the system.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
