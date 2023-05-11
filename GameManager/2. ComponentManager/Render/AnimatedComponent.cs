using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace EC_Framework
{
    /// <summary>
    /// <see cref="Component"/>  responsible for managing entity animations.
    /// </summary>
    /// <remarks>
    /// This component contains dictionary that maps an action to its respective animation (stateID to ActionAnimation). 
    /// It also contains current animation of an entity and few useful methods.
    /// </remarks>
    public class AnimatedComponent : Component
    {
        /// <summary>
        /// A dictionary of all the animations for this component, indexed by action name.
        /// </summary>
        public Dictionary<string, ActionAnimation> Animations { get; private set; }

        /// <summary>
        /// The name of the current animation action.
        /// </summary>
        public string CurrentAction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the AnimatedComponent class.
        /// </summary>
        public AnimatedComponent()
        {
            Animations = new Dictionary<string, ActionAnimation>();
            CurrentAction = "idle";
        }

        /// <summary>
        /// Adds an animation for a given action to the list of animations.
        /// </summary>
        /// <param name="sprite">The name (ID from Loader Class) of the sprite to use for the animation.</param>
        /// <param name="action">The name of the action (ID from StateComponent) associated with the animation.</param>
        /// <param name="rows">The number of rows in the sprite sheet.</param>
        /// <param name="columns">The number of columns in the sprite sheet.</param>
        /// <param name="fps">The number of frames per second for the animation.</param>
        public void AddAnimation(string sprite, string action, int rows, int columns, float fps)
        {
            Animations[action] = new ActionAnimation(sprite, rows, columns, fps);
        }

        // <summary>
        /// Returns the current animation associated with the component,
        /// or the default "idle" animation if the current action does not have an animation associated with it.
        /// </summary>
        /// <returns>The current animation associated with the component, or the default "idle" animation if the current action does not have an animation associated with it.</returns>
        public ActionAnimation GetCurrentAnimation()
        {
            if (Animations.ContainsKey(CurrentAction))
            {
                return Animations[CurrentAction];
            }

            if (GameConstants.AnimationDebugMessages)
            {
                Console.WriteLine($"Animation for action '{CurrentAction}' does not exist, playing default animation"); // Debug message
            }

            return Animations.TryGetValue("idle", out ActionAnimation defaultAnimation) ? defaultAnimation : null;
        }

        /// <summary>
        /// Updates the current animation based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public void Update(GameTime gameTime)
        {
            ActionAnimation currentAnimation = GetCurrentAnimation();
            currentAnimation.Update(gameTime);
        }

        /// <summary>
        /// Draws the current animation on the screen.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the animation.</param>
        /// <param name="position">The position of the animation on the screen.</param>
        /// <param name="direction">The horizontal direction the animation is facing.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int direction = 1)
        {
            if (Animations.ContainsKey(CurrentAction))
            {
                ActionAnimation currentAnimation = GetCurrentAnimation();
                currentAnimation.Draw(spriteBatch, position, direction);
            }
        }

        /// <summary>
        /// Changes the current animation if a new action is given.
        /// </summary>
        /// <param name="action">The name of the new action.</param>
        public void SetCurrentAction(string action)
        {
            if (CurrentAction != action)
            {
                if (GameConstants.AnimationDebugMessages)
                {
                    Console.WriteLine($"Animation {CurrentAction} changes to Animation {action}");
                }
                CurrentAction = action;
                ResetCurrentAnimation();
            }
        }

        /// <summary>
        /// Reset Current Animation
        /// </summary>
        private void ResetCurrentAnimation()
        {
            ActionAnimation currentAnimation = GetCurrentAnimation();
            currentAnimation.Reset();
        }
    }

    /// <summary>
    /// Represents an individual animation composed of multiple frames, each of which is a rectangle in a sprite sheet.
    /// </summary>
    public class ActionAnimation
    {
        private Texture2D _texture;         // The texture containing the sprite sheet.
        private int _rows;                  // The number of rows in the sprite sheet.
        private int _columns;               // The number of columns in the sprite sheet.
        private int _currentFrame;          // The index of the current frame being displayed.
        private int _totalFrames;           // The total number of frames in the sprite sheet.
        private float _frameTime;           // The time between each frame in seconds.
        private float _elapsedFrameTime;    // The time elapsed since the last frame change in seconds.
        private Rectangle[] _frames;        // An array of rectangles defining each frame in the sprite sheet.

        /// <summary>
        /// Tells if current animation was finished
        /// </summary>
        public bool IsFinished { get { return _currentFrame >= _totalFrames - 1; } }

        /// <summary>
        /// Creates a new instance of the ActionAnimation class.
        /// </summary>
        /// <param name="sprite">The ID of sprite sheet in Loader Class.</param>
        /// <param name="rows">The number of rows in the sprite sheet.</param>
        /// <param name="columns">The number of columns in the sprite sheet.</param>
        /// <param name="fps">The frame rate in frames per second.</param>
        public ActionAnimation(string sprite, int rows, int columns, float fps)
        {
            _texture = Loader.GetTexture(sprite);
            _rows = rows;
            _columns = columns;
            _currentFrame = 0;
            _totalFrames = _rows * _columns;
            _frameTime = 1 / fps;
            _elapsedFrameTime = 0;

            _frames = new Rectangle[_totalFrames];
            int frameWidth = _texture.Width / _columns;
            int frameHeight = _texture.Height / _rows;
            for (int i = 0; i < _totalFrames; i++)
            {
                int x = (i % _columns) * frameWidth;
                int y = (i % _rows) * frameHeight;
                _frames[i] = new Rectangle(x, y, frameWidth, frameHeight);
            }
        }

        /// <summary>
        /// Updates the animation based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public void Update(GameTime gameTime)
        {
            if (_frames.Length == 1)
            {
                return;
            }
            _elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedFrameTime >= _frameTime)
            {
                _currentFrame++;
                if (_currentFrame >= _totalFrames)
                {
                    _currentFrame = 0;
                }
                _elapsedFrameTime = 0;
            }
        }

        /// <summary>
        /// Draws the current frame of the animation at the specified position with the specified direction.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw the frame.</param>
        /// <param name="position">The position of the frame in the game world.</param>
        /// <param name="direction">The direction the frame is facing (1 for right, -1 for left).</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int direction = 1)
        {
            bool isFacingLeft = false;
            if (direction == -1)
                isFacingLeft = true;

            Rectangle currentFrame = _frames[_currentFrame];

            //Console.WriteLine($"Drawing frame {CurrentFrame}: X = {currentFrame.X}, Y = {currentFrame.Y}, Width = {currentFrame.Width}, Height = {currentFrame.Height}");

            spriteBatch.Draw(_texture,
                     position,
                     sourceRectangle: currentFrame,
                     Color.White,
                     0f,
                     Vector2.Zero,
                     1f,
                     isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                     0f);
        }

        /// <summary>
        /// Resets the animation to the first frame.
        /// </summary>
        public void Reset()
        {
            _currentFrame = 0;
            _elapsedFrameTime = 0;
        }
    }
}