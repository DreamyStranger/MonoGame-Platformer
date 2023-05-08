using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System;

namespace ECS_Framework
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

        //Debug box
        public static Texture2D collisionBox;

        /// <summary>
        /// Loads game assets into memory.
        /// </summary>
        /// <param name="content">The ContentManager to load assets with.</param>
        public static void LoadContent(ContentManager content)
        {
            // Player
            textures.Add("player_idle", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Idle")));
            textures.Add("player_walking", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Walking")));
            textures.Add("player_jump", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Jump")));
            textures.Add("player_double_jump", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Double Jump")));
            textures.Add("player_fall", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Fall")));
            textures.Add("player_slide", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Wall Jump")));
            textures.Add("player_death", content.Load<Texture2D>(Path.Combine("Player", "Frog", "Hit")));

            //Collictable Items
            textures.Add("apple_idle", content.Load<Texture2D>(Path.Combine("Items", "Fruits", "Apple")));

            //Collectable Collected
            textures.Add("fruits_death", content.Load<Texture2D>(Path.Combine("Items", "Fruits", "Collected")));

            // Background
            textures.Add("bg_green", content.Load<Texture2D>(Path.Combine("Background", "BG_Green")));
            textures.Add("bg_yellow", content.Load<Texture2D>(Path.Combine("Background", "BG_Yellow")));

            //Tilesets textures, make sure the key will be the same as respective tsx file's name
            textures.Add("Terrain", content.Load<Texture2D>(Path.Combine("TiledMap", "Textures", "Terrain")));
            textures.Add("UI", content.Load<Texture2D>(Path.Combine("TiledMap", "Textures", "UI")));
            // Add more tilesets here

            //Load TiledMaps
            tiledHandler = new TileHandler(content);
            foreach (LevelID level in LevelID.GetValues(typeof(LevelID)))
            {
                string levelName = level.ToString();
                tiledHandler.Load(
                    Path.Combine(content.RootDirectory, "TiledMap", "Levels", $"{levelName}.tmx"),
                    Path.Combine(content.RootDirectory, "TiledMap", "Levels", " "),
                    levelName
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
    }
}

