using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace ECS_Framework
{
    public class AppearSystem : System
    {
        private List<Entity> _entities;
        private List<Entity> _destroy;

        public AppearSystem()
        {
            _entities = new List<Entity>();
            _destroy = new List<Entity>();
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
            if(_entities.Count == 0)
            {
                //Console.WriteLine("IsEmpty!");  //Making sure the list is empty after entities appeared
                return;
            }
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                Entity entity = _entities[i];
                StateComponent stateComponent = entity.GetComponent<StateComponent>();
                AnimatedComponent animatedComponent = entity.GetComponent<AnimatedComponent>();

                if (stateComponent.CurrentSuperState == SuperState.IsAppearing)
                {
                    ActionAnimation appearAnimation = animatedComponent.GetCurrentAnimation();

                    if (appearAnimation.IsFinished)
                    {
                        _destroy.Add(entity);
                        stateComponent.CurrentSuperState = stateComponent.DefaultSuperState;
                        stateComponent.CurrentState = stateComponent.DefaultState;
                    }
                }
            }

            //removed entities that already appeared
            foreach(Entity entity in _destroy)
            {
                RemoveEntity(entity);
            }

            //Clear out destroy List
            _destroy.Clear();
        }
    }
}
