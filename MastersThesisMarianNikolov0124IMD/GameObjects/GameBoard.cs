﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastersThesisMarianNikolov0124IMD.GameObjects
{
    public class GameBoard : GameObject
    {
        public GameBoard(Position givePosition) : base(givePosition)
        {
        }

        public GameBoard(Position givePosition, Size giveSize) : base(givePosition, giveSize)
        {
        }
    }
}