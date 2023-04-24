using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ECS_Framework
{
    public class DeathAnimationSystem : System
    {
        private List<Entity> _entities;

        public DeathAnimationSystem()
        {
            _entities = new List<Entity>();
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.GetComponent<StateComponent>() != null && entity.GetComponent<AnimatedComponent>() != null)
            {
                _entities.Add(entity);
            }
        }

        public override void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                Entity entity = _entities[i];
                StateComponent stateComponent = entity.GetComponent<StateComponent>();
                AnimatedComponent animatedComponent = entity.GetComponent<AnimatedComponent>();

                if (stateComponent.CurrentSuperState == SuperState.IsDead)
                {
                    ActionAnimation deathAnimation = animatedComponent.GetCurrentAnimation();

                    // Check if the animation has completed
                    if (deathAnimation.IsFinished)
                    {
                        // Remove the entity
                        MessageBus.Publish(new DestroyEntityMessage(entity));
                    }
                }
            }
        }
    }
}
