using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> taht represents the input state of the player entity.
    /// </summary>
    public class PlayerInputComponent : Component
    {
        /// <summary>
        /// Gets a value indicating whether the key for moving the player to the left is currently being pressed.
        /// </summary>
        public bool IsLeftKeyDown { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the key for moving the player to the right is currently being pressed.
        /// </summary>
        public bool IsRightKeyDown { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the key for making the player jump is currently being pressed.
        /// </summary>
        public bool IsJumpKeyDown { get; private set; }

        /// <summary>
        /// Updates the component's input state based on the current keyboard state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            IsLeftKeyDown = keyboardState.IsKeyDown(GameConstants.LEFT_KEY);
            IsRightKeyDown = keyboardState.IsKeyDown(GameConstants.RIGHT_KEY);
            IsJumpKeyDown = keyboardState.IsKeyDown(GameConstants.JUMP_KEY);
        }
    }
}
