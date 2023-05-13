using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// System that manages entity respawning based on the RespawnComponent.
    /// </summary>
    public class RespawnSystem : System
    {
        private List<Entity> _entities;

        /// <summary>
        /// Initializes a new instance of the RespawnSystem class.
        /// </summary>
        public RespawnSystem()
        {
            _entities = new List<Entity>();
        }

        /// <summary>
        /// Adds an entity to the system if it has a RespawnComponent.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            if (entity.GetComponent<RespawnComponent>() != null)
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Updates the system, checking for entities that need to be respawned.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                RespawnComponent respawn = entity.GetComponent<RespawnComponent>();

                if (respawn.IsRespawning && !entity.IsActive)
                {
                    respawn.Update(gameTime);

                    if (!respawn.IsRespawning)
                    {
                        StateComponent state = entity.GetComponent<StateComponent>();
                        AnimatedComponent animations = entity.GetComponent<AnimatedComponent>();
                        MovementComponent movement =  entity.GetComponent<MovementComponent>();

                        // Make the entity active again
                        state.CurrentSuperState = SuperState.IsAppearing;
                        state.CurrentState = State.Idle;
                        if(animations != null)
                        {
                            ActionAnimation appearAnimation = animations.GetCurrentAnimation();
                            appearAnimation.Reset();
                        }

                        entity.IsActive = true;
                        if(movement != null)
                        {
                            movement.Position = respawn.position;
                        }
                        MessageBus.Publish(new EntityReAppearsMessage(entity));
                    }
                }
            }
        }
    }
}
