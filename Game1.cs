using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace ECS_Framework
{
    /// <summary>
    /// The main game class.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public World world;
        private KeyboardState previousKeyboardState;

        /// <summary>
        /// Initializes the Game1 class.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        /// <summary>
        /// Initializes the game window and sets the desired resolution.
        /// </summary>
        protected override void Initialize()
        {
            // Change the resolution 
            _graphics.PreferredBackBufferWidth = GameConstants.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = GameConstants.SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            // Limit FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / GameConstants.FPS);

            base.Initialize();
        }

        /// <summary>
        /// Loads game content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load resources
            Loader.LoadContent(Content);

            // Initialize the world
            world = new World();
        }


        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                Exit();
            }

            else if (currentKeyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R))
            {
                MessageBus.Publish(new ReloadLevelMessage());
            }
            else if (currentKeyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
            {
                world.PreviousLevel();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
            {
                MessageBus.Publish(new NextLevelMessage());
            }

            previousKeyboardState = currentKeyboardState;

            world.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            world.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}