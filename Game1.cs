using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public World world;
        private KeyboardState previousKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load resources
            Loader.LoadContent(Content);

            // Initialize the world
            world = new World();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
                Exit();

            if (currentKeyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R))
                world.ResetCurrentLevel();

            if (currentKeyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
                world.PreviousLevel();

            if (currentKeyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                world.NextLevel();

            previousKeyboardState = currentKeyboardState;

            world.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);
            Loader.tiledHandler.Draw(world.CurrentLevel.Id.ToString(), _spriteBatch);
            Loader.tiledHandler.DrawCollisionBoxes(world.CurrentLevel.Id.ToString(), _spriteBatch);
            world.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}