using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ECS_Framework
{
    /// <summary>
    /// <see cref="System"/> responsible for handling the movement of entities in the game.
    /// </summary>
    public class MovementSystem : System
    {
        // A list of EntityData instances, which store references to the associated Entity, StateComponent, and MovementComponent.
        private List<EntityData> _entitiesData;

        /// <summary>
        /// Initializes a new instance of the MovementSystem class and creates an empty list of EntityData.
        /// </summary>
        public MovementSystem()
        {
            _entitiesData = new List<EntityData>();
        }

        /// <summary>
        /// Adds an entity to the movement system.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || movement == null)
            {
                return;
            }

            EntityData data = new EntityData
            {
                Entity = entity,
                State = state,
                Movement = movement,
            };

            _entitiesData.Add(data);
        }

        /// <summary>
        /// Removes an entity from the movement system.
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
        /// Updates the movement system based on the elapsed game time.
        /// Note: Horizontal and Vertical borders are not checked
        /// Note: Above because borders are drown in Tiled and handled in ObstacleCollisionSystem
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                UpdatePositionBasedOnState(gameTime, data.Movement, data.State);
                //update collision box
                CollisionBoxComponent collisionBox = data.Entity.GetComponent<CollisionBoxComponent>();
                if (collisionBox != null)
                {
                    collisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
                }
            }
        }

        /// <summary>
        /// Updates the position of an entity based on its state, movement, and elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        /// <param name="movement">The movement component of the entity.</param>
        /// <param name="state">The state component of the entity.</param>
        private void UpdatePositionBasedOnState(GameTime gameTime, MovementComponent movement, StateComponent state)
        {
            // Motion variables
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            VerticalMovement(state, movement);
            HorizontalMovement(state, movement);

            // Update Velocity
            movement.Velocity += movement.Acceleration * deltaTime;

            // Clamp Velocity
            if (state.CurrentSuperState != SuperState.IsOnGround)
            {
                movement.Velocity = new Vector2(MathHelper.Clamp(movement.Velocity.X, -GameConstants.SpeedX, GameConstants.SpeedX), movement.Velocity.Y);
            }

            // Update Position
            movement.LastPosition = movement.Position;
            movement.Position += movement.Velocity * deltaTime;
        }

        /// <summary>
        /// Updates the vertical velocity of an entity based on its state.
        /// </summary>
        /// <param name="state">The state component of the entity.</param>
        /// <param name="movement">The movement component of the entity.</param>
        private void VerticalMovement(StateComponent state, MovementComponent movement)
        {
            // Vertical Movement
            switch (state.CurrentSuperState)
            {
                case SuperState.IsOnGround:
                    movement.Acceleration = Vector2.Zero;
                    movement.Velocity = Vector2.Zero;
                    if (state.CurrentState == State.Jump)
                    {
                        movement.Velocity = new Vector2(movement.Velocity.X, GameConstants.SpeedY);
                        state.CurrentSuperState = SuperState.IsJumping;
                    }
                    break;

                case SuperState.IsFalling:
                    movement.Acceleration = new Vector2(0, GameConstants.GRAVITY);
                    if (state.CurrentState == State.DoubleJump)
                    {
                        movement.Velocity += new Vector2(0, GameConstants.SpeedY);
                        state.CurrentSuperState = SuperState.IsDoubleJumping;
                    }
                    break;

                case SuperState.IsDead:
                    movement.Velocity = Vector2.Zero;
                    movement.Acceleration = Vector2.Zero;
                    break;

                default:
                    movement.Acceleration = new Vector2(0, GameConstants.GRAVITY);
                    if (movement.Velocity.Y > 0)
                    {
                        state.CurrentSuperState = SuperState.IsFalling;
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates the horizontal velocity of an entity based on its state.
        /// </summary>
        /// <param name="state">The state component of the entity.</param>
        /// <param name="movement">The movement component of the entity.</param>
        private void HorizontalMovement(StateComponent state, MovementComponent movement)
        {
            switch (state.CurrentState)
            {
                case State.WalkLeft:
                    state.HorizontalDirection = -1;
                    movement.Velocity += new Vector2(-GameConstants.SpeedX, 0);
                    break;

                case State.WalkRight:
                    state.HorizontalDirection = 1;
                    movement.Velocity += new Vector2(GameConstants.SpeedX, 0);
                    break;
            }
        }
    }
}

