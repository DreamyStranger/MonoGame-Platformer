using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ECS_Framework
{
    /// <summary>
    /// <see cref="System"/> that updates the state of entities based on keyboard input.
    /// </summary>
    public class SimpleWalkingEnemySystem : System
    {
        private List<Entity> entities;
        private List<StateComponent> states;
        private List<SimpleWalkingEnemyComponent> inputs;
        private List<MovementComponent> movements;


        /// <summary>
        /// Initializes a new instance of the PlayerInputSystem class.
        /// </summary>
        public SimpleWalkingEnemySystem()
        {
            entities = new List<Entity>();
            states = new List<StateComponent>();
            inputs = new List<SimpleWalkingEnemyComponent>();
            movements = new List<MovementComponent>();
        }

        /// <summary>
        /// Adds an entity to the system.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            SimpleWalkingEnemyComponent input = entity.GetComponent<SimpleWalkingEnemyComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            if (state == null || input == null || movement == null)
            {
                return;
            }

            entities.Add(entity);
            states.Add(state);
            inputs.Add(input);
            movements.Add(movement);
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
                movements.RemoveAt(index);
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Update(movements[i].Position.X);
                UpdateEntityState(gameTime, inputs[i], states[i]);
            }
        }

        private void UpdateEntityState(GameTime gameTime, SimpleWalkingEnemyComponent input, StateComponent state)
        {
            if (state.CurrentSuperState != SuperState.IsOnGround)
            {
                return;
            }
            switch (state.CurrentState)
            {
                case State.Idle:
                    state.CurrentState = State.WalkRight;
                    break;
                case State.WalkLeft:
                    if (input.IsRight)
                    {
                        state.CurrentState = State.WalkRight;
                    }
                    break;
                case State.WalkRight:
                    if (input.IsLeft)
                    {
                        state.CurrentState = State.WalkLeft;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

