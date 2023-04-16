using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;
namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public SystemManager systems;

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

        //Limit FPS
        IsFixedTimeStep = true;  //Force the game to update at fixed time intervals
        TargetElapsedTime = TimeSpan.FromSeconds(1 / GameConstants.FPS);  //Set Time Interval

        base.Initialize();
    }

    protected override void LoadContent()
    {

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        //Resources
        Loader.LoadContent(Content);

        //Entities
        systems = new SystemManager("Level 1");
        ObjectInitializer objectInitializer = new ObjectInitializer();
        List<Entity> objects = objectInitializer.GetObjects();
        foreach(Entity entity in objects)
        {
            systems.Add(entity);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        systems.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp); //By Layer Depth and no Blending to avoid blurs
        Loader.tiledHandler.Draw("Level 1", _spriteBatch);
        Loader.tiledHandler.DrawCollisionBoxes("Level 1", _spriteBatch);
        systems.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }


}
