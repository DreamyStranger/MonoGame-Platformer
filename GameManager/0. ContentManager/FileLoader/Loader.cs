using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;
using System;
using SpriteFontPlus;

namespace MonogameExamples
{
     /// <summary>
    /// Handles loading and retrieval of game assets, including textures, fonts, tile maps, and audio.
    /// </summary>
    public class Loader
    {
        // Dictionaries to hold textures, fonts, and audio assets.
        private static Dictionary<Enum, Texture2D> textures = new Dictionary<Enum, Texture2D>();
        private static Dictionary<Enum, SoundEffect> songs = new Dictionary<Enum, SoundEffect>();
        private static Dictionary<Enum, SoundEffectInstance> soundEffectInstances = new Dictionary<Enum, SoundEffectInstance>();
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        // Property to manage tiled maps.
        public static TileHandler tiledHandler { get; private set; }

        // Texture for debugging collisions.
        public static Texture2D collisionBox;

        /// <summary>
        /// Loads game assets such as textures, fonts, and audio into memory.
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice to load assets with.</param>
        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            // Load fonts
            AddFont("GameFont", 16, graphicsDevice,  "Fonts", "DejaVuSerif");

            //Load music
            AddMusic(BackgroundMusic.Default, graphicsDevice, "Audio", "Background", "FuriousFreak");
            // ... add more music here

            // Load textures
            //Player
            AddTexture(PlayerTexture.Idle, graphicsDevice, "Player", "Frog", "Idle");
            AddTexture(PlayerTexture.Walking, graphicsDevice, "Player", "Frog", "Walking");
            AddTexture(PlayerTexture.Jump, graphicsDevice, "Player", "Frog", "Jump");
            AddTexture(PlayerTexture.DoubleJump, graphicsDevice, "Player", "Frog", "Double Jump");
            AddTexture(PlayerTexture.Fall, graphicsDevice, "Player", "Frog", "Fall");
            AddTexture(PlayerTexture.Slide, graphicsDevice, "Player", "Frog", "Wall Jump");
            AddTexture(PlayerTexture.Hit, graphicsDevice, "Player", "Frog", "Hit");

            //Enemy
            AddTexture(MaskedEnemyTexture.Idle, graphicsDevice, "Enemies", "Voodo", "Idle");
            AddTexture(MaskedEnemyTexture.Walking, graphicsDevice, "Enemies", "Voodo", "Run");
            AddTexture(MaskedEnemyTexture.Jump, graphicsDevice, "Enemies", "Voodo", "Jump");
            AddTexture(MaskedEnemyTexture.DoubleJump, graphicsDevice, "Enemies", "Voodo", "Double Jump");
            AddTexture(MaskedEnemyTexture.Fall, graphicsDevice, "Enemies", "Voodo", "Fall");
            AddTexture(MaskedEnemyTexture.Slide, graphicsDevice, "Enemies", "Voodo", "Wall Jump");
            AddTexture(MaskedEnemyTexture.Hit, graphicsDevice, "Enemies", "Voodo", "Hit");

            //Collectables
            AddTexture(FruitTexture.Apple, graphicsDevice, "Items", "Fruits", "Apple");
            AddTexture(FruitTexture.Orange, graphicsDevice, "Items", "Fruits", "Orange");
            AddTexture(FruitTexture.Collected, graphicsDevice, "Items", "Fruits", "Collected");

            //Background tile for parallax
            AddTexture(BackgroundTexture.Green, graphicsDevice, "Background", "BG_Green");
            AddTexture(BackgroundTexture.Yellow, graphicsDevice, "Background", "BG_Yellow");

            // ... add more textures here

            //Load TiledMaps
            //Tilesets' textures, key should be the same as respective tilesets's name
            AddTexture(TiledTexture.Terrain, graphicsDevice, "TiledMap", "Textures", "Terrain");
            AddTexture(TiledTexture.UI, graphicsDevice, "TiledMap", "Textures", "UI");

            //TileMaps
            tiledHandler = new TileHandler();
            foreach (LevelID level in LevelID.GetValues(typeof(LevelID)))
            {
                string levelName = level.ToString();
                tiledHandler.Load(
                    Path.Combine("Content", "TiledMap", "Levels", $"{levelName}.tmx"),
                    Path.Combine("Content", "TiledMap", "Levels", " "),
                    levelName
                );

                // Save collision boxes for each level
                tiledHandler.GetLayersObstaclesInMap();
            }

