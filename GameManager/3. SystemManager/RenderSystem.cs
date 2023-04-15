using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MyGame
{
    /// <summary>
    /// The RenderSystem class is responsible for rendering the entities in the game.
    /// </summary>
    public class RenderSystem : System
    {
        // A list of EntityData instances, which store references to the associated Entity, StateComponent, AnimatedComponent, and MovementComponent.
        private List<EntityData> entitiesData;

        /// <summary>
        /// Initializes a new instance of the RenderSystem class and creates an empty list of EntityData.
        /// </summary>
        public RenderSystem()
        {
            entitiesData = new List<EntityData>();
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
                Console.WriteLine("Missing a Component!"); //Debug message
                return;
            }

            EntityData data = new EntityData
            {
                Entity = entity,
                State = state,
                Animations = animations,
                Movement = movement,
            };

            entitiesData.Add(data);
        }

        /// <summary>
        /// Removes an entity from the render system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            var index = entitiesData.FindIndex(data => data.Entity == entity);
            if (index != -1)
            {
                entitiesData.RemoveAt(index);
            }
        }

        /// <summary>
        /// Updates the render system based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in entitiesData)
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
            foreach (var data in entitiesData)
            {
                data.Animations.Draw(spriteBatch, data.Movement.Position, data.Movement.HorizontalDirection);
            }
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
