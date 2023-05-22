using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> responsible for rendering the entities in the game.
    /// </summary>
    public class LevelRenderSystem : System
    {
        // A list of EntityData instances, which store references to the associated Entity, StateComponent, AnimatedComponent, and MovementComponent.
        private string _levelID;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelRenderSystem"/> class.
        /// </summary>
        public LevelRenderSystem(LevelID levelID)
        {
            _levelID = levelID.ToString();
        }

        /// <summary>
        /// Draws the current level.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Loader.tiledHandler.Draw(_levelID, spriteBatch);
        }
    }
}
