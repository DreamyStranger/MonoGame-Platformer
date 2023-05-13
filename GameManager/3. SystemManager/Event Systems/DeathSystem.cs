using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// System that manages entity death events, triggering actions depending on the entity type.
    /// </summary>
    public class DeathSystem : System
    {
        private List<Entity> _entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathSystem"/> class.
        /// </summary>
        public DeathSystem()
        {
            _entities = new List<Entity>();
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
        /// Subscribes to appropriate messages
        /// </summary>
        public override void Subscribe()
        {
             MessageBus.Subscribe<EntityDiedMessage>(EntityDied);
        }

        /// <summary>
        /// Unsubscribes from all of the messages
        /// </summary>
        public override void Unsubscribe()
        {
            MessageBus.Unsubscribe<EntityDiedMessage>(EntityDied);
        }

        /// <summary>
        /// Updates the system, checking for entities in a dead state and triggering the appropriate action.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                Entity entity = _entities[i];
                if(!entity.IsActive)
                {
                    continue;
                }
                StateComponent stateComponent = entity.GetComponent<StateComponent>();
                AnimatedComponent animatedComponent = entity.GetComponent<AnimatedComponent>();
                RespawnComponent canBeRespawned = entity.GetComponent<RespawnComponent>();

                if (stateComponent.CurrentSuperState == SuperState.IsDead)
                {
                    ActionAnimation deathAnimation = animatedComponent.GetCurrentAnimation();

                    // Check if the animation has completed
                    if (deathAnimation.IsFinished)
                    {
                        if (entity.GetComponent<EntityTypeComponent>().Type == EntityType.Player)
                        {
                            MessageBus.Publish(new DestroyEntityMessage(entity));
                            MessageBus.Publish(new ReloadLevelMessage());
                        }
                        else if(canBeRespawned != null)
                        {
                            entity.IsActive = false;
                            deathAnimation.Reset();
                            canBeRespawned.StartRespawn();
                        }
                        else
                        {
                            // Remove the entity
                            MessageBus.Publish(new DestroyEntityMessage(entity));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds entities to the system
        /// </summary>
        /// <param name="message">The message containing the entity to add.</param>
        private void EntityDied(EntityDiedMessage message)
        {
            _entities.Add(message.Entity);
        }
    }
}