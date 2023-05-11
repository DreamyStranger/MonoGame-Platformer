using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace ECS_Framework
{
    /// <summary>
    /// <see cref="System"/> responsible for rendering the entities in the game.
    /// </summary>
    public class AnimationRenderSystem : System
    {
        private List<EntityData> _entitiesData;

        /// <summary>
        /// Initializes a new instance of the RenderSystem class and creates an empty list of EntityData.
        /// </summary>
        public AnimationRenderSystem()
        {
            _entitiesData = new List<EntityData>();
        }

        /// <summary>
        /// Adds an entity to the render system.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            AnimatedComponent animations = entity.GetComponent<AnimatedComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || animations == null || movement == null)
            {
                return;
            }

            EntityData data = new EntityData
            {
                Entity = entity,
                State = state,
                Animations = animations,
                Movement = movement,
            };

            _entitiesData.Add(data);
        }

        /// <summary>
        /// Removes an entity from the render system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            var index = _entitiesData.FindIndex(data => data.Entity == entity);
            if (index != -1)
            {
                _entitiesData.RemoveAt(index);
            }
        }

        /// <summary>
        /// Updates the render system based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                // Get the appropriate animation for the current state
                GetAnimationForState(data.State, data.Animations);
                data.Animations.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the entities in the render system.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var data in _entitiesData)
            {
                data.Animations.Draw(spriteBatch, data.Movement.Position, data.State.HorizontalDirection);
            }
        }

        /// <summary>
        /// Sets the current animation for an entity based on its current state.
        /// </summary>
        /// <param name="state">The state component of the entity.</param>
        /// <param name="animations">The animated component of the entity.</param>
        private void GetAnimationForState(StateComponent state, AnimatedComponent animations)
        {
            animations.SetCurrentAction(state.AnimationID);
        }
    }
}
