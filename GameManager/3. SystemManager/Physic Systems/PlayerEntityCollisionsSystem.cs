using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Framework
{
    /// <summary>
    /// Represents a system that handles collisions between the player entity and other entities.
    /// </summary>
    public class PlayerEntityCollisionSystem : System
    {
        private List<EntityData> _entitiesData;
        private EntityData playerData;
        public PlayerEntityCollisionSystem()
        {
            _entitiesData = new List<EntityData>();
            playerData = new EntityData();
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

            var data = new EntityData
            {
                Entity = entity,
                State = state,
                CollisionBox = collisionBox,
                Movement = movement,
            };

            _entitiesData.Add(data);
            if (entity.GetComponent<EntityTypeComponent>().Type == EntityType.Player)
            {
                playerData = data;
            }
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
            if (_entitiesData.Count <= 1 || playerData.Entity == null)
            {
                return;
            }

            // Iterate through all entities after the player
            foreach (EntityData data in _entitiesData)
            {
                if (data.Entity == playerData.Entity)
                {
                    continue;
                }
                // Check if the two entities are colliding
                if (playerData.CollisionBox.GetRectangle().Intersects(data.CollisionBox.GetRectangle()))
                {
                    // Check the entity types to determine the appropriate collision resolution
                    EntityTypeComponent entityType = data.Entity.GetComponent<EntityTypeComponent>();
                    switch (entityType.Type)
                    {
                        case EntityType.Coin:
                            ResolveCoinCollision(playerData, data);
                            break;
                        case EntityType.WalkingEnemy:
                            ResolveWalkingEnemyCollision(playerData, data);
                            break;
                        case EntityType.PortalToNextLevel:
                            ResolveNextLevelCollision(playerData, data);
                            break;
                        //Add more cases for new entity types as needed
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
            if (player.State.CurrentSuperState == SuperState.IsOnGround)
            {
                MessageBus.Publish(new NextLevelMessage());
            }
        }

        private void ResolveWalkingEnemyCollision(EntityData player, EntityData enemy)
        {
            switch (player.State.CurrentSuperState)
            {
                case SuperState.IsFalling:
                    enemy.State.CurrentSuperState = SuperState.IsDead;
                    break;
                default:
                    if (enemy.State.CurrentSuperState != SuperState.IsDead)
                    {
                        player.State.CurrentSuperState = SuperState.IsDead;
                    }
                    break;
            }
        }
    }
}