﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Square
    {
        private int col;
        private int row;
        private Piece piece;

        public  Square(int col, int row, Piece piece)
        {
            this.col = col;
            this.row = row;
            this.piece = piece;
        }
    }
}