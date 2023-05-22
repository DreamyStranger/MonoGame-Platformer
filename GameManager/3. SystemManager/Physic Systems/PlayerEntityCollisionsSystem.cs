using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> that handles collisions between the player entity and other entities.
    /// </summary>
    public class PlayerEntityCollisionSystem : System
    {
        private List<EntityData> _entitiesData;
        private EntityData _playerData;

        /// <summary>
        /// Creates an instance of <see cref="PlayerEntityCollisionSystem"/> class.
        /// </summary>
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

            if (entity.GetComponent<EntityTypeComponent>().Type == EntityType.Player)
            {
                _playerData = data;
            }
            else
            {
                _entitiesData.Add(data);
            }
        }

        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            if (entity.GetComponent<EntityTypeComponent>().Type == EntityType.Player)
            {
                _playerData = new EntityData();
            }
            else
            {
                _entitiesData.RemoveAll(data => data.Entity == entity);
            }
        }

        /// <summary>
        /// Handles Collisions between player and other entities.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                if (_playerData.State.CurrentSuperState == SuperState.IsDead || _playerData.State.CurrentSuperState == SuperState.IsAppearing || !_playerData.Entity.IsActive)
                {
                    return;
                }
                if (!data.Entity.IsActive || data.State.CurrentSuperState == SuperState.IsAppearing || data.State.CurrentSuperState == SuperState.IsDead)
                {
                    continue;
                }

                //Update Collision Boxes, should have done at any system manipulating movements before
                /*
                _playerData.CollisionBox.UpdateBoxPosition(_playerData.Movement.Position.X, _playerData.Movement.Position.Y, _playerData.State.HorizontalDirection);
                data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
                */

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
                            ResolveRegularEnemyCollision(_playerData, data);
                            break;
                        case EntityType.PortalToNextLevel:
                            ResolveNextLevelCollision(_playerData, data);
                            break;
                        //Add more cases for new entity types as needed
                        default:
                            break;
                    }

                    //Update Collision Boxes
                    _playerData.CollisionBox.UpdateBoxPosition(_playerData.Movement.Position.X, _playerData.Movement.Position.Y, _playerData.State.HorizontalDirection);
                    data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
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
            // Mark the coin as dead
            coinData.State.CurrentSuperState = SuperState.IsDead;
            MessageBus.Publish(new EntityDiedMessage(coinData.Entity));
        }

        /// <summary>
        /// Resolves collisions between the player and a portal object that leads to the next level.
        /// </summary>
        /// <param name="playerData">The data for the player entity.</param>
        /// <param name="portalData">The data for the portal entity.</param>
        private void ResolveNextLevelCollision(EntityData playerData, EntityData portalData)
        {
            // Publish a message indicating that the player has reached the next level
            MessageBus.Publish(new NextLevelMessage());
        }

        /// <summary>
        /// Resolves collisions between the player and a regular enemy that is walking on the ground.
        /// </summary>
        /// <param name="player">The data for the player entity.</param>
        /// <param name="enemy">The data for the enemy entity.</param>
        private void ResolveRegularEnemyCollision(EntityData player, EntityData enemy)
        {

            // If the player is falling, kill the enemy and set its state to idle
            switch (player.State.CurrentSuperState)
            {
                case SuperState.IsFalling:
                    int direction = player.State.HorizontalDirection;
                    float positionX = player.Movement.Position.X;
                    float positionY = player.Movement.Position.Y;

                    bool properHit = HandleFallCollision(player, player.CollisionBox.GetRectangle(), enemy.CollisionBox.GetRectangle(), ref positionX, ref positionY);
                    if (!properHit)
                    {
                        break;
                    }

                    enemy.Movement.Velocity = new Vector2(0, -20);
                    enemy.Movement.Acceleration = new Vector2(0, 100);
                    enemy.State.CurrentSuperState = SuperState.IsDead;
                    enemy.State.CurrentState = State.Idle;

                    player.Movement.Position = new Vector2(positionX, positionY);
                    player.Movement.Velocity = new Vector2(GameConstants.SpeedXonCollision * direction, player.Movement.Velocity.Y - GameConstants.SpeedYonCollision);
                    player.State.CurrentSuperState = SuperState.IsJumping;
                    player.State.CurrentState = State.Idle;
                    player.State.JumpsPerformed = 1;

                    MessageBus.Publish(new EntityDiedMessage(enemy.Entity));
                    break;

                default:
                    player.State.CurrentSuperState = SuperState.IsDead;
                    player.State.CurrentState = State.Idle;
                    player.Movement.Velocity = new Vector2(0, -200);
                    player.Movement.Acceleration = new Vector2(0, 1000);

                    MessageBus.Publish(new EntityDiedMessage(player.Entity));
                    break;
            }
        }

        //Helper Methods

        /// <summary>
        /// Handles collision when the entity is in a falling state.
        /// </summary>
        /// <param name="data">The EntityData containing the entity's components.</param>
        /// <param name="box">The entity's collision box.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">The entity's current X position.</param>
        /// <param name="positionY">The entity's current Y position.</param>
        /// <returns>Returns true if the entity collided with the top side of the obstacle.</returns>
        private bool HandleFallCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY)
        {
            bool wasAbove = data.Movement.LastPosition.Y + data.CollisionBox.OriginalHeight - data.CollisionBox.VertBottomOffset <= rect.Top + 1;
            bool collidesWithTopSide = box.Bottom > rect.Top && box.Top <= rect.Top;


            if (collidesWithTopSide && wasAbove)
            {
                positionY = rect.Top - data.CollisionBox.OriginalHeight + data.CollisionBox.VertBottomOffset;
                data.Movement.Velocity = Vector2.Zero;
                data.Movement.Acceleration = Vector2.Zero;
                return true;
            }
            return false;
        }

    }
}