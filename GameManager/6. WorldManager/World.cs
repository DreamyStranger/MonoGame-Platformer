using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MonogameExamples
{
    /// <summary>
    /// Represents the game world and manages loading and updating levels.
    /// </summary>
    public class World
    {
        private LevelID _currentLevel;
        private SystemManager _systems;
        private int _totalLevels = Enum.GetValues(typeof(LevelID)).Length;

        private readonly Queue<Entity> _entitiesToDestroy;
        private readonly Queue<Entity> _entitiesToAdd;
        private bool _levelNeedsChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="World"> class.
        /// </summary>
        public World()
        {
            _systems = new SystemManager(CurrentLevel);
            _entitiesToDestroy = new Queue<Entity>();
            _entitiesToAdd  = new Queue<Entity>();

            MessageBus.Subscribe<AddEntityMessage>(OnCreateEntity);
            MessageBus.Subscribe<DestroyEntityMessage>(OnDestroyEntity);
            MessageBus.Subscribe<NextLevelMessage>(NextLevel);
            MessageBus.Subscribe<PreviousLevelMessage>(PreviousLevel);
            MessageBus.Subscribe<ReloadLevelMessage>(ResetCurrentLevel);

            ChangeLevel(LevelID.Level1);
        }

        /// <summary>
        /// Gets the current level.
        /// </summary>
        public LevelID CurrentLevel
        {
            get { return _currentLevel; }
        }

        /// <summary>
        /// Changes the current level to the specified level.
        /// </summary>
        /// <param name="level">The level to change to.</param>
        private void ChangeLevel(LevelID level)
        {
            _currentLevel = level;
            _levelNeedsChanging = true;
        }

        /// <summary>
        /// Loads the current level and its entities into the game world.
        /// </summary>
        private void LoadLevel()
        {
            MediaPlayer.Stop();
            _systems.Unsubscribe();
            _systems.ResetSystems(CurrentLevel);
            _systems.Subscribe();
            LevelLoader.GetObjects(CurrentLevel);
            Loader.PlayMusic(BackgroundMusic.Default, true);
        }

        /// <summary>
        /// Resets the current level by reloading it.
        /// </summary>
        /// <param name="message">Optional message parameter.</param>
        public void ResetCurrentLevel(ReloadLevelMessage message = null)
        {
            _levelNeedsChanging = true; 
        }

        /// <summary>
        /// Advances to the next level.
        /// </summary>
        /// <param name="message">Optional message parameter.</param>
        public void NextLevel(NextLevelMessage message = null)
        {
            _currentLevel = (LevelID)(((int)_currentLevel + 1 + _totalLevels) % _totalLevels);
            _levelNeedsChanging = true;
        }

        /// <summary>
        /// Goes back to the previous level.
        /// </summary>
        public void PreviousLevel(PreviousLevelMessage message = null)
        {
            _currentLevel = (LevelID)(((int)_currentLevel - 1 + _totalLevels) % _totalLevels);
            _levelNeedsChanging = true;
        }

        /// <summary>
        /// Handles addition of entities to systems.
        /// </summary>
        /// <param name="message">The message containing an entity to add.</param>
        private void OnCreateEntity(AddEntityMessage message) // Add this method
        {
            _entitiesToAdd.Enqueue(message.Entity);
        }

        /// <summary>
        /// Handles the destruction of entities.
        /// </summary>
        /// <param name="message">The message containing the entity to destroy.</param>
        private void OnDestroyEntity(DestroyEntityMessage message)
        {
            _entitiesToDestroy.Enqueue(message.Entity);
        }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if(_levelNeedsChanging)
            {
                LoadLevel();
                _levelNeedsChanging = false;
            }

            while (_entitiesToAdd.Count > 0)
            {
                Entity entity = _entitiesToAdd.Dequeue();
                _systems.Add(entity);
            }

            _systems.Update(gameTime);

            while (_entitiesToDestroy.Count > 0)
            {
                Entity entity = _entitiesToDestroy.Dequeue();
                _systems.Remove(entity);
            }
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            _systems.Draw(spriteBatch);
        }
    }
}
