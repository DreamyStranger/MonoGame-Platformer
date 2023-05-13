using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Keeps track of coins
    /// </summary>
    public class CoinComponent : Component
    {
        //Position
        private  int _coins;

        /// <summary>
        /// Coins of the entity
        /// </summary>
        public int Coins { get => _coins; set => _coins= value; }

        /// <summary>
        /// Initializes a new instance of the CoinComponent class with the specified initial amount of coins.
        /// </summary>
        /// <param name="initialCoins">The initial amount of coins.</param>
        public CoinComponent(int initialCoins = 0)
        {
            _coins = 0;
        }
    }
}
