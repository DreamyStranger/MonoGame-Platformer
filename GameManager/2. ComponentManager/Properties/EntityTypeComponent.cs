using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ECS_Framework
{
    public class EntityTypeComponent : Component
    {
        public EntityType Type { get; private set; }

        public EntityTypeComponent(EntityType type)
        {
            Type = type;
        }
    }
}