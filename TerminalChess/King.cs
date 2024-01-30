using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class King : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public King(Colour colour) : base("", 0, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "K".Pastel(Color.Chocolate) : "K".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a King
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, GameEngine ge)
        {
            base.CalculatePossibleMoves(row, col, ge);

            CheckDiagonalMoves(row, col, 1, 1, ge);
            CheckDiagonalMoves(row, col, 1, -1, ge);
            CheckDiagonalMoves(row, col, -1, -1, ge);
            CheckDiagonalMoves(row, col, -1, 1, ge);
            CheckForwardAndBackMoves(row, col, 1, ge);
            CheckForwardAndBackMoves(row, col, -1, ge);
            CheckLeftAndRightMoves(row, col, 1, ge);
            CheckLeftAndRightMoves(row, col, -1, ge);
        }

        private void CheckDiagonalMoves(int row, int col, int rowModifier, int colModifier, GameEngine ge)
        {
            CheckMove(row + 1 * rowModifier, col + 1 * colModifier, ge);
        }

        private void CheckForwardAndBackMoves(int row, int col, int rowModifier, GameEngine ge)
        {
            CheckMove(row + 1 * rowModifier, col, ge);
        }

        private void CheckLeftAndRightMoves(int row, int col, int colModifier, GameEngine ge)
        {
            CheckMove(row, col + 1 * colModifier, ge);
        }

        private void CheckMove(int newRow, int newCol, GameEngine ge)
        {
            Square sqr = ge.GetSquareAtPos(newRow, newCol);

            if (sqr != null)
            {
                if(sqr.piece != null)
                {
                    if (sqr.piece.colour != this.colour)
                    {
                        possibleMoves.Add((sqr.row, sqr.col));
                    }
                }
            }
        }
    }
 }
