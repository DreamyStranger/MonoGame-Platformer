using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ECS_Framework
{
    /// <summary>
    /// Component representing the input state of the player entity.
    /// </summary>
    public class SimpleWalkingEnemyComponent : Component
    {
        /// <summary>
        /// Indicates whether the enemy is moving left
        public bool IsLeft { get; private set; }

        /// <summary>
        /// Indicates whether the enemy is moving right
        /// </summary>
        public bool IsRight { get; private set; }

        private float _left;
        private float _right;


        public SimpleWalkingEnemyComponent(float start, float leftRange, float rightRange)
        {
            _left = start - leftRange;
            _right = start + rightRange;
        }

        /// <summary>
        /// Updates the component's input state based on the position
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(float positionX)
        {
            if (positionX <= _left)
            {
                IsRight = true;
                IsLeft = false;
            }
            else if (positionX >= _right)
            {
                IsLeft = true;
                IsRight = false;
            }
        }
    }
}
