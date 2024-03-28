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
        public bool LastMoveWasDouble { get; set; }
        public (int, int) EPCapture { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Pawn(Colour colour) : base("", 1, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "P".Pastel(Color.Chocolate) : "P".Pastel(Color.SandyBrown);
            LastMoveWasDouble = false;
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Pawn
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, Board board)
        {
            base.CalculatePossibleMoves(row, col, board);
            CheckEnPassent(board, row, col);

            // Check if the square in front is occupied
            Square infront = board.GetSquareAtPos(row + colourModifier, col);

            if (infront != null)
            {
                if (infront.piece == null)
                {
                    possibleMoves.Add((infront.row, infront.col));

                    // Check for a double move
                    if (row == 1 || row == 6)
                    {
                        // Check if the square 2 in front is occupied
                        Square doubleMove = board.GetSquareAtPos(row + (2 * colourModifier), col);

                        if (doubleMove != null)
                        {
                            if (doubleMove.piece == null)
                            {
                                possibleMoves.Add((doubleMove.row, doubleMove.col));
                            }
                        }
                    }
                }
            }    

            // Check diagonals
            Square diagonal1 = board.GetSquareAtPos(row + colourModifier, col + colourModifier);

            if (diagonal1 != null)
            {
                if (diagonal1.piece != null)
                {
                    if (diagonal1.piece.colour != this.colour)
                    {
                        possibleMoves.Add((diagonal1.row, diagonal1.col));
                    }
                }
            }

            Square diagonal2 = board.GetSquareAtPos(row + colourModifier, col - colourModifier);

            if (diagonal2 != null)
            {
                if (diagonal2.piece != null)
                {
                    if (diagonal2.piece.colour != this.colour)
                    {
                        possibleMoves.Add((diagonal2.row, diagonal2.col));
                    }
                }
            }
        }

        // Check if this pawn can en passent
        private void CheckEnPassent(Board board, int row, int col)
        {
            Square s1 = board.GetSquareAtPos(row, col + 1);
            Square s2 = board.GetSquareAtPos(row, col - 1);

            if (s1 != null)
            {
                if (s1.piece != null)
                {
                    if (s1.piece is Pawn pawn)
                    {
                        if (pawn.LastMoveWasDouble)
                        {
                            possibleMoves.Add((s1.row + 1 * colourModifier, s1.col));
                            EPCapture = (s1.row, s1.col);
                        }
                    }
                }
            }

            if (s2 != null)
            {
                if (s2.piece != null)
                {
                    if (s2.piece is Pawn pawn)
                    {
                        if (pawn.LastMoveWasDouble)
                        {
                            possibleMoves.Add((s2.row + 1 * colourModifier, s2.col));
                            EPCapture = (s2.row, s2.col);
                        }
                    }
                }
            }
        }
    }
}
