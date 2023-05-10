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
        private EntityData _playerData;
        public PlayerEntityCollisionSystem()
        {
            _entitiesData = new List<EntityData>();
            _playerData = new EntityData();
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
                _playerData = data;
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
            if (_entitiesData.Count <= 1 || _playerData.Entity == null)
            {
                return;
            }

            foreach (EntityData data in _entitiesData)
            {
                if (data.Entity == _playerData.Entity)
                {
                    continue;
                }
                // Check if the two entities are colliding
                if (_playerData.CollisionBox.GetRectangle().Intersects(data.CollisionBox.GetRectangle()))
                {
                    // Check the entity types to determine the appropriate collision resolution
                    EntityTypeComponent entityType = data.Entity.GetComponent<EntityTypeComponent>();
                    switch (entityType.Type)
                    {
                        case EntityType.Coin:
                            ResolveCoinCollision(_playerData, data);
                            break;
                        case EntityType.RegularEnemy:
                            ResolveWalkingEnemyCollision(_playerData, data);
                            break;
                        case EntityType.PortalToNextLevel:
                            ResolveNextLevelCollision(_playerData, data);
                            break;
                        //Add more cases for new entity types as needed
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Resolves collisions between the player and a coin object.
        /// </summary>
        /// <param name="playerData">The data for the player entity.</param>
        /// <param name="coinData">The data for the coin entity.</param>
        private void ResolveCoinCollision(EntityData playerData, EntityData coinData)
        {
            // If the coin is already dead, do nothing
            if (coinData.State.CurrentSuperState == SuperState.IsDead)
            {
                return;
            }

            // Mark the coin as dead and set its state to idle
            coinData.State.CurrentSuperState = SuperState.IsDead;
            coinData.State.CurrentState = State.Idle;
        }

        /// <summary>
        /// Resolves collisions between the player and a portal object that leads to the next level.
        /// </summary>
        /// <param name="playerData">The data for the player entity.</param>
        /// <param name="portalData">The data for the portal entity.</param>
        private void ResolveNextLevelCollision(EntityData playerData, EntityData portalData)
        {
            // If the player is already dead, do nothing
            if (playerData.State.CurrentSuperState == SuperState.IsDead)
            {
                return;
            }

            // Publish a message indicating that the player has reached the next level
            MessageBus.Publish(new NextLevelMessage());
        }

        /// <summary>
        /// Resolves collisions between the player and a regular enemy that is walking on the ground.
        /// </summary>
        /// <param name="player">The data for the player entity.</param>
        /// <param name="enemy">The data for the enemy entity.</param>
        private void ResolveWalkingEnemyCollision(EntityData player, EntityData enemy)
        {
            // If the enemy is already dead, do nothing
            if (enemy.State.CurrentSuperState == SuperState.IsDead)
            {
                return;
            }

            // If the player is falling, kill the enemy and set its state to idle
            if (player.State.CurrentSuperState == SuperState.IsFalling)
            {
                enemy.State.CurrentSuperState = SuperState.IsDead;
                enemy.State.CurrentState = State.Idle;
            }
            // Otherwise, kill the player and set its state to idle
            else
            {
                player.State.CurrentSuperState = SuperState.IsDead;
                player.State.CurrentState = State.Idle;
            }
        }

    }
}