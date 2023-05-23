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
        public static void GetObjects(LevelID level)
        {
            // Fetch the TiledMap for the given level
            TiledMap map = Loader.tiledHandler.GetMap(level);

            // Iterate over each layer in the map
            foreach (var layer in map.Layers)
            {
                // Skip layers that are not entity layers
                if (!GameConstants.ENTITIES.Contains(layer.name))
                {
                    continue;
                }

                // Iterate over each object in the layer
                foreach (var obj in layer.objects)
                {
                    // Create position vector from object coordinates
                    Vector2 position = new Vector2((float)obj.x, (float)obj.y);

                    // Handle object creation based on object name
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

                        case "timer":
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
        private static void Background(TiledObject obj)
        {
            // Set default values for background color and velocities
            string bg_name = "bg_yellow";
            int velocityX = 0;
            int velocityY = 0;

            // Iterate over properties to update values as needed
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "color":
                        bg_name = property.value;
                        break;

                    case "velocityX":
                        int.TryParse(property.value, out velocityX);
                        break;

                    case "velocityY":
                        if (int.TryParse(property.value, out int tempVelocity))
                        {
                            velocityY = tempVelocity;
                        }
                        break;

                    default:
                        break;
                }
            }

            // Create the background entity
            EntityFactory.CreateParallaxBackground(bg_name, new Vector2(velocityX, velocityY));
        }

        /// <summary>
        /// Creates a regular enemy entity based on a TiledObject and its position.
        /// </summary>
        /// <param name="obj">The TiledObject representing the enemy.</param>
        /// <param name="position">The position of the enemy in the game world.</param>
        private static void RegularEnemy(TiledObject obj, Vector2 position)
        {
            // Set default values for enemy movement range and initial direction
            int leftRange = 40;
            int rightRange = 40;
            State direction = State.WalkLeft;

            // Iterate over object properties and update values if necessary
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "left":
                        if (int.TryParse(property.value, out int tempLeftRange))
                        {
                            leftRange = tempLeftRange;
                        }
                        break;

                    case "right":
                        if (int.TryParse(property.value, out int tempRightRange))
                        {
                            rightRange = tempRightRange;
                        }
                        break;

                    case "direction":
                        // If "direction" value is 1, enemy is set to walk right
                        if (int.TryParse(property.value, out int result) && result == 1)
                        {
                            direction = State.WalkRight;
                        }
                        break;
                }
            }

            // Create a regular enemy with the parsed properties
            EntityFactory.CreateRegularEnemy(position, leftRange, rightRange, direction);
        }

        /// <summary>
        /// Creates a fruit entity based on the given TiledObject and position.
        /// </summary>
        /// <param name="obj">The TiledObject containing the fruit properties.</param>
        /// <param name="position">The position of the fruit.</param>
        private static void Fruit(TiledObject obj, Vector2 position)
        {
            // Set default fruit type
            string fruitType = "apple";

            // Iterate over object properties and update values if necessary
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "fruitType":
                        fruitType = property.value;
                        break;
                }
            }

            // Create fruit entity with the parsed properties
            EntityFactory.CreateFruit(position, fruitType);
        }

        /// <summary>
        /// Creates a timer entity based on the given TiledObject and position.
        /// </summary>
        /// <param name="obj">The TiledObject containing the timer properties.</param>
        /// <param name="position">The position of the fruit.</param>
        private static void Timer(TiledObject obj, Vector2 position)
        {
            // Set default timer duration, in seconds
            int timer = 188;
            bool isActive = true;

            // Iterate over object properties and update values if necessary
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "time":
                        if (int.TryParse(property.value, out int tempTimer))
                        {
                            timer = tempTimer;
                        }
                        break;

                    case "active":
                        if (bool.TryParse(property.value, out bool temp))
                        {
                            isActive = temp;
                        }
                        break;

                    default:
                        break;
                }
            }

            // Create fruit entity with the parsed properties
            EntityFactory.CreateTimer(position, timer, isActive);
        }
    }
}
