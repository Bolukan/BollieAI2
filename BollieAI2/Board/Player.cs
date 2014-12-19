﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BollieAI2.Board
{
    /// <summary>
    /// The player for a region
    /// </summary>
    public enum PlayerType
    {
        Unknown = 0x01,
        Neutral = 0x02,
        Me = 0x04,
        Opponent = 0x08,
        Wasteland = 0x10,
        NotMe = Unknown | Neutral | Opponent | Wasteland,
        All = NotMe | Me
    }

    /// <summary>
    /// Player
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Transform name to playercode
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>playercode</returns>
        public static PlayerType Player(string name)
        {
            if (name == Configuration.PLAYER_NEUTRAL) return PlayerType.Neutral;
            if (name == Map.Current.YourBot) return PlayerType.Me;
            if (name == Map.Current.OpponentBot) return PlayerType.Opponent;
            return PlayerType.Unknown;
        }
    }

}