using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameExamples
{
    //// <summary>
    /// <see cref="Component"/> responsible for controlling a regular enemy's movement within a specified range. 
    /// It decides the direction of movement based on the enemy's current position.
    /// </summary>
    public class RegularEnemyComponent : Component
    {
        /// <summary>
        /// Gets a value indicating whether the enemy should move to the left.
        /// </summary>
        public bool IsLeft { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the enemy should move to the right.
        /// </summary>
        public bool IsRight { get; private set; }

        private float _left;
        private float _right;

        /// <summary>
        /// Initializes a new instance of the RegularEnemyComponent class with a specific starting position and movement range.
        /// </summary>
        /// <param name="start">The starting position of the enemy on the x-axis.</param>
        /// <param name="leftRange">The distance the enemy is allowed to move to the left from the start position.</param>
        /// <param name="rightRange">The distance the enemy is allowed to move to the right from the start position.</param>
        public RegularEnemyComponent(float start, float leftRange, float rightRange)
        {
            _left = start - leftRange;
            _right = start + rightRange;
        }

        /// <summary>
        /// Updates the enemy's movement direction based on its current position.
        /// </summary>
        /// <param name="positionX">The enemy's current position on the x-axis.</param>
        public void Update(float positionX)
        {
            IsLeft = false;
            IsRight = false;

            if (positionX <= _left)
            {
                IsRight = true;
            }
            else if (positionX >= _right)
            {
                IsLeft = true;
            }
        }
    }
}