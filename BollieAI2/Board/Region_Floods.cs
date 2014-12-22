﻿using System;
using System.Collections.Generic;
using System.Linq;
using BollieAI2.Services;

namespace BollieAI2.Board
{
    /// <summary>
    /// Subclass for Region to contain Floods
    /// </summary>
    public partial class Region
    {
        /// <summary>
        /// Distance (1 each Region) to nearest Unknown or Opponent Region
        /// </summary>
        public int DangerousBorderDistance { get; set; }
    }

}
