namespace MonogameExamples
{
    /// <summary>
    /// Enumeration representing the different states of player animation textures.
    /// </summary>
    public enum PlayerTexture
    {
        /// <summary>
        /// Player idle state.
        /// </summary>
        Idle,

        /// <summary>
        /// Player walking state.
        /// </summary>
        Walking,

        /// <summary>
        /// Player jump state.
        /// </summary>
        Jump,

        /// <summary>
        /// Player double jump state.
        /// </summary>
        DoubleJump,

        /// <summary>
        /// Player falling state.
        /// </summary>
        Fall,

        /// <summary>
        /// Player sliding state.
        /// </summary>
        Slide,

        /// <summary>
        /// Player hit state.
        /// </summary>
        Hit
    }

    /// <summary>
    /// Enumeration representing the different states of masked enemy animation textures.
    /// </summary>
    public enum MaskedEnemyTexture
    {
        /// <summary>
        /// Enemy idle state.
        /// </summary>
        Idle,

        /// <summary>
        /// Enemy walking state.
        /// </summary>
        Walking,

        /// <summary>
        /// Enemy jump state.
        /// </summary>
        Jump,

        /// <summary>
        /// Enemy double jump state.
        /// </summary>
        DoubleJump,

        /// <summary>
        /// Enemy falling state.
        /// </summary>
        Fall,

        /// <summary>
        /// Enemy sliding state.
        /// </summary>
        Slide,

        /// <summary>
        /// Enemy hit state.
        /// </summary>
        Hit
    }

    /// <summary>
    /// Enumeration representing the different types of background textures.
    /// </summary>
    public enum BackgroundTexture
    {
        /// <summary>
        /// Green background texture.
        /// </summary>
        Green,

        /// <summary>
        /// Yellow background texture.
        /// </summary>
        Yellow
    }

    /// <summary>
    /// Enumeration representing the different types of fruit textures.
    /// </summary>
    public enum FruitTexture
    {
        /// <summary>
        /// Apple fruit texture.
        /// </summary>
        Apple,

        /// <summary>
        /// Collected fruit texture.
        /// </summary>
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
