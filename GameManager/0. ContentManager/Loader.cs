using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace MyGame
{
    /// <summary>
    /// Handles loading and retrieval of game assets, including textures and tile maps.
    /// </summary>
    public class Loader
    {
        // Textures
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // TiledMap
        public static TileHandler tiledHandler { get; private set; }
        public static Dictionary<string, List<Rectangle>> obstacles { get; private set; }
        private static Dictionary<string, List<LevelID>> terrainToLevel = new Dictionary<string, List<LevelID>>();

        //Debug box
        public static Texture2D collisionBox;

        // <summary>
        /// Loads game assets into memory.
        /// </summary>
        /// <param name="content">The ContentManager to load assets with.</param>
        public static void LoadContent(ContentManager content)
        {
            // Player
            textures.Add("player_idle", content.Load<Texture2D>("Player/Frog/Idle"));
            textures.Add("player_walking", content.Load<Texture2D>("Player/Frog/Walking"));
            textures.Add("player_jump", content.Load<Texture2D>("Player/Frog/Jump"));
            textures.Add("player_double_jump", content.Load<Texture2D>("Player/Frog/Double Jump"));
            textures.Add("player_fall", content.Load<Texture2D>("Player/Frog/Fall"));
            textures.Add("player_slide", content.Load<Texture2D>("Player/Frog/Wall Jump"));

            // Background
            textures.Add("background_green", content.Load<Texture2D>("Background/BG_Green"));
            
            // Add more terrain types and their corresponding levels here

            textures.Add("Terrain", content.Load<Texture2D>("TiledMap/Terrain"));
            // Tilesets
            AddTerrain("Terrain", LevelID.Level1);

            //Load TiledMaps
            tiledHandler = new TileHandler(content);
            foreach (LevelID level in LevelID.GetValues(typeof(LevelID)))
            {
                string levelName = level.ToString();
                tiledHandler.Load(
                    Path.Combine(content.RootDirectory, "TiledMap", $"{levelName}.tmx"),
                    Path.Combine(content.RootDirectory, "TiledMap", " "),
                    levelName,
                    GetTerrain(level)
                );

                // Save collision boxes for each level
                tiledHandler.GetLayersBoundsInMap();
            }

            //Box to debug Collisions
            GraphicsDevice graphicsDevice = ((IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;
            collisionBox = new Texture2D(graphicsDevice, 1, 1);
            collisionBox.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Retrieves a loaded texture by name.
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

        private static void AddTerrain(string terrain, LevelID levelID)
        {
            if (!terrainToLevel.ContainsKey(terrain))
            {
                terrainToLevel[terrain] = new List<LevelID>();
            }
            terrainToLevel[terrain].Add(levelID);
        }

        private static string GetTerrain(LevelID levelID)
        {
            foreach (var key in terrainToLevel.Keys)
            {
                foreach (var value in terrainToLevel[key])
                {
                    if (value == levelID)
                    {
                        return key;
                    }
                }
            }
            return null;
        }
    }
}