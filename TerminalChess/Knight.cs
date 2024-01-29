using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Knight : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Knight(Colour colour) : base("", 3, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "N".Pastel(Color.Chocolate) : "N".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Knight
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, GameEngine ge )
        {
           base.CalculatePossibleMoves(row, col, ge);

            // Check if the square in the front-left is within the board boundaries (L shape move)
            Square frontLeft = ge.GetSquareAtPos(row + (2 * modifier), col - (1 * modifier));

            if (frontLeft != null)
            {
                possibleMoves.Add((frontLeft.row, frontLeft.col));
            }

            // Check if the square in the front-right is within the board boundaries (L shape move)
            Square frontRight = ge.GetSquareAtPos(row + (2 * modifier), col + (1 * modifier));

            if (frontRight != null)
            {
                possibleMoves.Add((frontRight.row, frontRight.col));
            }

            // Check if the square to the left is within the board boundaries (L shape move)
            Square left = ge.GetSquareAtPos(row + (1 * modifier), col - (2 * modifier));

            if (left != null)
            {
                possibleMoves.Add((left.row, left.col));
            }

            // Check if the square to the right is within the board boundaries (L shape move)
            Square right = ge.GetSquareAtPos(row + (1 * modifier), col + 2 * modifier);

            if (right != null)
            {
                possibleMoves.Add((right.row, right.col));
            }

            // Check if the square in the back-left is within the board boundaries (L shape move)
            Square backLeft = ge.GetSquareAtPos(row - (2 * modifier), col - (1 * modifier));

            if (backLeft != null)
            {
                possibleMoves.Add((backLeft.row, backLeft.col));
            }

            // Check if the square in the back-right is within the board boundaries (L shape move)
            Square backRight = ge.GetSquareAtPos(row - (2 * modifier), col + (1 * modifier));

            if (backRight != null)
            {
                possibleMoves.Add((backRight.row, backRight.col));
            }
        }
    }
}
