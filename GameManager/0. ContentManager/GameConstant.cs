using System.Reflection;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Holds common game constants
    /// </summary>
    public static class GameConstants
    {
        // Player-related constants
        public static int PLAYER_MAX_HP = 3;

        // Entity-related constant
        public static float GRAVITY = 2000f;
        public static int OTHER_HP = 1;
        public static float SpeedY = -500f;
        public static float SpeedX = 100f;
        public static float SpeedXonCollision = -50f;
        public static float SpeedYonCollision = 300f;

        // Obstacles
        public static HashSet<string> OBSTACLES = new HashSet<string>()
        {
            "solid",
            "float"
        };

        // Entities
        public static HashSet<string> ENTITIES = new HashSet<string>()
        {
            "entity"
        };

        // Other game constants
        public static int SCREEN_WIDTH = 640;
        public static int SCREEN_HEIGHT = 368;
        public static bool FULL_SCREEN = false;
        public static float AnimationFPS = 20f;
        public static float PhysicFPS = 60f;

        // Keyboard
        public const Keys LEFT_KEY = Keys.A;
        public const Keys RIGHT_KEY = Keys.D;
        public const Keys JUMP_KEY = Keys.Space;

        //Debug
        public static bool DisplayCollisionBoxes = false;
        public static bool AnimationDebugMessages = false;
        public static bool EntityDebugMessages = false;

        /// <summary>
        /// Updates the value of a game constant field.
        /// </summary>
        /// <param name="fieldName">The name of the field to update.</param>
        /// <param name="value">The new value of the field.</param>
        public static void UpdateConstant(string fieldName, object value)
        {
            FieldInfo field = typeof(GameConstants).GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
            if (field != null)
            {
                field.SetValue(null, value);
            }
        }
    }
}
