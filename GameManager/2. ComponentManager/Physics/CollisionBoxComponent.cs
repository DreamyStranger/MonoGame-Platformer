using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> that contains data and methods related to collision box of an entity in the game.
    /// </summary>
    /// <remarks>
    /// This component contains properties for the original width and height of the entity, width and height of the entity's collision box,
    /// dimensions of vertical and horizontal offsets, and position of the collision box.
    /// </remarks>
    public class CollisionBoxComponent : Component
    {
        /// <summary>
        /// The original width of the collision box.
        /// </summary>
        public int OriginalWidth { get; private set; }

        /// <summary>
        /// The original height of the collision box.
        /// </summary>
        public int OriginalHeight { get; private set; }

        /// <summary>
        /// The current width of the collision box.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The current height of the collision box.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The vertical top offset of the collision box.
        /// </summary>
        public int VertTopOffset { get; private set; }

        /// <summary>
        /// The vertical bottom offset of the collision box.
        /// </summary>
        public int VertBottomOffset { get; private set; }

        /// <summary>
        /// The horizontal left offset of the collision box.
        /// </summary>
        public int HorLeftOffset { get; private set; }

        /// <summary>
        /// The horizontal right offset of the collision box.
        /// </summary>
        public int HorRightOffset { get; private set; }

        /// <summary>
        /// The collision box for the entity.
        /// </summary>
        private Rectangle _entityCollisionBox;

        /// <summary>
        /// The left boundary of the platform the entity is currently standing on.
        /// </summary>
        private int _groundLeft = 0;

        /// <summary>
        /// // The right boundary of the platform entity currently standing on.
        /// </summary>
        private int _groundRight = GameConstants.SCREEN_WIDTH;

        /// <summary>
        /// // The bottom boundary of the platform entity currently standing on.
        /// </summary>
        private int _groundBottom = GameConstants.SCREEN_HEIGHT;


        /// <summary>
        /// Creates a new instance of the CollisionBoxComponent class.
        /// </summary>
        /// <param name="position">The position of the collision box in the game world.</param>
        /// <param name="width">The width of the collision box.</param>
        /// <param name="height">The height of the collision box.</param>
        /// <param name="vertTopOffset">The vertical offset from the top of the collision box.</param>
        /// <param name="vertBottomOffset">The vertical offset from the bottom of the collision box.</param>
        /// <param name="horLeftOffset">The horizontal offset from the left of the collision box.</param>
        /// <param name="horRightOffset">The horizontal offset from the right of the collision box.</param>
        public CollisionBoxComponent(Vector2 position, int width, int height,
                                int vertTopOffset = 0, int vertBottomOffset = 0, int horLeftOffset = 0, int horRightOffset = 0)
        {
            this.OriginalWidth = width;
            this.OriginalHeight = height;
            this.VertTopOffset = vertTopOffset;
            this.VertBottomOffset = vertBottomOffset;
            this.HorLeftOffset = horLeftOffset;
            this.HorRightOffset = horRightOffset;
            this.Width = width - horLeftOffset - horRightOffset;
            this.Height = height - vertBottomOffset - vertTopOffset;
            this._entityCollisionBox = new Rectangle((int)position.X + horLeftOffset, (int)position.Y + vertTopOffset, this.Width, this.Height);
        }

        /// <summary>
        /// Updates the position of the collision box based on the entity's position and direction.
        /// </summary>
        /// <param name="positionX">The X position of the entity in the game world.</param>
        /// <param name="positionY">The Y position of the entity in the game world.</param>
        /// <param name="direction">The direction the entity is facing (1 for right, -1 for left).</param>
        public void UpdateBoxPosition(float positionX, float positionY, int direction)
        {
            switch (direction)
            {
                case -1:
                    _entityCollisionBox.X = (int)positionX + HorRightOffset;
                    break;

                case 1:
                    _entityCollisionBox.X = (int)positionX + HorLeftOffset;
                    break;
            }

            _entityCollisionBox.Y = (int)positionY + VertTopOffset;

        }

        /// <summary>
        /// Retrieves the collision box as a Rectangle object.
        /// </summary>
        /// <returns>The collision box as a Rectangle object.</returns>
        public Rectangle GetRectangle()
        {
            return _entityCollisionBox;
        }

        /// <summary>
        /// Sets the boundaries of the platform for checking if the entity is on the ground.
        /// </summary>
        /// <param name="left">The left boundary of the platform.</param>
        /// <param name="right">The right boundary of the platform.</param>
        public void SetGroundLocation(int left, int right)
        {
            _groundLeft = left;
            _groundRight = right;
        }

        /// <summary>
        /// Sets the boundaries of the platform for checking if the entity is on the ground.
        /// </summary>
        /// <param name="left">The left boundary of the platform.</param>
        /// <param name="right">The right boundary of the platform.</param>
        public void SetSlidingLocation(int bottom)
        {
            _groundBottom = bottom;
        }

        /// <summary>
        /// Checks if the entity is in the air (not on the platform).
        /// </summary>
        /// <param name="left">The left boundary of the entity.</param>
        /// <returns>True if the entity is in the air, false otherwise.</returns>
        public bool CheckIfInAir(float position, int direction)
        {
            float left = position + HorLeftOffset;
            float right = position - HorRightOffset + OriginalWidth;
            if (direction == -1)
            {
                left = position + HorRightOffset;
                right = position - HorLeftOffset + OriginalWidth;
            }
            return right < _groundLeft || left > _groundRight;
        }

        /// <summary>
        /// Checks if the entity is below platform when sliding.
        /// </summary>
        /// <param name="position">The top boundary of the entity.</param>
        /// <returns>True if the entity is in the air, false otherwise.</returns>
        public bool CheckIfbelow(float position)
        {

            float top = position + VertTopOffset + 5;
            return top > _groundBottom;
        }
    }
}