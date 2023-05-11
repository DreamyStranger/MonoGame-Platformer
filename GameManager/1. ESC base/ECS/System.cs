using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
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
        public virtual void AddEntity(Entity entity)
        {

        }

        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public virtual void RemoveEntity(Entity entity)
        {

        }

        /// <summary>
        /// Subscribes system to the MessageBus events.
        /// </summary>
        public virtual void Subscribe()
        {

        }

        /// <summary>
        /// Unsubscribes system from the MessageBus events.
        /// </summary>
        public virtual void Unsubscribe()
        {

        }

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
