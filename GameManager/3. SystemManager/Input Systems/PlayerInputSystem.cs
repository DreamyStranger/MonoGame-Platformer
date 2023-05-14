using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> that updates the state of entities based on keyboard input.
    /// </summary>
    public class PlayerInputSystem : System
    {
        private List<Entity> entities;
        private List<StateComponent> states;
        private List<PlayerInputComponent> inputs;

        /// <summary>
        /// Initializes a new instance of the PlayerInputSystem class.
        /// </summary>
        public PlayerInputSystem()
        {
            entities = new List<Entity>();
            states = new List<StateComponent>();
            inputs = new List<PlayerInputComponent>();
        }

        /// <summary>
        /// Adds an entity to the system.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            PlayerInputComponent input = entity.GetComponent<PlayerInputComponent>();
            if (state == null || input == null)
            {
                return;
            }

            entities.Add(entity);
            states.Add(state);
            inputs.Add(input);
        }

        /// <summary>
        /// Removes an entity from the system.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            int index = entities.IndexOf(entity);
            if (index != -1)
            {
                entities.RemoveAt(index);
                states.RemoveAt(index);
                inputs.RemoveAt(index);
            }
        }

        /// <summary>
        /// Updates the system's state based on input from the keyboard.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            int n = inputs.Count;
            for (int i = 0; i < n; i++)
            {
                Entity entity = entities[i];
                StateComponent state = states[i];
                PlayerInputComponent input = inputs[i];

                if (!entity.IsActive || state.CurrentSuperState == SuperState.IsDead || state.CurrentSuperState == SuperState.IsAppearing)
                {
                    continue;
                }

                input.Update(gameTime);
                UpdateEntityState(gameTime, input, state);
            }
        }

        /// <summary>
        /// Updates the state of an entity based on its input and current state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <param name="input">The input component for the entity.</param>
        /// <param name="state">The state component for the entity.</param>
        private void UpdateEntityState(GameTime gameTime, PlayerInputComponent input, StateComponent state)
        {
            bool bothKeysDown = input.IsLeftKeyDown && input.IsRightKeyDown;
            bool bothKeysUp = !input.IsLeftKeyDown && !input.IsRightKeyDown;

            switch (state.CurrentState)
            {
                default:
                    if (bothKeysDown || bothKeysUp)
                    {
                        state.CurrentState = State.Idle;
                    }
                    else if (input.IsLeftKeyDown && state.CanMoveLeft)
                    {
                        state.CurrentState = State.WalkLeft;
                    }
                    else if (input.IsRightKeyDown && state.CanMoveRight)
                    {
                        state.CurrentState = State.WalkRight;
                    }
                    else if(!state.CanMoveLeft || !state.CanMoveRight)
                    {
                        state.CurrentState = State.Slide;
                    }
                    break;
            }

            switch (state.CurrentSuperState)
            {
                case SuperState.IsOnGround:
                    state.JumpsPerformed = 0;
                    if (input.IsJumpKeyDown && !bothKeysDown)
                    {
                        state.JumpsPerformed = 1;
                        state.CurrentState = State.Jump;
                    }
                    break;

                case SuperState.IsFalling:
                    if (state.CurrentState == State.Slide)
                    {
                        state.JumpsPerformed = 2;
                    }
                    else if (input.IsJumpKeyDown && !bothKeysDown)
                    {
                        if (state.JumpsPerformed == 0)
                        {
                            state.CurrentState = State.Jump;
                            state.JumpsPerformed = 1;
                        }
                        else if (state.JumpsPerformed == 1)
                        {
                            state.CurrentState = State.DoubleJump;
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

