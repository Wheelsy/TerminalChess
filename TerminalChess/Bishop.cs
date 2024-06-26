﻿using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Bishop : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Bishop(Colour colour) : base("", 3, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "B".Pastel(Color.Chocolate) : "B".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Bishop
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, Board board)
        {
            base.CalculatePossibleMoves(row, col, board);

            CheckDiagonalMoves(row, col, 1, 1, board); // Upper right diagonal
            CheckDiagonalMoves(row, col, 1, -1, board); // Upper left diagonal
            CheckDiagonalMoves(row, col, -1, -1, board); // Lower left diagonal
            CheckDiagonalMoves(row, col, -1, 1, board); // Lower right diagonal
        }

        /// <summary>
        /// Loop through diagonals for possible squares to move to.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rowModifier"></param>
        /// <param name="colModifier"></param>
        /// <param name="ge"></param>
        private void CheckDiagonalMoves(int row, int col, int rowModifier, int colModifier, Board board)
        {
            for (int i = 1; i < 8; i++)
            {
                Square sqr = board.GetSquareAtPos(row + i * rowModifier, col + i * colModifier);

                if (sqr == null)
                {
                    break;
                }

                if (sqr.piece != null)
                {
                    if (sqr.piece.colour != this.colour)
                    {
                        possibleMoves.Add((sqr.row, sqr.col));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                possibleMoves.Add((sqr.row, sqr.col));
            }
        }
    }
}
