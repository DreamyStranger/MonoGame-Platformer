using System.Collections.Generic;
using System;

namespace MyGame {

    public struct ObjectInitializer
    {
        private List<Entity> objects;

        public ObjectInitializer()
        {
            objects = new List<Entity>();
            objects.Add(EntityFactory.CreateParallaxBackground());
            objects.Add(EntityFactory.CreatePlayer());
        }

        public List<Entity> GetObjects()
        {
            return objects;
        }
    }
}
