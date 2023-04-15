using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// The MovementSystem class is responsible for handling the movement of entities in the game.
    /// </summary>
    public class MovementSystem : System
    {
        // A list of EntityData instances, which store references to the associated Entity, StateComponent, and MovementComponent.
        List<EntityData> entitiesData;

        /// <summary>
        /// Initializes a new instance of the MovementSystem class and creates an empty list of EntityData.
        /// </summary>
        public MovementSystem()
        {
            entitiesData = new List<EntityData>();
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

            entitiesData.Add(data);
        }

        /// <summary>
        /// Removes an entity from the movement system.
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
        /// Updates the movement system based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in entitiesData)
            {
                UpdatePositionBasedOnState(gameTime, data.Movement, data.State);
            }
        }

        /// <summary>
        /// Updates the position of an entity based on its state, movement, and elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        /// <param name="movement">The movement component of the entity.</param>
        /// <param name="state">The state component of the entity.</param>
        public void UpdatePositionBasedOnState(GameTime gameTime, MovementComponent movement, StateComponent state)
        {
            // Motion variables
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            VerticalMovement(state, movement);
            HorizontalMovement(state, movement);

            // Update Velocity
            movement.Velocity += movement.Acceleration * deltaTime;

            // Clamp Velocity
            if (!state.IsSuperState(SuperState.OnGround))
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
        public void VerticalMovement(StateComponent state, MovementComponent movement)
        {
            // Vertical Movement
            switch (state.currentSuperState)
            {
                case SuperState.OnGround:
                    movement.Acceleration = Vector2.Zero;
                    movement.Velocity = Vector2.Zero;
                    if (state.IsState(ObjectState.Jump))
                    {
                        movement.Velocity = new Vector2(movement.Velocity.X, GameConstants.SpeedY);
                        state.SetSuperState(SuperState.isJumping);
                    }
                    break;

                case SuperState.isFalling:
                    movement.Acceleration = new Vector2(0, GameConstants.GRAVITY);
                    if (state.IsState(ObjectState.DoubleJump))
                    {
                        movement.Velocity += new Vector2(0, GameConstants.SpeedY);
                        state.SetSuperState(SuperState.isDoubleJumping);
                    }
                    break;
                default:
                    movement.Acceleration = new Vector2(0, GameConstants.GRAVITY);
                    if (movement.Velocity.Y > 0) state.SetSuperState(SuperState.isFalling);
                    break;
            }
        }

        /// <summary>
        /// Updates the horizontal velocity of an entity based on its state.
        /// </summary>
        /// <param name="state">The state component of the entity.</param>
        /// <param name="movement">The movement component of the entity.</param>
        public void HorizontalMovement(StateComponent state, MovementComponent movement)
        {
            switch (state.currentState)
            {
                case ObjectState.WalkLeft:
                    movement.HorizontalDirection = -1;
                    if (movement.CanMoveLeft)
                    {
                        movement.Velocity += new Vector2(-GameConstants.SpeedX, 0);
                    }
                    break;

                case ObjectState.WalkRight:
                    movement.HorizontalDirection = 1;
                    if (movement.CanMoveRight)
                    {
                        movement.Velocity += new Vector2(GameConstants.SpeedX, 0);
                    }
                    break;
            }
        }
    }
}

