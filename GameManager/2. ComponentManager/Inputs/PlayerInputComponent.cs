using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EC_Framework
{
    /// <summary>
    /// Component representing the input state of the player entity.
    /// </summary>
    public class PlayerInputComponent : Component
    {
        /// <summary>
        /// Indicates whether the left key is currently pressed.
        /// </summary>
        public bool IsLeftKeyDown { get; private set; }
        
        /// <summary>
        /// Indicates whether the right key is currently pressed.
        /// </summary>
        public bool IsRightKeyDown { get; private set; }
        
        /// <summary>
        /// Indicates whether the jump key is currently pressed.
        /// </summary>
        public bool IsJumpKeyDown { get; private set; }

        public PlayerInputComponent()
        {
        }

        /// <summary>
        /// Updates the component's input state based on the current keyboard state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            IsLeftKeyDown = keyboardState.IsKeyDown(Keys.A);
            IsRightKeyDown = keyboardState.IsKeyDown(Keys.D);
            IsJumpKeyDown = keyboardState.IsKeyDown(Keys.Space);
        }
    }
}
