using System;
using System.Collections.Generic;

namespace ECS_Framework
{
    /// <summary>
    /// Represents an entity in the game world. An entity is a collection of components that define its behavior and appearance.
    /// </summary>
    public class Entity
    {
        private Dictionary<Type, List<Component>> components;

        /// <summary>
        /// The ID of the entity.
        /// </summary>
        public int id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class with a specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>        
        public Entity(int id = 1)
        {
            components = new Dictionary<Type, List<Component>>();
            this.id = id;
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddComponent(Component component)
        {
            Type type = component.GetType();
            if (!ComponentExists(type, component))
            {
                if (!components.TryGetValue(type, out List<Component> list))
                {
                    list = new List<Component>();
                    components.Add(type, list);
                }
                list.Add(component);
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
            if (ComponentExists(type, component))
            {
                components[type].Remove(component);
            }
            else
            {
                Console.WriteLine("Tried to remove a component that doesn't exist!");
                return;
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
            if (components.TryGetValue(type, out List<Component> list))
            {
                foreach (Component component in list)
                {
                    if (component is T tComponent)
                    {
                        return tComponent;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all the components of the entity.
        /// </summary>
        /// <returns>A list of all the components of the entity.</returns>
        public List<Component> GetAllComponents()
        {
            List<Component> componentList = new List<Component>();
            foreach (var list in components.Values)
            {
                componentList.AddRange(list);
            }
            return componentList;
        }

        /// <summary>
        /// Checks if a component of a specified type exists in the entity.
        /// </summary>
        /// <param name="type">The type of the component to check.</param>
        /// <param name="component">The component to check.</param>
        /// <returns>true if the component exists, false otherwise.</returns>
        private bool ComponentExists(Type type, Component component)
        {
            if (components.TryGetValue(type, out List<Component> list))
            {
                return list.Contains(component);
            }
            return false;
        }
    }
}
