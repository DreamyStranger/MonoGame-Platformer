using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ECS_Framework
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
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Update(gameTime);
                UpdateEntityState(gameTime, inputs[i], states[i]);
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
            bool isNotBothKeys = !(input.IsLeftKeyDown && input.IsRightKeyDown);
            bool bothKeysUp = !input.IsLeftKeyDown && !input.IsRightKeyDown;

            switch (state.currentState)
            {
                default:
                    if (input.IsLeftKeyDown && isNotBothKeys)
                    {
                        if (state.CanMoveLeft)
                        {
                            state.SetState(ObjectState.WalkLeft);
                        }
                    }
                    else if (input.IsRightKeyDown && isNotBothKeys)
                    {
                        if (state.CanMoveRight)
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

            switch (state.currentSuperState)
            {
                case SuperState.OnGround:
                    state.JumpsPerformed = 0;
                    if (input.IsJumpKeyDown && isNotBothKeys)
                    {
                        state.JumpsPerformed = 1;
                        state.SetState(ObjectState.Jump);
                    }
                    break;

                case SuperState.IsFalling:
                    if (state.currentState == ObjectState.Slide) state.JumpsPerformed = 2;
                    if (input.IsJumpKeyDown && isNotBothKeys)
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

