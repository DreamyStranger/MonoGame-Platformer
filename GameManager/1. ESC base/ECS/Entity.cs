using System;
using System.Collections.Generic;

namespace MyGame
{
    public class Entity
    {
        private Dictionary<Type, List<Component>> components;
        public int id { get; private set; }
        public Entity(int id = 1)
        {
            components = new Dictionary<Type, List<Component>>();
            this.id = id;
        }

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

        public void RemoveComponent<T>(T component) where T : Component
        {
            Type type = typeof(T);
            if (ComponentExists(type, component))
            {
                components[type].Remove(component);
                component.OnDestroy();
            }
            else
            {
                Console.WriteLine("Tried to remove a component that doesn't exist!");
                return;
            }
        }

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

        public List<Component> GetAllComponents()
        {
            List<Component> componentList = new List<Component>();
            foreach (var list in components.Values)
            {
                componentList.AddRange(list);
            }
            return componentList;
        }

        public void Destroy()
        {
            foreach (var list in components.Values)
            {
                foreach (var component in list)
                {
                    component.OnDestroy();
                }
            }
        }

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
