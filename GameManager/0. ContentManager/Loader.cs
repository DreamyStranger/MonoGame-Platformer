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
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // Music
        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();

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
            //Load music
            AddMusic("bg_music", content, "Audio", "Background", "FuriousFreak");
            // ... add more music here

            // Load textures
            //Player
            AddTexture("player_idle", content, "Player", "Frog", "Idle");
            AddTexture("player_walking", content, "Player", "Frog", "Walking");
            AddTexture("player_jump", content, "Player", "Frog", "Jump");
            AddTexture("player_double_jump", content, "Player", "Frog", "Double Jump");
            AddTexture("player_fall", content, "Player", "Frog", "Fall");
            AddTexture("player_slide", content, "Player", "Frog", "Wall Jump");
            AddTexture("player_death", content, "Player", "Frog", "Hit");

            //Enemy
            AddTexture("voodo_idle", content, "Enemies", "Voodo", "Idle");
            AddTexture("voodo_walking", content, "Enemies", "Voodo", "Run");
            AddTexture("voodo_jump", content, "Enemies", "Voodo", "Jump");
            AddTexture("voodo_double_jump", content, "Enemies", "Voodo", "Double Jump");
            AddTexture("voodo_fall", content, "Enemies", "Voodo", "Fall");
            AddTexture("voodo_slide", content, "Enemies", "Voodo", "Wall Jump");
            AddTexture("voodo_death", content, "Enemies", "Voodo", "Hit");

            //Collectables
            AddTexture("apple_idle", content, "Items", "Fruits", "Apple");

            //Collectable collected
            AddTexture("fruits_death", content, "Items", "Fruits", "Collected");

            //Background tile for parallax
            AddTexture("bg_green", content, "Background", "BG_Green");
            AddTexture("bg_yellow", content, "Background", "BG_Yellow");
            // ... add more textures here

            //Load TiledMaps
            //Tilesets' textures, key should be the same as respective tilesets's name
            AddTexture("Terrain", content, "TiledMap", "Textures", "Terrain");
            AddTexture("UI", content, "TiledMap", "Textures", "UI");

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
                tiledHandler.GetLayersObjectsInMap();
            }

            //Box to debug Collisions
            GraphicsDevice graphicsDevice = ((IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;
            collisionBox = new Texture2D(graphicsDevice, 1, 1);
            collisionBox.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Adds a texture to the textures dictionary.
        /// </summary>
        /// <param name="textureName">The name of the texture, used as the key in the dictionary.</param>
        /// <param name="content">The ContentManager to load assets with.</param>
        /// <param name="pathParts">An array of strings representing the path parts to the texture file. These will be combined using Path.Combine.</param>
        private static void AddTexture(string textureName, ContentManager content, params string[] pathParts)
        {
            string path = Path.Combine(pathParts);
            textures.Add(textureName, content.Load<Texture2D>(path));
        }

        /// <summary>
        /// Adds a music file to the songs dictionary.
        /// </summary>
        /// <param name="songName">The name of the music file, used as the key in the dictionary.</param>
        /// <param name="content">The ContentManager to load assets with.</param>
        /// <param name="pathParts">An array of strings representing the path parts to the music file. These will be combined using Path.Combine.</param>
        private static void AddMusic(string songName, ContentManager content, params string[] pathParts)
        {
            string path = Path.Combine(pathParts);
            songs.Add(songName, content.Load<Song>(path));
        }

        /// <summary>
        /// Retrieves a texture given its name.
        /// </summary>
        /// <param name="textureName">The name of the texture to retrieve.</param>
        /// <returns>The loaded texture, or null if the texture was not found.</returns>
        public static Texture2D GetTexture(string textureName)
        {
            if (textures.ContainsKey(textureName))
            {
                return textures[textureName];
            }

            return null;
        }

        /// <summary>
        /// Retrieves a music file given its name.
        /// </summary>
        /// <param name="songName">The name of the music file to retrieve.</param>
        /// <returns>The loaded music, or null if the music file was not found.</returns>
        public static Song GetMusic(string songName)
        {
            if (songs.ContainsKey(songName))
            {
                return songs[songName];
            }

            return null;
        }

        // <summary>
        /// Plays a music file given its name.
        /// </summary>
        /// <param name="musicName">The name of the music file to play.</param>
        /// <param name="loop">If set to true, the music will loop. Defaults to false.</param>
        public static void PlayMusic(string musicName, bool loop = false)
        {
            if (songs.ContainsKey(musicName))
            {
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(songs[musicName]);
            }
        }
    }
}
