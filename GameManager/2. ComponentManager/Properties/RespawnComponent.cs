using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// Represents a respawn component for an entity.
    /// </summary>
    public class RespawnComponent : Component
    {
        public Vector2 position { get; private set; }
        private readonly float _respawnTimer;
        private float _currentTimer;
        private bool _isRespawning;

        /// <summary>
        /// Gets or sets the remaining time until the entity respawns.
        /// </summary>
        public float RemainingTime => _respawnTimer - _currentTimer;

        /// <summary>
        /// Gets a value indicating whether the entity is currently respawning.
        /// </summary>
        public bool IsRespawning => _isRespawning;

        /// <summary>
        /// Initializes a new instance of the RespawnComponent class with the specified respawn timer.
        /// </summary>
        /// <param name="respawnTimer">The duration in seconds for the entity to respawn.</param>
        public RespawnComponent(Vector2 respawnPosition, float respawnTimer = 5)
        {
            _respawnTimer = respawnTimer;
            _currentTimer = 0f;
            _isRespawning = false;
            this.position = respawnPosition;
        }

        /// <summary>
        /// Starts the respawn timer for the entity.
        /// </summary>
        public void StartRespawn()
        {
            _currentTimer = 0f;
            _isRespawning = true;
        }

        /// <summary>
        /// Updates the respawn component.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(GameTime gameTime)
        {
            if (_isRespawning)
            {
                _currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_currentTimer >= _respawnTimer)
                {
                    // Respawn the entity
                    _isRespawning = false;
                }
            }
        }
    }
}
