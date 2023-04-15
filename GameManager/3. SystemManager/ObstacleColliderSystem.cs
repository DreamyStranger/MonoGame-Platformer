using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class ObstacleColliderSystem : System
    {
        private List<EntityData> entitiesData;
        private Dictionary<string, List<Rectangle>> obstacles;

        public ObstacleColliderSystem(string LevelID)
        {
            entitiesData = new List<EntityData>();
            obstacles = Loader.tiledHandler.obstacles[LevelID];
        }

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

        public override void RemoveEntity(Entity entity)
        {
            entitiesData.RemoveAll(data => data.Entity == entity);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in entitiesData)
            {
                data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.Movement.HorizontalDirection);
                Rectangle box = data.CollisionBox.GetRectangle();

                //Debug Message
                Console.WriteLine($"StateID: {data.State.stateID}");
                float positionX = data.Movement.Position.X;
                float positionY = data.Movement.Position.Y;
                bool touchingRight = false;
                bool touchingLeft = false;

                foreach (string key in obstacles.Keys)
                {
                    foreach (Rectangle rect in obstacles[key])
                    {
                        Rectangle intersection = Rectangle.Intersect(box, rect);
                        if (intersection.IsEmpty)
                        {
                            continue;
                        }

                        int depthX = intersection.Width;
                        int depthY = intersection.Height;

                        bool collidesWithRightSide = box.Left < rect.Right && box.Right >= rect.Right;
                        bool collidesWithLeftSide = box.Right > rect.Left && box.Left <= rect.Left;
                        bool collidesWithBottomSide = box.Top < rect.Bottom && box.Bottom >= rect.Bottom;
                        bool collidesWithTopSide = box.Bottom > rect.Top && box.Top <= rect.Top;

                        switch (data.State.currentSuperState)
                        {
                            case SuperState.isFalling:
                                if (collidesWithTopSide)
                                {
                                    positionY = rect.Top - data.CollisionBox.originalHeight;
                                    data.State.SetSuperState(SuperState.OnGround);
                                    data.CollisionBox.SetGroundLocation(rect.Left, rect.Right);
                                }
                                goto default;

                            case SuperState.OnGround:
                                if (data.State.IsState(ObjectState.Slide)) data.State.SetState(ObjectState.Idle);

                                if (data.Movement.Velocity.X > 0 && collidesWithLeftSide)
                                {
                                    positionX = rect.Left - data.CollisionBox.originalWidth + data.CollisionBox.horRightOffset;
                                    touchingRight = true;
                                }
                                else if (data.Movement.Velocity.X < 0 && collidesWithRightSide)
                                {
                                    positionX = rect.Right - data.CollisionBox.horRightOffset;
                                    touchingLeft = true;
                                }
                                break;

                            case SuperState.isJumping:
                            case SuperState.isDoubleJumping:
                                if (collidesWithBottomSide && key != "float")
                                {
                                    positionY = rect.Bottom - data.CollisionBox.vertTopOffset;
                                }
                                data.State.SetSuperState(SuperState.isFalling);
                                goto default;

                            default:
                                if (depthX < depthY && data.Movement.Velocity.X > 0 && box.Left <= rect.Left)
                                {
                                    positionX = rect.Left - data.CollisionBox.originalWidth + data.CollisionBox.horRightOffset;
                                    touchingRight = true;
                                }
                                else if (depthX < depthY && data.Movement.Velocity.X < 0 && box.Right >= rect.Right)
                                {
                                    positionX = rect.Right - data.CollisionBox.horRightOffset;
                                    touchingLeft = true;
                                }

                                data.Movement.Velocity = Vector2.Zero;
                                break;
                        }

                        Console.WriteLine($"Obstacle: {rect.ToString()}, StateID: {data.State.stateID}");

                        data.Movement.Position = new Vector2(positionX, positionY);
                        data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.Movement.HorizontalDirection);
                    }

                    if (data.CollisionBox.checkIfInAir(data.Movement.Position.X + data.CollisionBox.originalWidth, data.Movement.Position.X))
                    {
                        if (data.State.IsSuperState(SuperState.OnGround))
                        {
                            data.State.SetSuperState(SuperState.isFalling);
                        }
                    }

                    data.Movement.CanMoveRight = true;
                    data.Movement.CanMoveLeft = true;
                    if (touchingLeft)
                    {
                        data.Movement.CanMoveRight = false;
                        data.State.SetState(ObjectState.Slide);
                    }
                    if (touchingRight)
                    {
                        data.Movement.CanMoveLeft = false;
                        data.State.SetState(ObjectState.Slide);
                    }

                }
            }
        }

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
    }
}
