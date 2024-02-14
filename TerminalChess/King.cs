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

            CheckDiagonalMove(row, col, 1, -1, ge); // Upper left diagonal
            CheckDiagonalMove(row, col, -1, -1, ge); // Lower left diagonal
            CheckDiagonalMove(row, col, -1, 1, ge); // Lower right diagonal
            CheckForwardAndBackMove(row, col, 1, ge); // Check up moves
            CheckForwardAndBackMove(row, col, -1, ge); // Check down moves
            CheckLeftAndRightMove(row, col, 1, ge); // Check right moves
            CheckLeftAndRightMove(row, col, -1, ge); // Check left moves
            CheckCastling(ge);
        }

        private void CheckCastling(GameEngine ge)
        {
            if(!HasMoved && colour == Colour.Black)
            {
                // Check black castle king side
                if(ge.GetSquareAtPos(7, 7).piece != null)
                {
                    if (!ge.GetSquareAtPos(7, 7).piece.HasMoved)
                    {
                        if (ge.GetSquareAtPos(7, 6).piece == null && ge.GetSquareAtPos(7, 5).piece == null)
                        {
                            possibleMoves.Add((7, 6));
                        }
                    }
                }
                // Check black castle queen side
                if (ge.GetSquareAtPos(7, 0).piece != null)
                {
                    if (!ge.GetSquareAtPos(7, 0).piece.HasMoved)
                    {
                        if (ge.GetSquareAtPos(7, 1).piece == null && ge.GetSquareAtPos(7, 2).piece == null && ge.GetSquareAtPos(7, 3).piece == null)
                        {
                            possibleMoves.Add((7, 2));
                        }
                    }
                }
            }
            else if(!HasMoved && colour == Colour.White)
            {
                // Check white castle king side
                if (ge.GetSquareAtPos(0, 7).piece != null)
                {
                    if (!ge.GetSquareAtPos(0, 7).piece.HasMoved)
                    {
                        if (ge.GetSquareAtPos(0, 6).piece == null && ge.GetSquareAtPos(0, 5).piece == null)
                        {
                            possibleMoves.Add((0, 6));
                        }
                    }
                }
                // Check white castle queen side
                if (ge.GetSquareAtPos(0, 0).piece != null)
                {
                    if (!ge.GetSquareAtPos(0, 0).piece.HasMoved)
                    {
                        if (ge.GetSquareAtPos(0, 1).piece == null && ge.GetSquareAtPos(0, 2).piece == null && ge.GetSquareAtPos(0, 3).piece == null)
                        {
                            possibleMoves.Add((0, 2));
                        }
                    }
                }
            }
        }

        private void CheckDiagonalMove(int row, int col, int rowModifier, int colModifier, GameEngine ge)
        {
            CheckMove(row + 1 * rowModifier, col + 1 * colModifier, ge);
        }

        private void CheckForwardAndBackMove(int row, int col, int rowModifier, GameEngine ge)
        {
            CheckMove(row + 1 * rowModifier, col, ge);
        }

        private void CheckLeftAndRightMove(int row, int col, int colModifier, GameEngine ge)
        {
            CheckMove(row, col + 1 * colModifier, ge);
        }

        private void CheckMove(int newRow, int newCol, GameEngine ge)
        {
            Square sqr = ge.GetSquareAtPos(newRow, newCol);

            if (sqr != null)
            {
                if (sqr.piece != null)
                {
                    if (sqr.piece.colour != this.colour)
                    {
                        possibleMoves.Add((sqr.row, sqr.col));
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                possibleMoves.Add((sqr.row, sqr.col));
            }
        }
    }
 }
