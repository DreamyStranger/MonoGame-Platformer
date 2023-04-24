using System.Collections.Generic;
using System;

namespace ECS_Framework
{
    /// <summary>
    /// Represents the game world and manages loading and updating levels.
    /// </summary>
    public class World
    {
        private LevelManager levelManager;
        private Level currentLevel;
        private SystemManager systems;
        private int totalLevels = Enum.GetValues(typeof(LevelID)).Length;

        private List<Entity> entitiesToDestroy;

        public World()
        {
            levelManager = new LevelManager();
            CurrentLevel = levelManager.GetLevel(LevelID.Level1);
            systems = new SystemManager(CurrentLevel.Id);
            LoadLevel(CurrentLevel);
            MessageBus.Subscribe<DestroyEntityMessage>(OnDestroyEntity);
            MessageBus.Subscribe<NextLevelMessage>(NextLevel);
            entitiesToDestroy = new List<Entity>();
        }

        /// <summary>
        /// Gets or sets the current level.
        /// </summary>
        public Level CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                currentLevel = value;
                LoadLevel(currentLevel);
            }
        }

        /// <summary>
        /// Loads the given level and its entities into the game world.
        /// </summary>
        private void LoadLevel(Level level)
        {

            systems = new SystemManager(level.Id);
            List<Entity> objects = level.Initializer.GetObjects();
            foreach (Entity entity in objects)
            {
                systems.Add(entity);
            }
        }

        /// <summary>
        /// Resets the current level by reloading it.
        /// </summary>
        public void ResetCurrentLevel()
        {
            LoadLevel(CurrentLevel);
        }

        /// <summary>
        /// Advances to the next level.
        /// </summary>
        public void NextLevel(NextLevelMessage message = null)
        {
            LevelID nextLevelID = GetNextLevelId(CurrentLevel.Id);
            CurrentLevel = levelManager.GetLevel(nextLevelID);
        }

        /// <summary>
        /// Goes back to the previous level.
        /// </summary>
        public void PreviousLevel()
        {
            LevelID previousLevelID = GetPreviousLevelId(CurrentLevel.Id);
            CurrentLevel = levelManager.GetLevel(previousLevelID);
        }

        /// <summary>
        /// Gets the ID of the next level given the current level ID.
        /// </summary>
        private LevelID GetNextLevelId(LevelID currentLevelID)
        {
            int currentLevelIndex = (int)currentLevelID;
            int nextLevelIndex = (currentLevelIndex + 1) % totalLevels;
            return (LevelID)nextLevelIndex;
        }

        /// <summary>
        /// Gets the ID of the previous level given the current level ID.
        /// </summary>
        private LevelID GetPreviousLevelId(LevelID currentLevelID)
        {
            int currentLevelIndex = (int)currentLevelID;
            int previousLevelIndex = (currentLevelIndex - 1 + totalLevels) % totalLevels;
            return (LevelID)previousLevelIndex;
        }


        private void OnDestroyEntity(DestroyEntityMessage message)
        {
            // Remove the entity from all systems
            entitiesToDestroy.Add(message.Entity);
        }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            systems.Update(gameTime);
            foreach (Entity entity in entitiesToDestroy)
            {
                systems.Remove(entity);
            }
            entitiesToDestroy.Clear();
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            systems.Draw(spriteBatch);
        }
    }
}
