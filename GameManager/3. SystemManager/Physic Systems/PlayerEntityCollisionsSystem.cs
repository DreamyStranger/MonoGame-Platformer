using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Framework
{
    public class PlayerEntityCollisionSystem : System
    {
        private List<EntityData> _entitiesData;
        public PlayerEntityCollisionSystem()
        {
            _entitiesData = new List<EntityData>();
        }
        /// <summary>
        /// Adds an entity to the system.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            CollisionBoxComponent collisionBox = entity.GetComponent<CollisionBoxComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || collisionBox == null || movement == null)
            {
                return;
            }

            _entitiesData.Add(new EntityData
            {
                Entity = entity,
                State = state,
                CollisionBox = collisionBox,
                Movement = movement,
            });
        }

        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            _entitiesData.RemoveAll(data => data.Entity == entity);
        }

        /// <summary>
        /// Handles Collisions between player and other entities.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (_entitiesData.Count <= 1)
            {
                return;
            }

            //Player always must be first in entity data for this system!
            EntityData player = _entitiesData[0];

            // Iterate through all entities after the player
            for (int i = 1; i < _entitiesData.Count; i++)
            {
                EntityData gameObject = _entitiesData[i];
                // Check if the two entities are colliding
                if (player.CollisionBox.GetRectangle().Intersects(gameObject.CollisionBox.GetRectangle()))
                {
                    // Check the entity types to determine the appropriate collision resolution
                    EntityTypeComponent entityType = gameObject.Entity.GetComponent<EntityTypeComponent>();
                    switch (entityType.Type)
                    {
                        case EntityType.Coin:
                            ResolveCoinCollision(player, gameObject);
                            break;
                        case EntityType.WalkingEnemy:
                            ResolveWalkingEnemyCollision(player, gameObject);
                            break;
                        case EntityType.PortalToNextLevel:
                            ResolveNextLevelCollision(player, gameObject);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ResolveCoinCollision(EntityData player, EntityData coin)
        {
            coin.State.CurrentSuperState = SuperState.IsDead;
        }

        private void ResolveNextLevelCollision(EntityData player, EntityData portal)
        {
            //Implement
        }

        private void ResolveWalkingEnemyCollision(EntityData player, EntityData enemy)
        {
            //Implement
        }
    }
}