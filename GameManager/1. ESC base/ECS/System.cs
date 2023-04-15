using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{

    public abstract class System
    {
        public abstract void AddEntity(Entity entity);
        public abstract void RemoveEntity(Entity entity);

        public virtual void Update(GameTime gameTime) 
        {
            
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
