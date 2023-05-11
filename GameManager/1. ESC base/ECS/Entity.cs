using System;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Represents an entity in the game world. An entity is a collection of components that define its behavior and appearance.
    /// </summary>
    public class Entity
    {
        // Stores a dictionary where the key is the Type of the Component and the value is the Component instance.
        private Dictionary<Type, Component> components;

        /// <summary>
        /// Initializes a new instance of the Entity class.
        /// </summary>
        public Entity()
        {
            components = new Dictionary<Type, Component>();
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddComponent(Component component)
        {
            Type type = component.GetType();
            if (!components.ContainsKey(type))
            {
                components.Add(type, component);
            }
            else
            {
                Console.WriteLine($"Component of type {type} already exists!");
            }
        }

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove.</typeparam>
        /// <param name="component">The component to remove.</param>
        public void RemoveComponent<T>(T component) where T : Component
        {
            Type type = typeof(T);
            if (components.ContainsKey(type) && components[type] == component)
            {
                components.Remove(type);
            }
            else
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
            if (components.TryGetValue(type, out Component component) && component is T tComponent)
            {
                return tComponent;
            }
            return null;
        }

        /// <summary>
        /// Gets all the components of the entity.
        /// </summary>
        /// <returns>A list of all the components of the entity.</returns>
        public List<Component> GetAllComponents()
        {
            List<Component> componentList = new List<Component>(components.Values);
            return componentList;
        }
    }
}
