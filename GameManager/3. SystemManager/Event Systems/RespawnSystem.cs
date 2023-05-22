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
        /// Initializes a new instance of the <see cref="RespawnSystem"/> class.
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

                if (!respawn.IsRespawning || entity.IsActive)
                {
                    continue;
                }

                respawn.Update(gameTime);

                if (respawn.IsRespawning)
                {
                    continue;
                }

                StateComponent state = entity.GetComponent<StateComponent>();
                MovementComponent movement = entity.GetComponent<MovementComponent>();
                CollisionBoxComponent collision = entity.GetComponent<CollisionBoxComponent>();

                // for any entity, state component should always exist
                state.CurrentSuperState = SuperState.IsAppearing;
                state.HorizontalDirection = state.DefaultHorizontalDirection;

                if (movement != null)
                {
                    movement.Position = respawn.Position;
                    if (collision != null)
                    {
                        collision.UpdateBoxPosition(respawn.Position.X, respawn.Position.Y, state.HorizontalDirection);
                    }
                }

                entity.IsActive = true;
                MessageBus.Publish(new EntityReAppearsMessage(entity));
            }
        }

    }
}
