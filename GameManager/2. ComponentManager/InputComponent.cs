using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    /// <summary>
    /// The InputComponent class is responsible for handling player input and updating the entity's state accordingly.
    /// </summary>
    public class InputComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the InputComponent class.
        /// </summary>
        public InputComponent()
        {
        }

        /// <summary>
        /// Updates the state of the entity based on keyboard input and the current state of the entity.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        /// <param name="state">The state component of the entity.</param>
        public void Update(GameTime gameTime, StateComponent state)
        {
            // Get the current state of the keyboard
            KeyboardState keyboardState = Keyboard.GetState();

            // Check if the left, right, or space keys are pressed
            bool isLeftKeyDown = keyboardState.IsKeyDown(Keys.A);
            bool isRightKeyDown = keyboardState.IsKeyDown(Keys.D);
            bool isSpaceKeyDown = keyboardState.IsKeyDown(Keys.Space);

            // Check if both the left and right keys are not pressed
            bool isNotBothKeys = !(isLeftKeyDown && isRightKeyDown);

            // Check if both the left and right keys are up
            bool bothKeysUp = !isLeftKeyDown && !isRightKeyDown;

            // Update the state of the entity based on the current keyboard input and state
            switch (state.currentState)
            {
                default:
                    if (isLeftKeyDown && isNotBothKeys)
                    {
                        if(state.CanMoveLeft) 
                        {
                            state.SetState(ObjectState.WalkLeft);
                        }
                    }
                    else if (isRightKeyDown && isNotBothKeys)
                    {
                        if(state.CanMoveRight) 
                        {
                            state.SetState(ObjectState.WalkRight);
                        }
                    }
                    else if (bothKeysUp)
                    {
                        state.SetState(ObjectState.Idle);
                    }
                    break;
            }

            // Update the state of the entity based on the current keyboard input and super state
            switch (state.currentSuperState)
            {
                case SuperState.OnGround:
                    state.JumpsPerformed = 0;
                    if (isSpaceKeyDown && isNotBothKeys)
                    {
                        state.JumpsPerformed = 1;
                        state.SetState(ObjectState.Jump);
                    }
                    break;

                case SuperState.IsFalling:
                    if(state.currentState == ObjectState.Slide) state.JumpsPerformed = 2;
                    if (isSpaceKeyDown && isNotBothKeys)
                    {
                        if (state.JumpsPerformed == 0)
                        {
                            state.SetState(ObjectState.Jump);
                            state.JumpsPerformed = 1;
                        }
                        if (state.JumpsPerformed == 1)
                        {
                            state.SetState(ObjectState.DoubleJump);
                            state.JumpsPerformed = 2;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}