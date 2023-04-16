using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// A component that implements a parallax effect for a sprite in a 2D game.
    /// </summary>
    public class ParallaxComponent : Component
    {
        private Texture2D _texture;
        private Vector2 _velocity;
        private Vector2 _position;
        private Vector2 _position2;

        private int _tileX;
        private int _tileY;

        /// <summary>
        /// Initializes a new instance of the ParallaxComponent class.
        /// NOTE: It will miss a tile in a top left corner for Velcity = (x < 0, y > 0)!!!
        /// </summary>
        /// <param name="sprite">The filename of the sprite to use.</param>
        /// <param name="velocity">The velocity of the parallax effect.</param>
        /// <param name="position">The starting position of the sprite.</param>
        /// <param name="viewX">The width of the parallax window.</param>
        /// <param name="viewY">The height of the parallax window.</param>
        public ParallaxComponent(string sprite, Vector2 velocity, Vector2 position, int viewX, int viewY)
        {
            _texture = Loader.GetTexture(sprite);
            _velocity = velocity;
            _position = position;
            _position2 = position;

            _tileX = (int)Math.Ceiling((float)viewX / _texture.Width) + 2;
            _tileY = (int)Math.Ceiling((float)viewY / _texture.Height) + 2;

            _position2 = position;


            if (velocity.X != 0)
            {
                _position2.X -= Math.Sign(velocity.X) * _texture.Width;
            }

            if (velocity.Y != 0)
            {
                _position2.Y -= Math.Sign(velocity.Y) * _texture.Height;
            }

        }

        /// <summary>
        /// Updates the position of the sprite based on the elapsed time and velocity. Also loops the sprite horizontally and vertically if necessary.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game time.</param>
        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_velocity.X != 0)
            {
                _position.X += _velocity.X * elapsedSeconds;
                _position2.X += _velocity.X * elapsedSeconds;
            }

            if (_velocity.Y != 0)
            {
                _position.Y += _velocity.Y * elapsedSeconds;
                _position2.Y += _velocity.Y * elapsedSeconds;
            }

            // Loop the parallax background horizontally
            if (_velocity.X != 0)
            {
                if (_position.X <= -_texture.Width)
                {
                    _position.X = _position2.X + _texture.Width;
                }
                if (_position2.X <= -_texture.Width)
                {
                    _position2.X = _position.X + _texture.Width;
                }
                if (_position.X >= _texture.Width)
                {
                    _position.X = _position2.X - _texture.Width;
                }
                if (_position2.X >= _texture.Width)
                {
                    _position2.X = _position.X - _texture.Width;
                }
            }

            // Loop the parallax background vertically
            if (_velocity.Y != 0)
            {
                if (_position.Y >= _texture.Height)
                {
                    _position.Y = _position2.Y - _texture.Height;
                }
                if (_position2.Y >= _texture.Height)
                {
                    _position2.Y = _position.Y - _texture.Height;
                }
                if (_position.Y <= -_texture.Height)
                {
                    _position.Y = _position2.Y + _texture.Height;
                }
                if (_position2.Y <= -_texture.Height)
                {
                    _position2.Y = _position.Y + _texture.Height;
                }
            }

        }

        /// <summary>
        /// Draws the sprite with the parallax effect.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object to use for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _tileX; x++)
            {
                for (int y = 0; y < _tileY; y++)
                {
                    Vector2 texturePosition = new Vector2(x * _texture.Width, y * _texture.Height);
                    spriteBatch.Draw(_texture, _position + texturePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    spriteBatch.Draw(_texture, _position2 + texturePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }
        }

    }
}
