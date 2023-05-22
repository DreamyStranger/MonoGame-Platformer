using System;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Represents an entity in the game world. An entity is a collection of components that define its behavior and appearance.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Stores a dictionary where the key is the Type of the Component and the value is the Component instance.
        /// </summary>
        private Dictionary<Type, Component> _components;

        /// <summary>
        /// Tells if the Entity is active.
        /// </summary>
        public bool IsActive;

        /// <summary>
        /// Initializes a new instance of the Entity class.
        /// </summary>
        /// <param name="isActive">Determines if the entity is active or not. Default is true.</param>
        public Entity(bool isActive = true)
        {
            _components = new Dictionary<Type, Component>();
            IsActive = isActive;
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddComponent(Component component)
        {
            Type type = component.GetType();
            if (!_components.ContainsKey(type))
            {
                _components.Add(type, component);
            }
            else if(GameConstants.EntityDebugMessages)
            {
                Console.WriteLine($"Component of type {type} already exists!");
            }
        }

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove.</typeparam>
        public void RemoveComponent<T>() where T : Component
        {
            Type type = typeof(T);
            if (_components.ContainsKey(type))
            {
                _components.Remove(type);
            }
            else if(GameConstants.EntityDebugMessages)
            {
                Console.WriteLine("Tried to remove a component that doesn't exist!");
            }
        }

        /// <summary>
        /// Gets a component of a specified type from the entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to get.</typeparam>
        /// <returns>The component of the specified type, or null if it doesn't exist.</returns>
        public T GetComponent<T>() where T : Component
        {
            Type type = typeof(T);
            if (_components.TryGetValue(type, out Component component) && component is T tComponent)
            {
                return tComponent;
            }
            else if(GameConstants.EntityDebugMessages)
            {
                Console.WriteLine("Tried to get a component that doesn't exist!");
            }
            return null;
        }

        /// <summary>
        /// Gets all the components of the entity.
        /// </summary>
        /// <returns>A list of all the components of the entity.</returns>
        public List<Component> GetAllComponents()
        {
            List<Component> componentList = new List<Component>(_components.Values);
            return componentList;
        }
    }
}
