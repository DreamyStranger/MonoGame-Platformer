using System.Reflection;

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


        // Other game constants
        public static int SCREEN_WIDTH = 640;
        public static int SCREEN_HEIGHT = 368;
        public static float AnimationFPS = 20f;
        public static float FPS = 60f;

        //Debug
        public static bool DisplayCollisionBoxes = false;
        public static bool AnimationDebugMessages = false;

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
