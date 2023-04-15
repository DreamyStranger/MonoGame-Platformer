using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// Represents a rectangular collision box for an entity in the game.
    /// </summary>
    public class CollisionBoxComponent : Component
    {
        // Collision box variables
        private Rectangle entityCollisionBox;  // The collision box for the entity.
        public int originalWidth { get; private set; }  // The original width of the collision box.
        public int originalHeight { get; private set; }  // The original height of the collision box.
        public int width { get; private set; }  // The current width of the collision box.
        public int height { get; private set; }  // The current height of the collision box.
        public int vertTopOffset { get; private set; }  // The vertical top offset of the collision box.
        public int vertBottomOffset { get; private set; }  // The vertical bottom offset of the collision box.
        public int horLeftOffset { get; private set; }  // The horizontal left offset of the collision box.
        public int horRightOffset { get; private set; }  // The horizontal right offset of the collision box.

        // Variables to check if an entity is on a platform
        private int groundLeft = 0;  // The left boundary of the platform.
        private int groundRight = GameConstants.SCREEN_WIDTH;  // The right boundary of the platform.

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
        public CollisionBoxComponent(Vector2 position, int width = 32, int height = 32,
                                int vertTopOffset = 8, int vertBottomOffset = 0, int horLeftOffset = 4, int horRightOffset = 6)
        {
            this.originalWidth = width;
            this.originalHeight = height;
            this.vertTopOffset = vertTopOffset;
            this.vertBottomOffset = vertBottomOffset;
            this.horLeftOffset = horLeftOffset;
            this.horRightOffset = horRightOffset;
            this.width = width - horLeftOffset - horRightOffset;
            this.height = height - vertBottomOffset - vertTopOffset;
            this.entityCollisionBox = new Rectangle((int)position.X + horLeftOffset, (int)position.Y + vertTopOffset, this.width, this.height);
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
                    entityCollisionBox.X = (int)positionX + horRightOffset;
                    break;

                case 1:
                    entityCollisionBox.X = (int)positionX + horLeftOffset;
                    break;
            }

            entityCollisionBox.Y = (int)positionY + vertTopOffset;

        }

        /// <summary>
        /// Draws the collision box for debugging purposes.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw the collision box.</param>
        public void DrawCollisionBoxes(SpriteBatch spriteBatch)
        {
            Texture2D collisionBox = Loader.collisionBox;

            var color = Color.Orange;
            spriteBatch.Draw(collisionBox, entityCollisionBox, color);
        }

        /// <summary>
        /// Retrieves the collision box as a Rectangle object.
        /// </summary>
        /// <returns>The collision box as a Rectangle object.</returns>
        public Rectangle GetRectangle()
        {
            return entityCollisionBox;
        }

        /// <summary>
        /// Sets the boundaries of the platform for checking if the entity is on the ground.
        /// </summary>
        /// <param name="left">The left boundary of the platform.</param>
        /// <param name="right">The right boundary of the platform.</param>
        public void SetGroundLocation(int left, int right)
        {
            groundLeft = left;
            groundRight = right;
        }
        
        /// <summary>
        /// Checks if the entity is in the air (not on the platform).
        /// </summary>
        /// <param name="left">The left boundary of the entity.</param>
        /// <param name="right">The right boundary of the entity.</param>
        /// <returns>True if the entity is in the air, false otherwise.</returns>
        public bool checkIfInAir(float left, float right)
        {
            return left < groundLeft || right > groundRight;
        }
    }
}