            //Box to debug Collisions
            collisionBox = new Texture2D(graphicsDevice, 1, 1);
            collisionBox.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Loads a font from a .ttf file and stores it in a dictionary.
        /// </summary>
        /// <param name="fontKey">The key to associate with the loaded font.</param>
        /// <param name="fontSize">Size of the font to generate.</param>
        /// <param name="graphicsDevice">The GraphicsDevice used to create the SpriteFont.</param>
        /// <param name="pathParts">Path elements leading to the .ttf file.</param>
        private static void AddFont(string fontKey, int fontSize, GraphicsDevice graphicsDevice, params string[] pathParts)
        {
            string path = Path.Combine(pathParts);
            path = Path.Combine("Content", path);
            path += ".ttf";
            using (var fileStream = File.OpenRead(path))
            {
                var ttfFontBakerResult = TtfFontBaker.Bake(fileStream,
                                                        fontSize,
                                                        1024,
                                                        1024,
                                                        new[] { CharacterRange.BasicLatin });

                var ttfFont = ttfFontBakerResult.CreateSpriteFont(graphicsDevice);

                if (ttfFont != null)
                {
                    fonts[fontKey] = ttfFont;
                }
            }
        }

        /// <summary>
        /// Loads a texture from a file and stores it in a dictionary.
        /// </summary>
        /// <param name="textureKey">The key to associate with the loaded texture.</param>
        /// <param name="graphicsDevice">The GraphicsDevice used to load the texture.</param>
        /// <param name="pathParts">Path elements leading to the texture file.</param>
        private static void AddTexture<T>(T textureKey, GraphicsDevice graphicsDevice, params string[] pathParts) where T : Enum
        {
            string path = Path.Combine(pathParts);
            path = Path.Combine("Content", path);
            path += ".png";

            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
                    if (texture != null)
                    {
                        textures[textureKey] = texture;
                    }
                }
            }
            else
            {
                // Log or handle the error that the file does not exist.
                Console.WriteLine($"Error: The file at path {path} does not exist.");
            }
        }

        /// <summary>
        /// Loads a music file and stores it as a SoundEffect in a dictionary.
        /// </summary>
        /// <param name="musicKey">The key to associate with the loaded music.</param>
        /// <param name="graphicsDevice">The GraphicsDevice used to load the music.</param>
        /// <param name="pathParts">Path elements leading to the music file.</param>
        private static void AddMusic<T>(T musicKey, GraphicsDevice graphicsDevice, params string[] pathParts) where T : Enum
        {
            string path = Path.Combine(pathParts);
            path = Path.Combine("Content", path);
            path += ".wav";

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                SoundEffect soundEffect = SoundEffect.FromStream(fileStream);
                songs.Add(musicKey, soundEffect);
            }
        }


        /// <summary>
        /// Retrieves a texture associated with a specific key.
        /// </summary>
        /// <param name="textureKey">The key of the texture to retrieve.</param>
        /// <returns>The retrieved Texture2D object, or null if not found.</returns>
        public static Texture2D GetTexture<T>(T textureKey) where T : Enum
        {
            if (textures.ContainsKey(textureKey))
            {
                return textures[textureKey];
            }
            return null;
        }

        /// <summary>
        /// Retrieves a font associated with a specific key.
        /// </summary>
        /// <param name="fontKey">The key of the font to retrieve.</param>
        /// <returns>The retrieved SpriteFont object, or null if not found.</returns>
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
        public static SoundEffect GetMusic(Enum musicKey)
        {
            if (songs.ContainsKey(musicKey))
            {
                return songs[musicKey];
            }

            return null;
        }


        /// <summary>
        /// Retrieves and plays a SoundEffect associated with a specific key.
        /// </summary>
        /// <param name="musicKey">The key of the music to play.</param>
        /// <param name="loop">Specifies whether the music should loop.</param>
        public static void PlayMusic(Enum musicKey, bool loop = false)
        {
            // Stop any previously playing instance of the sound
            if (soundEffectInstances.ContainsKey(musicKey) && soundEffectInstances[musicKey].State == SoundState.Playing)
            {
                soundEffectInstances[musicKey].Stop();
            }
            
            if (songs.ContainsKey(musicKey))
            {
                SoundEffectInstance soundEffectInstance = songs[musicKey].CreateInstance();
                soundEffectInstance.IsLooped = loop;
                soundEffectInstance.Play();
                
                // Store the instance so it can be managed later
                soundEffectInstances[musicKey] = soundEffectInstance;
            }
        }
    }
}
