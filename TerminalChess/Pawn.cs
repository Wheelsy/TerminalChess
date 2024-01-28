using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Pawn : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Pawn(Colour colour) : base("", 1, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "P".Pastel(Color.Chocolate) : "P".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Pawn
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col)
        {
            base.CalculatePossibleMoves(row, col);

            if (ge.TurnNo == 1)
            {
                possibleMoves.Add(row + 1 * modifier, col);
                possibleMoves.Add(row + 2 * modifier, col);
                return;
            }

            // Check if the square in fron is occupied
            Square infront = ge.GetSquareAtPos(row + 1 * modifier, col);

            if(infront.piece == null)
            {
                possibleMoves.Add(infront.row, infront.col);
            }

            // Check diagonals
            Square diagonal1 = ge.GetSquareAtPos(row + 1 * modifier, col + 1 * modifier);

            if (diagonal1.piece != null)
            {
                possibleMoves.Add(diagonal1.row, diagonal1.col);
            }

            Square diagonal2 = ge.GetSquareAtPos(row + 1 * modifier, col - 1 * modifier);

            if (diagonal2.piece != null)
            {
                possibleMoves.Add(diagonal2.row, diagonal2.col);
            }
        }
    }
}
