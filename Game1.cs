﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// The main game class.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Rectangle _destinationRenderRectangle = new Rectangle();
        private KeyboardState _previousKeyboardState;
        private bool _isPaused = false;

        public World world;

        /// <summary>
        /// Initializes the Game1 class.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = false;
        }

        /// <summary>
        /// Initializes the game window and sets the desired resolution.
        /// </summary>
        protected override void Initialize()
        {
            // Set the game window title
            Window.Title = "NinjaFrog";

            // Change the resolution 
            _graphics.PreferredBackBufferWidth = GameConstants.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = GameConstants.SCREEN_HEIGHT;

            // Set fullscreen mode
            _graphics.IsFullScreen = GameConstants.FULL_SCREEN;

            // Set the hardware mode switch (optional)
            // Set to false to use a borderless windowed fullscreen mode that scales your game resolution
            // Set to true to use exclusive fullscreen mode that changes the screen resolution to match your game resolution
            _graphics.HardwareModeSwitch = false; // or true, depending on your preference

            _graphics.ApplyChanges();

            // Limit FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / GameConstants.PhysicFPS);

            base.Initialize();
        }

        /// <summary>
        /// Loads game content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _renderTarget = new RenderTarget2D(GraphicsDevice, GameConstants.SCREEN_WIDTH, GameConstants.SCREEN_HEIGHT);

            // Load resources
            Loader.LoadContent(GraphicsDevice);

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

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (_isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }

            if (!_isPaused)
            {
                // Process game input and update game logic
                if (currentKeyboardState.IsKeyDown(Keys.R) && _previousKeyboardState.IsKeyUp(Keys.R))
                {
                    MessageBus.Publish(new ReloadLevelMessage());
                }
                else if (currentKeyboardState.IsKeyDown(Keys.P) && _previousKeyboardState.IsKeyUp(Keys.P))
                {
                    world.PreviousLevel();
                }
                else if (currentKeyboardState.IsKeyDown(Keys.N) && _previousKeyboardState.IsKeyUp(Keys.N))
                {
                    MessageBus.Publish(new NextLevelMessage());
                }

                world.Update(gameTime);
            }

            _previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (GameConstants.FULL_SCREEN)
            {
                // Set the render target to draw the game content
                GraphicsDevice.SetRenderTarget(_renderTarget);

                // Draw the game content
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                world.Draw(_spriteBatch);
                _spriteBatch.End();

                // Reset the render target back to the default one
                GraphicsDevice.SetRenderTarget(null);
                //Scale window
                UpdateScaling();

                // Draw the render target texture centered and scaled to the screen
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                _spriteBatch.Draw(_renderTarget, _destinationRenderRectangle, Color.White);
                _spriteBatch.End();
            }
            else
            {
                // Draw the game content
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                world.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void UpdateScaling()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / GameConstants.SCREEN_WIDTH;
            float scaleY = (float)GraphicsDevice.Viewport.Height / GameConstants.SCREEN_HEIGHT;
            float scaleFactor = Math.Min(scaleX, scaleY);
            int screenWidth = (int)(GameConstants.SCREEN_WIDTH * scaleFactor);
            int screenHeight = (int)(GameConstants.SCREEN_HEIGHT * scaleFactor);
            int offsetX = (GraphicsDevice.Viewport.Width - screenWidth) / 2;
            int offsetY = (GraphicsDevice.Viewport.Height - screenHeight) / 2;
            _destinationRenderRectangle = new Rectangle(offsetX, offsetY, screenWidth, screenHeight);
        }

        private void PauseGame()
        {
            _isPaused = true;
            MediaPlayer.Pause();
        }

        private void ResumeGame()
        {
            _isPaused = false;
            MediaPlayer.Resume();
        }
    }
}