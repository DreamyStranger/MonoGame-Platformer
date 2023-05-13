using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameExamples
{
    /// <summary>
    /// Component representing the movement state of a simple walking enemy entity.
    /// </summary>
    public class RegularEnemyComponent : Component
    {
        /// <summary>
        /// Indicates whether the enemy is moving left.
        /// </summary>
        public bool IsLeft { get; private set; }

        /// <summary>
        /// Indicates whether the enemy is moving right.
        /// </summary>
        public bool IsRight { get; private set; }

        private float _left;
        private float _right;

        /// <summary>
        /// Initializes a new instance of the SimpleWalkingEnemyComponent class.
        /// </summary>
        /// <param name="start">The starting position of the enemy on the x-axis.</param>
        /// <param name="leftRange">The range the enemy can move to the left.</param>
        /// <param name="rightRange">The range the enemy can move to the right.</param>
        public RegularEnemyComponent(float start, float leftRange, float rightRange)
        {
            _left = start - leftRange;
            _right = start + rightRange;
        }

        /// <summary>
        /// Updates the component's movement state based on the enemy's position.
        /// </summary>
        /// <param name="positionX">The enemy's position on the x-axis.</param>
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