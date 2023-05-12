using System.Collections.Generic;
using TiledCS;

namespace MonogameExamples
{
    /// <summary>
    /// Provides a static method for loading entities from a Tiled map.
    /// </summary>
    public class LevelLoader
    {
        /// <summary>
        /// Loads entities from the specified level's Tiled map.
        /// </summary>
        /// <param name="level">The level to load entities for.</param>
        /// <returns>A list of entities loaded from the level's Tiled map.</returns>
        public static void GetObjects(LevelID level)
        {
            var objectDictionary = Loader.tiledHandler.objects[level.ToString()];

            TiledMap map = Loader.tiledHandler.GetMap(level);

            foreach (var layer in map.Layers)
            {
                // Skip layers that are not entity layers
                if (!GameConstants.ENTITIES.Contains(layer.name))
                {
                    continue;
                }

                foreach (var obj in layer.objects)
                {
                    Vector2 position = new Vector2((float)obj.x, (float)obj.y);

                    switch (obj.name)
                    {
                        case "bg":
                            Background(obj);
                            break;

                        case "player":
                            EntityFactory.CreatePlayer(position);
                            break;

                        case "regularEnemy":
                            RegularEnemy(obj, position);
                            break;

                        case "fruit":
                            Fruit(obj, position);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a parallax background entity from the specified Tiled object.
        /// </summary>
        /// <param name="obj">The Tiled object representing the background.</param>
        /// <returns>A new parallax background entity.</returns>
        private static void Background(TiledObject obj)
        {
            string bg_name = "bg_yellow";
            int velocityX = 0;
            int velocityY = 0;
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "color":
                        break;

                    case "velocityX":
                        int.TryParse(property.value, out velocityX);
                        break;

                    case "velocityY":
                        int.TryParse(property.value, out velocityY);
                        break;

                    default:
                        break;
                }
                if (property.name == "color")
                {
                    bg_name = property.value;
                }
            }

            EntityFactory.CreateParallaxBackground(bg_name, new Vector2(velocityX, velocityY));
        }

        /// <summary>
        /// Creates a regular enemy entity based on a TiledObject and its position.
        /// </summary>
        /// <param name="obj">The TiledObject representing the enemy.</param>
        /// <param name="position">The position of the enemy in the game world.</param>
        /// <returns>A new regular enemy entity.</returns>
        private static void RegularEnemy(TiledObject obj, Vector2 position)
        {
            // Set default values for left and right movement range, and initial direction
            int leftRange = 40;
            int rightRange = 40;
            State direction = State.WalkLeft;

            // Loop through object properties and update values if necessary
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "left":
                        int.TryParse(property.value, out leftRange);
                        break;

                    case "right":
                        int.TryParse(property.value, out rightRange);
                        break;

                    case "direction":
                        int result = 0;
                        int.TryParse(property.value, out result);
                        if (result == 1)
                        {
                            direction = State.WalkRight;
                        }
                        break;
                }
            }

            // Create and return a new regular enemy
            EntityFactory.CreateRegularEnemy(position, leftRange, rightRange, direction);
        }

        /// <summary>
        /// Creates a fruit entity based on the given TiledObject and position.
        /// </summary>
        /// <param name="obj">The TiledObject containing the fruit properties.</param>
        /// <param name="position">The position of the fruit.</param>
        /// <returns>The created fruit entity.</returns>
        private static void Fruit(TiledObject obj, Vector2 position)
        {
            string fruitType = "apple";
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "fruitType":
                        fruitType = property.value;
                        break;
                }
            }

            // Create and return the fruit entity
            EntityFactory.CreateFruit(position, fruitType);
        }
    }
}