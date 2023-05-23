using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> that manages collision detection and resolution between entities and level obstacles.
    /// </summary>
    public class ObstacleCollisionSystem : System
    {
        private List<EntityData> _entitiesData;
        /// <summary>
        /// Dictionary containing the obstacle data for each layer in the game environment.
        /// </summary>
        private Dictionary<string, List<Rectangle>> _obstacles;

        /// <summary>
        /// Initializes a new instance of the <see cref= "ObstacleCollisionSystem"/> class.
        /// </summary>
        /// <param name="LevelID">The identifier for the level's obstacle data.</param>

        public ObstacleCollisionSystem(LevelID levelID)
        {
            _entitiesData = new List<EntityData>();
            _obstacles = new Dictionary<string, List<Rectangle>>();
            _obstacles = Loader.tiledHandler.objects[levelID.ToString()];

            //Console.WriteLine($"Loaded obstacles for Levels: {levelID}"); // Debug message
            //Console.WriteLine($"Loaded obstacle layers: {string.Join(", ", obstacles.Keys)}"); // Debug message
        }
        /// <summary>
        /// Adds an entity to the system.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            CollisionBoxComponent collisionBox = entity.GetComponent<CollisionBoxComponent>();

            if (state == null || movement == null || collisionBox == null)
            {
                return;
            }

            _entitiesData.Add(new EntityData
            {
                Entity = entity,
                State = state,
                Movement = movement,
                CollisionBox = collisionBox,
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
        /// Handles Collisions between an entity and all level obstacles.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                if (!data.Entity.IsActive || data.State.CurrentSuperState == SuperState.IsAppearing || data.State.CurrentSuperState == SuperState.IsDead)
                {
                    continue;
                }

                //Should not be needed if collision box was updated after any movements beforehand
                //data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);

                Rectangle box = data.CollisionBox.GetRectangle();
                float positionX = data.Movement.Position.X;
                float positionY = data.Movement.Position.Y;
                data.State.CanMoveRight = true;
                data.State.CanMoveLeft = true;

                foreach (string key in _obstacles.Keys)
                {
                    foreach (Rectangle rect in _obstacles[key])
                    {
                        if (!box.Intersects(rect))
                        {
                            continue;
                        }

                        switch (data.State.CurrentSuperState)
                        {
                            case SuperState.IsFalling:
                                HandleFallCollision(data, box, rect, ref positionX, ref positionY, key);
                                break;

                            case SuperState.IsOnGround:
                                HandleGroundCollision(data, box, rect, ref positionX);
                                break;

                            case SuperState.IsJumping:
                            case SuperState.IsDoubleJumping:
                                if (key == "float")
                                {
                                    break;
                                }

                                HandleJumpCollision(data, box, rect, ref positionX, ref positionY);
                                break;

                            default:
                                break;
                        }
                        //Console.WriteLine($"Obstacle: {rect.ToString()}, StateID: {data.State.stateID}"); //Debug Message
                    }
                }

                //Update entity's position and collisionBox
                data.Movement.Position = new Vector2(positionX, positionY);
                data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);

                //Check if entity is not on platform anymore
                if (data.CollisionBox.CheckIfInAir(data.Movement.Position.X, data.State.HorizontalDirection))
                {
                    if (data.State.CurrentSuperState == SuperState.IsOnGround)
                    {
                        data.State.CurrentSuperState = SuperState.IsFalling;
                    }
                }
                //Check if entity is not sliding anymore
                if (data.CollisionBox.CheckIfBelow(data.Movement.Position.Y))
                {
                    if (data.State.CurrentState == State.Slide)
                    {
                        data.State.CurrentState = State.Idle;
                    }
                }
            }
        }

        /// <summary>
        /// Handles collision when the entity is in a falling state.
        /// </summary>
        /// <param name="data">The EntityData containing the entity's components.</param>
        /// <param name="box">The entity's collision box.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">The entity's current X position.</param>
        /// <param name="positionY">The entity's current Y position.</param>
        private void HandleFallCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY, string key)
        {
            data.State.CurrentState = State.Idle;
            bool wasAbove = data.Movement.LastPosition.Y + data.CollisionBox.OriginalHeight - data.CollisionBox.VertBottomOffset <= rect.Top + 1;

            if (wasAbove)
            {
                data.State.CurrentSuperState = SuperState.IsOnGround;
                positionY = rect.Top - data.CollisionBox.OriginalHeight + data.CollisionBox.VertBottomOffset - 0.1f;
                data.CollisionBox.SetGroundLocation(rect.Left, rect.Right);
            }
            else if (key != "float")
            {
                HandleHorizontalInAirCollision(data, box, rect, ref positionX);
            }
        }

        /// <summary>
        /// Handles collision when the entity is in a jumping state.
        /// </summary>
        /// <param name="data">The EntityData containing the entity's components.</param>
        /// <param name="box">The entity's collision box.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">The entity's current X position.</param>
        /// <param name="positionY">The entity's current Y position.</param>
        /// <param name="key">The key indicating the current layer being checked.</param>
        private void HandleJumpCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY)
        {
            bool wasBelow = data.Movement.LastPosition.Y + data.CollisionBox.VertTopOffset >= rect.Bottom - 1;
            if (wasBelow)
            {
                positionY = rect.Bottom - data.CollisionBox.VertTopOffset + 0.1f;
                data.State.CurrentSuperState = SuperState.IsFalling;
                data.Movement.Velocity = Vector2.Zero;
            }
            else
            {
                HandleHorizontalInAirCollision(data, box, rect, ref positionX);
            }
        }

        /// <summary>
        /// Handles collision when the entity is on the ground.
        /// </summary>
        /// <param name="data">The EntityData containing the entity's components.</param>
        /// <param name="box">The entity's collision box.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">The entity's current X position.</param>
        private void HandleGroundCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX)
        {

            if (data.State.CurrentState == State.Slide)
            {
                data.State.CurrentState = State.Idle;
            }

            if (data.Movement.Velocity.X > 0 && box.Left <= rect.Left)
            {
                positionX = rect.Left - data.CollisionBox.OriginalWidth + data.CollisionBox.HorRightOffset - 0.1f;
                data.State.CanMoveRight = false;
            }
            if (data.Movement.Velocity.X < 0 && box.Right >= rect.Right)
            {
                positionX = rect.Right - data.CollisionBox.HorRightOffset + 0.1f;
                data.State.CanMoveLeft = false;
            }
        }

        /// <summary>
        /// Handles horizontal collision when the entity is in air (jumping or falling).
        /// </summary>
        /// <param name="data">The EntityData containing the entity's components.</param>
        /// <param name="box">The entity's collision box.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">The entity's current X position.</param>
        private void HandleHorizontalInAirCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX)
        {

            if (data.Movement.Velocity.X > 0 && box.Left <= rect.Left)
            {
                positionX = rect.Left - data.CollisionBox.OriginalWidth + data.CollisionBox.HorRightOffset;
                data.State.CanMoveRight = false;
                data.CollisionBox.SetSlidingLocation(rect.Bottom);
                data.State.CurrentSuperState = SuperState.IsFalling;
                data.Movement.Velocity = Vector2.Zero;
            }
            if (data.Movement.Velocity.X < 0 && box.Right >= rect.Right)
            {
                positionX = rect.Right - data.CollisionBox.HorRightOffset;
                data.State.CanMoveLeft = false;
                data.CollisionBox.SetSlidingLocation(rect.Bottom);
                data.State.CurrentSuperState = SuperState.IsFalling;
                data.Movement.Velocity = Vector2.Zero;
            }

        }

        /// <summary>
        /// Draws the system's state using the provided SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!GameConstants.DisplayCollisionBoxes)
            {
                return;
            }
            foreach (EntityData data in _entitiesData)
            {
                Texture2D collisionBox = Loader.collisionBox;

                var color = Color.Orange;
                spriteBatch.Draw(collisionBox, data.CollisionBox.GetRectangle(), color);
            }
        }
    }
}