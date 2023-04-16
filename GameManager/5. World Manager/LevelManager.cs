using System.Collections.Generic;

namespace MyGame
{
    public class LevelManager
    {
        private Dictionary<LevelID, Level> levels;

        public LevelManager()
        {
            levels = new Dictionary<LevelID, Level>();
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            levels.Add(LevelID.Level1, new Level(LevelID.Level1, new Level1Initializer()));
            
            // Add more levels here
        }

        public Level GetLevel(LevelID levelID)
        {
            return levels[levelID];
        }
    }
}