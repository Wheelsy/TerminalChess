using Pastel;
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
        protected override void CalculatePossibleMoves(int row, int col, GameEngine ge)
        {
            base.CalculatePossibleMoves(row, col, ge);

            CheckDiagonalMoves(row, col, 1, 1, ge); // Upper right diagonal
            CheckDiagonalMoves(row, col, 1, -1, ge); // Upper left diagonal
            CheckDiagonalMoves(row, col, -1, -1, ge); // Lower left diagonal
            CheckDiagonalMoves(row, col, -1, 1, ge); // Lower right diagonal
        }

        private void CheckDiagonalMoves(int row, int col, int rowModifier, int colModifier, GameEngine ge)
        {
            for (int i = 1; i < 8; i++)
            {
                Square sqr = ge.GetSquareAtPos(row + i * rowModifier, col + i * colModifier);

                if (sqr == null)
                {
                    break;
                }

                possibleMoves.Add((sqr.row, sqr.col));

                if(sqr.piece != null)
                {
                    break;
                }
            }
        }
    }
}
