using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MyGame
{
    // <summary>
    /// Input system that updates all entities with a StateComponent and InputComponent.
    /// It manages the communication between the two components, allowing entities to respond to user input
    /// </summary>
    public class InputSystem : System
    {
        private List<Entity> entities;
        private List<StateComponent> states;
        private List<InputComponent> inputs;

        /// <summary>
        /// Constructor for InputSystem. Initializes lists for entities, states, and inputs.
        /// </summary>
        public InputSystem()
        {
            entities = new List<Entity>();
            states = new List<StateComponent>();
            inputs = new List<InputComponent>();
        }

        /// <summary>
        /// Adds an entity to the system if it has a StateComponent and an InputComponent.
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            InputComponent input = entity.GetComponent<InputComponent>();
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
        /// <param name="entity">The entity to be removed</param>
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
        /// Updates all entities' InputComponents based on their current StateComponents.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Update(gameTime, states[i]);
            }
        }
    }
}
