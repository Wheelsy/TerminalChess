﻿using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Queen : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Queen(Colour colour) : base("", 9, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "Q".Pastel(Color.Chocolate) : "Q".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Queen
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, Board board)
        {
            base.CalculatePossibleMoves(row, col, board);

            CheckDiagonalMoves(row, col, 1, 1, board); // Upper right diagonal
            CheckDiagonalMoves(row, col, 1, -1, board); // Upper left diagonal
            CheckDiagonalMoves(row, col, -1, -1, board); // Lower left diagonal
            CheckDiagonalMoves(row, col, -1, 1, board); // Lower right diagonal
            CheckForwardAndBackMoves(row, col, 1, board); // Check up moves
            CheckForwardAndBackMoves(row, col, -1, board); // Check down moves
            CheckLeftAndRightMoves(row, col, 1, board); // Check right moves
            CheckLeftAndRightMoves(row, col, -1, board); // Check left moves
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

        /// <summary>
        /// Loop through the forward and backward column of squares
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rowModifier"></param>
        /// <param name="ge"></param>
        private void CheckForwardAndBackMoves(int row, int col, int rowModifier, Board board)
        {
            for (int i = 1; i < 8; i++)
            {
                Square sqr = board.GetSquareAtPos(row + i * rowModifier, col);

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

        /// <summary>
        /// Loop through the left and right row of squares
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="colModifier"></param>
        /// <param name="ge"></param>
        private void CheckLeftAndRightMoves(int row, int col,  int colModifier, Board board)
        {
            for (int i = 1; i < 8; i++)
            {
                Square sqr = board.GetSquareAtPos(row, col + i * colModifier);

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
