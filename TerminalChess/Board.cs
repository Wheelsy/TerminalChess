using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TerminalChess.Piece;

namespace TerminalChess
{
    internal class Board
    {
        public List<Square> BoardSquares { get; }
        const int COLS = 8;
        const int ROWS = 8;

        public Board() {
            BoardSquares = new();
        }

        public void AddPiecesToBoard()
        {
            // Iterate through column
            for (int k = COLS - 1; k >= 0; k--)
            {
                // Iterate through rows
                for (int i = 0; i < ROWS; i++)
                {
                    // First column
                    if (k == 0)
                    {
                        // Rook rows
                        if (i == 0 || i == 7)
                        {
                            Rook rook = new(Piece.Colour.White);
                            Square sqr = new(i, k, rook);
                            sqr.piece = rook;

                            BoardSquares.Add(sqr);
                        }
                        // Knight rows
                        else if (i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.White);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            BoardSquares.Add(sqr);
                        }
                        // Bishop rows
                        else if (i == 2 || i == 5)
                        {
                            Bishop bishop = new(Piece.Colour.White);
                            Square sqr = new(i, k, bishop);
                            sqr.piece = bishop;
                            BoardSquares.Add(sqr);
                        }
                        // Queen row
                        else if (i == 3)
                        {
                            Queen queen = new(Piece.Colour.White);
                            Square sqr = new(i, k, queen);
                            sqr.piece = queen;
                            BoardSquares.Add(sqr);
                        }
                        // King row
                        else if (i == 4)
                        {
                            King king = new(Piece.Colour.White);
                            Square sqr = new(i, k, king);
                            sqr.piece = king;
                            BoardSquares.Add(sqr);
                        }
                    }
                    // Second column (pawns)
                    else if (k == 1)
                    {
                        Pawn pawn = new(Piece.Colour.White);
                        Square sqr = new(i, k, pawn);
                        sqr.piece = pawn;
                        BoardSquares.Add(sqr);
                    }
                    // Seventh column (pawns)
                    else if (k == 6)
                    {
                        Pawn pawn = new(Piece.Colour.Black);
                        Square sqr = new(i, k, pawn);
                        sqr.piece = pawn;
                        BoardSquares.Add(sqr);
                    }
                    // Eighth column
                    else if (k == 7)
                    {
                        // Rook rows
                        if (i == 0 || i == 7)
                        {
                            Rook rook = new(Piece.Colour.Black);
                            Square sqr = new(i, k, rook);
                            sqr.piece = rook;
                            BoardSquares.Add(sqr);
                        }
                        // Knight rows
                        else if (i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.Black);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            BoardSquares.Add(sqr);
                        }
                        // Bishop rows
                        else if (i == 2 || i == 5)
                        {
                            Bishop bishop = new(Piece.Colour.Black);
                            Square sqr = new(i, k, bishop);
                            sqr.piece = bishop;
                            BoardSquares.Add(sqr);
                        }
                        // Queen row
                        else if (i == 3)
                        {
                            Queen queen = new(Piece.Colour.Black);
                            Square sqr = new(i, k, queen);
                            sqr.piece = queen;
                            BoardSquares.Add(sqr);
                        }
                        // King row
                        else if (i == 4)
                        {
                            King king = new(Piece.Colour.Black);
                            Square sqr = new(i, k, king);
                            sqr.piece = king;
                            BoardSquares.Add(sqr);
                        }
                    }
                    // All other squares are empty and are added with a NULL piece
                    else
                    {
                        Square sqr = new(i, k, null);
                        BoardSquares.Add(sqr);
                    }
                }
            }
        }

        /// <summary>
        /// Find the square at a given coordinate
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Square GetSquareAtPos(int row, int col)
        {
            foreach (Square s in BoardSquares)
            {
                if (s.col == col && s.row == row)
                {
                    return s;
                }
            }

            return null;
        }

        public List<Square> GetAllSquaresWithPiecesOfOneColour(Colour colour)
        {
            var squares = new List<Square>();

            foreach (Square s in BoardSquares)
            {
                if (s.piece != null)
                {
                    if (s.piece.colour == colour)
                    { 
                        squares.Add(s);
                    }
                }
            }

            return squares;
        }

        public Square GetSquareByPiece<T>(Colour colour) where T : Piece
        {
            foreach (Square square in BoardSquares)
            {
                if (square.piece != null && square.piece.colour == colour && square.piece.GetType() == typeof(T))
                {
                    return square;
                }
            }

            return null;
        }

        public Square GetSquareAtIndex(int index){
            return BoardSquares[index];
        }

        /// <summary>
        /// Find and return all remaining pieces on the board
        /// </summary>
        /// <returns></returns>
        public List<Piece> GetRemainingPieces()
        {
            List<Piece> remainingPieces = new List<Piece>();

            foreach (Square square in BoardSquares)
            {
                if (square.piece != null)
                {
                    remainingPieces.Add(square.piece);
                }
            }

            return remainingPieces;
        }
    }
}
