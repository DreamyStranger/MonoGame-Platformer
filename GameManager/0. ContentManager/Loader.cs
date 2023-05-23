using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// Handles loading and retrieval of game assets, including textures, tile maps, and music.
    /// </summary>
    public class Loader
    {
        // Textures
        private static Dictionary<Enum, Texture2D> textures = new Dictionary<Enum, Texture2D>();

        // Music
        private static Dictionary<Enum, Song> songs = new Dictionary<Enum, Song>();
        // Fonts
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();


        // TiledMap
        public static TileHandler tiledHandler { get; private set; }

        // Debug box
        public static Texture2D collisionBox;

        /// <summary>
        /// Loads game assets into memory.
        /// </summary>
        /// <param name="content">The ContentManager to load assets with.</param>
        public static void LoadContent(ContentManager content)
        {
            // Load fonts
            AddFont("GameFont", content, "Fonts", "font");

            //Load music
            AddMusic(BackgroundMusic.Default, content, "Audio", "Background", "FuriousFreak");
            // ... add more music here

            // Load textures
            //Player
            AddTexture(PlayerTexture.Idle, content, "Player", "Frog", "Idle");
            AddTexture(PlayerTexture.Walking, content, "Player", "Frog", "Walking");
            AddTexture(PlayerTexture.Jump, content, "Player", "Frog", "Jump");
            AddTexture(PlayerTexture.DoubleJump, content, "Player", "Frog", "Double Jump");
            AddTexture(PlayerTexture.Fall, content, "Player", "Frog", "Fall");
            AddTexture(PlayerTexture.Slide, content, "Player", "Frog", "Wall Jump");
            AddTexture(PlayerTexture.Hit, content, "Player", "Frog", "Hit");

            //Enemy
            AddTexture(MaskedEnemyTexture.Idle, content, "Enemies", "Voodo", "Idle");
            AddTexture(MaskedEnemyTexture.Walking, content, "Enemies", "Voodo", "Run");
            AddTexture(MaskedEnemyTexture.Jump, content, "Enemies", "Voodo", "Jump");
            AddTexture(MaskedEnemyTexture.DoubleJump, content, "Enemies", "Voodo", "Double Jump");
            AddTexture(MaskedEnemyTexture.Fall, content, "Enemies", "Voodo", "Fall");
            AddTexture(MaskedEnemyTexture.Slide, content, "Enemies", "Voodo", "Wall Jump");
            AddTexture(MaskedEnemyTexture.Hit, content, "Enemies", "Voodo", "Hit");

            //Collectables
            AddTexture(FruitTexture.Apple, content, "Items", "Fruits", "Apple");
            AddTexture(FruitTexture.Collected, content, "Items", "Fruits", "Collected");

            //Background tile for parallax
            AddTexture(BackgroundTexture.Green, content, "Background", "BG_Green");
            AddTexture(BackgroundTexture.Yellow, content, "Background", "BG_Yellow");

            // ... add more textures here

            //Load TiledMaps
            //Tilesets' textures, key should be the same as respective tilesets's name
            AddTexture(TiledTexture.Terrain, content, "TiledMap", "Textures", "Terrain");
            AddTexture(TiledTexture.UI, content, "TiledMap", "Textures", "UI");

            //TileMaps
            tiledHandler = new TileHandler();
            foreach (LevelID level in LevelID.GetValues(typeof(LevelID)))
            {
                string levelName = level.ToString();
                tiledHandler.Load(
                    Path.Combine(content.RootDirectory, "TiledMap", "Levels", $"{levelName}.tmx"),
                    Path.Combine(content.RootDirectory, "TiledMap", "Levels", " "),
                    levelName
                );

                // Save collision boxes for each level
                tiledHandler.GetLayersObstaclesInMap();
            }

            //Box to debug Collisions
            GraphicsDevice graphicsDevice = ((IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;
            collisionBox = new Texture2D(graphicsDevice, 1, 1);
            collisionBox.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Loads a font into memory and associates it with a key in the fonts dictionary. 
        /// </summary>
        /// <param name="fontKey">The key with which the font will be associated in the dictionary.</param>
        /// <param name="content">ContentManager instance that is used for loading the font.</param>
        /// <param name="path">Path to the .spritefont file in the content project.</param>
        public static void AddFont(string fontKey, ContentManager content, params string[] pathParts)
        {
            string path = Path.Combine(pathParts);
            SpriteFont font = content.Load<SpriteFont>(path);
            if (font != null)
            {
                fonts[fontKey] = font;
            }
        }

        /// <summary>
        /// Loads a texture into memory and associates it with a key in the textures dictionary. 
        /// </summary>
        /// <param name="textureKey">The key with which the texture will be associated in the dictionary. It should be a value from an Enum.</param>
        /// <param name="content">ContentManager instance that is used for loading the texture.</param>
        /// <param name="pathParts">An array of strings that represent parts of the path to the texture file. They will be combined to form the full path.</param>
        /// <typeparam name="T">An Enum type that is used as the key in the dictionary.</typeparam>
        public static void AddTexture<T>(T textureKey, ContentManager content, params string[] pathParts) where T : Enum
        {
            string path = Path.Combine(pathParts);
            Texture2D texture = content.Load<Texture2D>(path);
            if (texture != null)
            {
                textures[textureKey] = texture;
            }
        }

        /// <summary>
        /// Adds a music file to the songs dictionary.
        /// </summary>
        /// <param name="musicKey">The name of the music file, used as the key in the dictionary.</param>
        /// <param name="content">The ContentManager to load assets with.</param>
        /// <param name="pathParts">An array of strings representing the path parts to the music file. These will be combined using Path.Combine.</param>
        private static void AddMusic<T>(T musicKey, ContentManager content, params string[] pathParts) where T : Enum
        {
            string path = Path.Combine(pathParts);
            songs.Add(musicKey, content.Load<Song>(path));
        }


        /// <summary>
        /// Retrieves a texture from the textures dictionary using the provided key. 
        /// </summary>
        /// <param name="textureKey">The key associated with the texture to retrieve. It should be a value from an Enum.</param>
        /// <returns>The Texture2D associated with the key. If the key is not found in the dictionary, the method returns null.</returns>
        /// <typeparam name="T">An Enum type that is used as the key in the dictionary.</typeparam>
        public static Texture2D GetTexture<T>(T textureKey) where T : Enum
        {
            if (textures.ContainsKey(textureKey))
            {
                return textures[textureKey];
            }
            return null;
        }

        /// <summary>
        /// Retrieves a font from the fonts dictionary using the provided key. 
        /// </summary>
        /// <param name="fontKey">The key associated with the font to retrieve.</param>
        /// <returns>The SpriteFont associated with the key. If the key is not found in the dictionary, the method returns null.</returns>
        public static SpriteFont GetFont(string fontKey)
        {
            if (fonts.ContainsKey(fontKey))
            {
                return fonts[fontKey];
            }
            return null;
        }

        /// <summary>
        /// Retrieves a music file given its name.
        /// </summary>
        /// <param name="musicKey">The name of the music file to retrieve.</param>
        /// <returns>The loaded music, or null if the music file was not found.</returns>
        public static Song GetMusic(Enum musicKey)
        {
            if (songs.ContainsKey(musicKey))
            {
                return songs[musicKey];
            }

            return null;
        }

        // <summary>
        /// Plays a music file given its name.
        /// </summary>
        /// <param name="musicKey">The name of the music file to play.</param>
        /// <param name="loop">If set to true, the music will loop. Defaults to false.</param>
        public static void PlayMusic(Enum musicKey, bool loop = false)
        {
            if (songs.ContainsKey(musicKey))
            {
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(songs[musicKey]);
            }
        }
    }
}
