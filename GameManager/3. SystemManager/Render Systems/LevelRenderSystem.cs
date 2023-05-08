using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace ECS_Framework
{
    /// <summary>
    /// <see cref="System"/> responsible for rendering the entities in the game.
    /// </summary>
    public class LevelRenderSystem : System
    {
        // A list of EntityData instances, which store references to the associated Entity, StateComponent, AnimatedComponent, and MovementComponent.
        private string _levelID;

        /// <summary>
        /// Initializes a new instance of the LevelRenderSystem class and creates an empty list of EntityData.
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

        /// <summary>
        /// Sets the current animation for an entity based on its current state.
        /// </summary>
        /// <param name="state">The state component of the entity.</param>
        /// <param name="animations">The animated component of the entity.</param>
        private void GetAnimationForState(StateComponent state, AnimatedComponent animations)
        {
            animations.SetCurrentAction(state.stateID);
        }
    }
}
