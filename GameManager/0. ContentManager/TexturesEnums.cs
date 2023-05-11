using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonogameExamples
{
    public enum PlayerTexture
    {
        Idle,
        Walking,
        Jump,
        DoubleJump,
        Fall,
        Slide,
        Hit
    }

    public enum MaskedEnemyTexture
    {
        Idle,
        Walking,
        Jump,
        DoubleJump,
        Fall,
        Slide,
        Hit
    }

    public enum BackgroundTexture
    {
        Green,
        Yellow
    }

    public enum FruitTexture
    {
        Apple,
        Collected
    }

    public enum TiledTexture
    {
        Terrain,
        UI,
    }

}
