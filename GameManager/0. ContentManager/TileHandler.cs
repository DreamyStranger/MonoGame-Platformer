using System.Collections.Generic;
using TiledCS;

namespace ECS_Framework
{
    /// <summary>
    /// Handles loading and rendering of tile maps.
    /// </summary>
    public class TileHandler
    {
        private Dictionary<string, Texture2D> tilesetTextures;
        private Dictionary<string, TiledMap> tiledMaps;
        private Dictionary<string, Dictionary<int, TiledTileset>> tileSets;

        /// <summary>
        /// A dictionary of all obstacles in every loaded map.
        /// </summary>
        public Dictionary<string, Dictionary<string, List<Rectangle>>> obstacles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TileHandler class.
        /// </summary>
        /// <param name="contentManager">The ContentManager to load tile textures with.</param>
        public TileHandler(ContentManager contentManager)
        {
            this.tilesetTextures = new Dictionary<string, Texture2D>();
            this.tiledMaps = new Dictionary<string, TiledMap>();
            this.tileSets = new Dictionary<string, Dictionary<int, TiledTileset>>();
            this.obstacles = new Dictionary<string, Dictionary<string, List<Rectangle>>>();
        }

        /// <summary>
        /// Loads a map and its associated tileset texture.
        /// </summary>
        /// <param name="pathToMap">The path to the Tiled map file.</param>
        /// <param name="pathToFolder">The path to the folder containing the tileset texture.</param>
        /// <param name="levelID">The name to give the loaded map.</param>
        /// <param name="textureName">The name to give the loaded tileset texture.</param>
        public void Load(string pathToMap, string pathToFolder, string levelID, string textureName)
        {
            // Load the tileset using TiledCS
            TiledMap map = new TiledMap(pathToMap);
            var tilesets = map.GetTiledTilesets(pathToFolder);

            // Load the tileset texture using MonoGame
            Texture2D tilesetTexture = Loader.GetTexture(textureName);

            // Update dictionaries
            tilesetTextures[levelID] = tilesetTexture;
            tiledMaps[levelID] = map;
            tileSets[levelID] = tilesets;
        }

        /// <summary>
        /// Helper method for the All Bounds method below. Creates rectangles representing the bounds of a specified layer.
        /// </summary>
        /// <param name="layer">The layer to create bounds for.</param>
        /// <param name="mapName">The name of the map the layer belongs to.</param>
        /// <returns>A list of rectangles representing the layer's bounds.</returns>
        public List<Rectangle> GetLayerBounds(TiledLayer layer, string mapName)
        {
            List<Rectangle> layerBounds = new List<Rectangle>();
            TiledMap map = tiledMaps[mapName];

            foreach (var obj in layer.objects)
            {
                int objX = (int)obj.x;
                int objY = (int)obj.y;
                int objWidth = (int)obj.width;
                int objHeight = (int)obj.height;
                layerBounds.Add(new Rectangle(objX, objY, objWidth, objHeight));
            }

            return layerBounds;
        }

        /// <summary>
        /// Creates rectangles that bound every Object Layer in every loaded Level. Useful for collision detection with Rectangle.Intersects() method.
        /// </summary>
        public void GetLayersBoundsInMap()
        {
            foreach (string mapName in tiledMaps.Keys)
            {
                Dictionary<string, List<Rectangle>> layerBoundsMap = new Dictionary<string, List<Rectangle>>();
                foreach (var layer in tiledMaps[mapName].Layers)
                {
                    string layerName = layer.name;
                    if (layer.type != TiledLayerType.ObjectLayer)
                    {
                        continue;
                    }
                    List<Rectangle> layerBounds = GetLayerBounds(layer, mapName);
                    if (!layerBoundsMap.ContainsKey(layerName))
                    {
                        layerBoundsMap[layerName] = new List<Rectangle>();
                    }
                    layerBoundsMap[layerName].AddRange(layerBounds);
                }
                obstacles[mapName] = layerBoundsMap;
            }
        }

        /// <summary>
        /// Draws every tile in the "TileLayer" type layers of the specified map.
        /// </summary>
        /// <param name="LevelID">The name of the map to draw.</param>
        /// <param name="spriteBatch">The SpriteBatch object to use for rendering.</param>
        public void Draw(string LevelID, SpriteBatch spriteBatch)
        {
            TiledMap map = tiledMaps[LevelID];
            Texture2D tilesetTexture = tilesetTextures[LevelID];
            var tilesets = tileSets[LevelID];

            foreach (var layer in map.Layers)
            {
                if (layer.type == TiledLayerType.TileLayer)
                {
                    for (int y = 0; y < layer.height; y++)
                    {
                        for (int x = 0; x < layer.width; x++)
                        {
                            // Assuming the default render order is used which is from right to bottom
                            var index = (y * layer.width) + x;
                            var gid = layer.data[index];
                            var tileX = x * map.TileWidth;
                            var tileY = y * map.TileHeight;
                            // Gid 0 is used to tell there is no tile set
                            if (gid == 0)
                            {
                                continue;
                            }

                            //TiledCS methods to retrieve needed data
                            var mapTileset = map.GetTiledMapTileset(gid);
                            var tileset = tilesets[mapTileset.firstgid];
                            var rect = map.GetSourceRect(mapTileset, tileset, gid);

                            // Create destination and source rectangles
                            var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                            var destination = new Rectangle(tileX, tileY, map.TileWidth, map.TileHeight);

                            spriteBatch.Draw(tilesetTexture, destination, source, Color.White);
                        }
                    }
                }
            }
        }
    }
}
