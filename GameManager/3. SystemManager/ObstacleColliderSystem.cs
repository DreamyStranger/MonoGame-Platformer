using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// The ObstacleColliderSystem class manages collision detection and response for game entities
    /// with obstacles in the game environment.
    /// </summary>
    public class ObstacleColliderSystem : System
    {
        private List<EntityData> entitiesData;
        /// <summary>
        /// Dictionary containing the obstacle data for each layer in the game environment.
        /// </summary>
        private Dictionary<string, List<Rectangle>> obstacles;

        /// <summary>
        /// Initializes a new instance of the ObstacleColliderSystem class.
        /// </summary>
        /// <param name="LevelID">The identifier for the level's obstacle data.</param>

        public ObstacleColliderSystem(LevelID levelID)
        {
            entitiesData = new List<EntityData>();
            obstacles = new Dictionary<string, List<Rectangle>>();
            obstacles = Loader.tiledHandler.obstacles[levelID.ToString()];

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

            entitiesData.Add(new EntityData
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
            entitiesData.RemoveAll(data => data.Entity == entity);
        }

        /// <summary>
        /// Handles Collisions between all 
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in entitiesData)
            {
                data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
                Rectangle box = data.CollisionBox.GetRectangle();
                float positionX = data.Movement.Position.X;
                float positionY = data.Movement.Position.Y;
                data.State.CanMoveRight = true;
                data.State.CanMoveLeft = true;

                foreach (string key in obstacles.Keys)
                {
                    //Console.WriteLine($"Layer Name: {key}");  //Debug message

                    foreach (Rectangle rect in obstacles[key])
                    {
                        if (!box.Intersects(rect))
                        {
                            continue;
                        }

                        switch (data.State.currentSuperState)
                        {
                            case SuperState.IsFalling:
                                HandleFallCollision(data, box, rect, ref positionX, ref positionY);
                                break;

                            case SuperState.OnGround:
                                HandleGroundCollision(data, box, rect, ref positionX);
                                break;

                            case SuperState.IsJumping:
                            case SuperState.IsDoubleJumping:
                                HandleJumpCollision(data, box, rect, ref positionX, ref positionY, key);
                                break;

                            default:
                                break;
                        }
                        //Console.WriteLine($"Obstacle: {rect.ToString()}, StateID: {data.State.stateID}"); //Debug Message

                        //Update entity's position and collisionBox
                        data.Movement.Position = new Vector2(positionX, positionY);
                        data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
                    }

                    //Check if entity still on a platform
                    if (data.CollisionBox.checkIfInAir(data.Movement.Position.X + data.CollisionBox.originalWidth, data.Movement.Position.X))
                    {
                        if (data.State.IsSuperState(SuperState.OnGround))
                        {
                            data.State.SetSuperState(SuperState.IsFalling);
                        }
                    }
                }
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
            foreach (EntityData data in entitiesData)
            {
                data.CollisionBox.DrawCollisionBoxes(spriteBatch);
            }
        }

        private void HandleFallCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY)
        {
            bool collidesWithTopSide = box.Bottom > rect.Top && box.Top <= rect.Top;
            if (collidesWithTopSide)
            {
                positionY = rect.Top - data.CollisionBox.originalHeight + data.CollisionBox.vertBottomOffset;
                data.State.SetSuperState(SuperState.OnGround);
                data.CollisionBox.SetGroundLocation(rect.Left, rect.Right);
            }
            else
            {
                HandleHorizontalInAirCollision(data, box, rect, ref positionX);
            }
        }

        private void HandleJumpCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY, string key)
        {
            if (key == "float")
            {
                return;
            }
            bool collidesWithBottomSide = box.Top < rect.Bottom && box.Bottom >= rect.Bottom;
            if (collidesWithBottomSide)
            {
                positionY = rect.Bottom - data.CollisionBox.vertTopOffset;
            }
            HandleHorizontalInAirCollision(data, box, rect, ref positionX);
        }

        private void HandleGroundCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX)
        {
            bool collidesWithRightSide = box.Left < rect.Right && box.Right >= rect.Right;
            bool collidesWithLeftSide = box.Right > rect.Left && box.Left <= rect.Left;

            if (data.State.IsState(ObjectState.Slide)) data.State.SetState(ObjectState.Idle);

            if (data.Movement.Velocity.X > 0 && collidesWithLeftSide)
            {
                positionX = rect.Left - data.CollisionBox.originalWidth + data.CollisionBox.horRightOffset;
                data.State.CanMoveRight = false;
            }
            else if (data.Movement.Velocity.X < 0 && collidesWithRightSide)
            {
                positionX = rect.Right - data.CollisionBox.horRightOffset;
                data.State.CanMoveLeft = false;
            }
        }

        private void HandleHorizontalInAirCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX)
        {
            Rectangle intersection = Rectangle.Intersect(box, rect);
            int depthX = intersection.Width;
            int depthY = intersection.Height;
            if (depthX < depthY)
            {
                if (data.Movement.Velocity.X > 0 && box.Left <= rect.Left)
                {
                    positionX = rect.Left - data.CollisionBox.originalWidth + data.CollisionBox.horRightOffset;
                    data.State.CanMoveRight = false;
                }
                else if (data.Movement.Velocity.X < 0 && box.Right >= rect.Right)
                {
                    positionX = rect.Right - data.CollisionBox.horRightOffset;
                    data.State.CanMoveLeft = false;
                }
            }
            data.Movement.Velocity = Vector2.Zero;
            data.State.SetSuperState(SuperState.IsFalling);
        }
    }
}
