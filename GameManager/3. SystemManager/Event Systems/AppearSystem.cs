using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// System that manages entity appearance events, triggering actions depending on the entity type.
    /// </summary>
    public class AppearSystem : System
    {
        private List<Entity> _entities;
        private HashSet<Entity> _destroy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppearSystem"/> class.
        /// </summary>
        public AppearSystem()
        {
            _entities = new List<Entity>();
            _destroy = new HashSet<Entity>();
        }

        /// <summary>
        /// Subscribes to appropriate messages
        /// </summary>
        public override void Subscribe()
        {
            MessageBus.Subscribe<EntityReAppearsMessage>(EntityAppeared);
        }

        /// <summary>
        /// Unsubscribes from all of the messages
        /// </summary>
        public override void Unsubscribe()
        {
            MessageBus.Unsubscribe<EntityReAppearsMessage>(EntityAppeared);
        }

        /// <summary>
        /// Adds an entity to the system if it has both a StateComponent and an AnimatedComponent.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            if (state == null || entity.GetComponent<AnimatedComponent>() == null)
            {
                return;
            }
            if (state.CurrentSuperState == SuperState.IsAppearing)
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Handles addition of re-appearing entity to the system.
        /// </summary>
        /// <param name="message">The message containing an entity to add.</param>
        public void EntityAppeared(EntityReAppearsMessage message)
        {
            AddEntity(message.Entity);
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
        /// Updates the system, checking for entities in an IsAppearing state, and triggering the appropriate action.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (_entities.Count == 0)
            {
                //Console.WriteLine("IsEmpty!");  //Making sure the list is empty after entities appeared
                return;
            }
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                Entity entity = _entities[i];
                if (!entity.IsActive)
                {
                    continue;
                }
                StateComponent stateComponent = entity.GetComponent<StateComponent>();
                AnimatedComponent animatedComponent = entity.GetComponent<AnimatedComponent>();

                // Check if the entity is in the IsAppearing state
                if (stateComponent.CurrentSuperState == SuperState.IsAppearing)
                {
                    ActionAnimation appearAnimation = animatedComponent.GetCurrentAnimation();

                    // Check if the animation has completed
                    if (appearAnimation.IsFinished)
                    {
                        _destroy.Add(entity);
                        stateComponent.HorizontalDirection = stateComponent.DefaultHorizontalDirection;
                        stateComponent.CurrentSuperState = stateComponent.DefaultSuperState;
                        stateComponent.CurrentState = stateComponent.DefaultState;
                        stateComponent.CanMoveLeft = true;
                        stateComponent.CanMoveRight = true;
                    }
                }
            }

            // Remove entities that have already appeared
            foreach (Entity entity in _destroy)
            {
                RemoveEntity(entity);
            }

            // Clear out the destroy HashSet
            _destroy.Clear();
        }
    }
}
