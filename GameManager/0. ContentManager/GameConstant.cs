namespace ECS_Framework
{
    /// <summary>
    /// Holds common game constants
    /// </summary>
    public static class GameConstants
    {
        // Player-related constants
        public const int PLAYER_MAX_HP = 3;

        // Entity-related constant
        public const float GRAVITY = 2000f;
        public const int OTHER_HP = 1;
        public const float SpeedY = -500f;
        public const float SpeedX = 100f;

        // Other game constants
        public const int SCREEN_WIDTH = 640;
        public const int SCREEN_HEIGHT = 368;
        public const float AnimationFPS = 20f;
        public const float FPS = 60f;

        //Debug
        public const bool DisplayCollisionBoxes = false;
        public const bool AnimationDebugMessages = false;
    }
}
