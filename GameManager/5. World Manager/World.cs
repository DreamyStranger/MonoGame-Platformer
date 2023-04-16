using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyGame
{
    public class World
    {
        private LevelManager levelManager;
        private Level currentLevel;
        private SystemManager systems;
        private int totalLevels = Enum.GetValues(typeof(LevelID)).Length;

        public World()
        {
            levelManager = new LevelManager();
            CurrentLevel = levelManager.GetLevel(LevelID.Level1);
            systems = new SystemManager(CurrentLevel.Id.ToString());
            LoadLevel(CurrentLevel);
        }

        public Level CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                currentLevel = value;
                LoadLevel(currentLevel);
            }
        }

        private void LoadLevel(Level level)
        {
            systems = new SystemManager(level.Id.ToString());
            List<Entity> objects = level.Initializer.GetObjects();
            foreach (Entity entity in objects)
            {
                systems.Add(entity);
            }
        }

        public void ResetCurrentLevel()
        {
            LoadLevel(CurrentLevel);
        }

        public void NextLevel()
        {
            LevelID nextLevelID = GetNextLevelId(CurrentLevel.Id);
            CurrentLevel = levelManager.GetLevel(nextLevelID);
            LoadLevel(CurrentLevel);
        }

        public void PreviousLevel()
        {
            LevelID previousLevelID = GetPreviousLevelId(CurrentLevel.Id);
            CurrentLevel = levelManager.GetLevel(previousLevelID);
            LoadLevel(CurrentLevel);
        }

        private LevelID GetNextLevelId(LevelID currentLevelID)
        {
            int currentLevelIndex = (int)currentLevelID;
            int nextLevelIndex = (currentLevelIndex + 1) % totalLevels;
            return (LevelID)nextLevelIndex;
        }

        private LevelID GetPreviousLevelId(LevelID currentLevelID)
        {
            int currentLevelIndex = (int)currentLevelID;
            int previousLevelIndex = (currentLevelIndex - 1 + totalLevels) % totalLevels;
            return (LevelID)previousLevelIndex;
        }

        public void Update(GameTime gameTime)
        {
            systems.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            systems.Draw(spriteBatch);
        }
    }
}
