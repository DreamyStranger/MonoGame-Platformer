using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> that represents a respawn timer for an entity.
    /// </summary>
    public class RespawnComponent : Component
    {

        private readonly float _respawnTimer;
        private float _currentTimer;
        private bool _isRespawning;

        /// <summary>
        /// Gets a value indicating whether the entity is currently respawning.
        /// </summary>
        public bool IsRespawning => _isRespawning;

        /// <summary>
        /// Gets and sets respawn position for the entity.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RespawnComponent"/> class with the specified respawn timer.
        /// </summary>
        /// <param name="respawnTimer">The duration in seconds for the entity to respawn.</param>
        public RespawnComponent(Vector2 respawnPosition, float respawnTimer = 5)
        {
            _respawnTimer = respawnTimer;
            _currentTimer = 0f;
            _isRespawning = false;
            Position = respawnPosition;
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
                    // Respawned
                    _isRespawning = false;
                }
            }
        }
    }
}
