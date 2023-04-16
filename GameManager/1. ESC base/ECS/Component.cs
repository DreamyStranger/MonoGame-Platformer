using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// The base class for all components.
    /// </summary>
    public abstract class Component
    {
        public virtual void OnDestroy()
        {
        }
    }
}