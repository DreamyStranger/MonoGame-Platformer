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
        private LevelID currentLevel;
        private SystemManager systems;
        private int totalLevels = Enum.GetValues(typeof(LevelID)).Length;

        private readonly Queue<Entity> entitiesToDestroy;

        /// <summary>
        /// Initializes a new instance of the World class.
        /// </summary>
        public World()
        {
            systems = new SystemManager(CurrentLevel);
            entitiesToDestroy = new Queue<Entity>();

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
            get { return currentLevel; }
        }

        /// <summary>
        /// Changes the current level to the specified level.
        /// </summary>
        /// <param name="level">The level to change to.</param>
        private void ChangeLevel(LevelID level)
        {
            currentLevel = level;
            LoadLevel();
        }

        /// <summary>
        /// Loads the current level and its entities into the game world.
        /// </summary>
        private void LoadLevel()
        {
            MediaPlayer.Stop();
            systems.Unsubscribe();
            systems.ResetSystems(CurrentLevel);
            systems.Subscribe();
            List<Entity> objects = LevelLoader.GetObjects(CurrentLevel);
            foreach (Entity entity in objects)
            {
                systems.Add(entity);
            }
            Loader.PlayMusic(BackgroundMusic.Default, true);
        }

        /// <summary>
        /// Resets the current level by reloading it.
        /// </summary>
        /// <param name="message">Optional message parameter.</param>
        public void ResetCurrentLevel(ReloadLevelMessage message = null)
        {
            LoadLevel();
        }

        /// <summary>
        /// Advances to the next level.
        /// </summary>
        /// <param name="message">Optional message parameter.</param>
        public void NextLevel(NextLevelMessage message = null)
        {
            currentLevel = (LevelID)(((int)currentLevel + 1 + totalLevels) % totalLevels);
            LoadLevel();
        }

        /// <summary>
        /// Goes back to the previous level.
        /// </summary>
        public void PreviousLevel(PreviousLevelMessage message = null)
        {
            currentLevel = (LevelID)(((int)currentLevel - 1 + totalLevels) % totalLevels);
            LoadLevel();
        }

        /// <summary>
        /// Handles the destruction of entities.
        /// </summary>
        /// <param name="message">The message containing the entity to destroy.</param>
        private void OnDestroyEntity(DestroyEntityMessage message)
        {
            entitiesToDestroy.Enqueue(message.Entity);
        }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            systems.Update(gameTime);
            while (entitiesToDestroy.Count > 0)
            {
                Entity entity = entitiesToDestroy.Dequeue();
                systems.Remove(entity);
            }
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            systems.Draw(spriteBatch);
        }
    }
}
