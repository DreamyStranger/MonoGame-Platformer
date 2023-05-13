using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// Represents the health of an entity.
    /// </summary>
    public class HealthComponent : Component
    {
        private int _currentHealth;
        private int _maxHealth;

        /// <summary>
        /// Gets or sets the current health value.
        /// </summary>
        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = MathHelper.Clamp(value, 0, MaxHealth);
        }

        /// <summary>
        /// Gets or sets the maximum health value.
        /// </summary>
        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = MathHelper.Max(0, value);
        }

        /// <summary>
        /// Initializes a new instance of the HealthComponent class with the specified max health value.
        /// </summary>
        /// <param name="maxHealth">The maximum health value.</param>
        public HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}