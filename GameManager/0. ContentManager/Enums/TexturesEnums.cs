namespace MonogameExamples
{
    /// <summary>
    /// Enumeration representing the different states of player animation textures.
    /// </summary>
    public enum PlayerTexture
    {
        Idle,
        Walking,
        Jump,
        DoubleJump,
        Fall,
        Slide,
        Hit
    }

    /// <summary>
    /// Enumeration representing the different states of masked enemy animation textures.
    /// </summary>
    public enum MaskedEnemyTexture
    {
        Idle,
        Walking,
        Jump,
        DoubleJump,
        Fall,
        Slide,
        Hit
    }

    /// <summary>
    /// Enumeration representing the different types of background textures.
    /// </summary>
    public enum BackgroundTexture
    {
        Green,
        Yellow
    }

    /// <summary>
    /// Enumeration representing the different types of fruit textures.
    /// </summary>
    public enum FruitTexture
    {
        Apple,
        Orange,
        Collected
    }

    /// <summary>
    /// Enumeration representing the different types of Tiled textures.
    /// </summary>
    public enum TiledTexture
    {
        /// <summary>
        /// Terrain Tiled texture.
        /// </summary>
        Terrain,

        /// <summary>
        /// UI Tiled texture.
        /// </summary>
        UI,
    }
}
