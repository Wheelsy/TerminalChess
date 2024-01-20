using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class GameEngine
    {
        private Player p1;
        private Player p2;
        private int cols = 8;
        private int rows = 8;
        private List<Square> board = new();
        private int numPieces;

        /// <summary>
        /// Constructor for a new game
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public GameEngine(Player p1, Player p2) { 
            this.p1 = p1;
            this.p2 = p2;
            numPieces = 32;

            AddPiecesToBoard();
        }

        /// <summary>
        /// Add all pieces in their starting position to the board
        /// </summary>
        public void AddPiecesToBoard()
        {
            for(int k = 0; k < cols; k++)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (k == 0)
                    {
                        if(i == 0 || i == 7)
                        {
                            Rook rook = new(Piece.Colour.White);
                            Square sqr = new(i, k, rook);
                            sqr.piece = rook;
                            board.Add(sqr);
                        }
                        else if(i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.White);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            board.Add(sqr);
                        }
                        else if(i == 2 || i == 5)
                        {
                            Bishop bishop = new(Piece.Colour.White);
                            Square sqr = new(i, k, bishop);
                            sqr.piece = bishop;
                            board.Add(sqr);
                        }
                        else if (i == 3)
                        {
                            Queen queen = new(Piece.Colour.White);
                            Square sqr = new(i, k, queen);
                            sqr.piece = queen;
                            board.Add(sqr);
                        }
                        else if (i == 4)
                        {
                            King king = new(Piece.Colour.White);
                            Square sqr = new(i, k, king);
                            sqr.piece = king;
                            board.Add(sqr);
                        }
                    }
                    else if (k == 1)
                    {
                        Pawn pawn = new(Piece.Colour.White);
                        Square sqr = new(i, k, pawn);
                        sqr.piece = pawn;
                        board.Add(sqr);
                    }
                    else if (k == 6)
                    {
                        Pawn pawn = new(Piece.Colour.Black);
                        Square sqr = new(i, k, pawn);
                        sqr.piece = pawn;
                        board.Add(sqr);
                    }
                    else if (k == 7)
                    {
                        if (i == 0 || i == 7)
                        {
                            Rook rook = new(Piece.Colour.Black);
                            Square sqr = new(i, k, rook);
                            sqr.piece = rook;
                            board.Add(sqr);
                        }
                        else if (i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.Black);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            board.Add(sqr);
                        }
                        else if (i == 2 || i == 5)
                        {
                            Bishop bishop = new(Piece.Colour.Black);
                            Square sqr = new(i, k, bishop);
                            sqr.piece = bishop;
                            board.Add(sqr);
                        }
                        else if (i == 3)
                        {
                            Queen queen = new(Piece.Colour.Black);
                            Square sqr = new(i, k, queen);
                            sqr.piece = queen;
                            board.Add(sqr);
                        }
                        else if (i == 4)
                        {
                            King king = new(Piece.Colour.Black);
                            Square sqr = new(i, k, king);
                            sqr.piece = king;
                            board.Add(sqr);
                        }
                    }
                    else
                    {
                        Square sqr = new(i, k, null);
                        board.Add(sqr);
                    }
                }
            }
        }

        public string View()
        {
            string view = "";
            Utils utils = new();
            string currentPlayer = (p1.MyTurn) ? p1.GetUsername() : p2.GetUsername();

            for (int row = 0; row < rows; row++)
            {
                view += "|"; // Start of the row

                for (int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;

                    if (board[index].piece == null)
                    {
                        view += " |"; // Space between squares
                    }
                    else
                    {
                        if (board[index].piece.color != Piece.Colour.Black)
                        {
                            view += $"{board[index].piece.name}".Pastel(Color.Chocolate); // Piece and end of square with colour
                            view += "|";
                        }
                        else
                        {
                            view += $"{board[index].piece.name}".Pastel(Color.SandyBrown); // Piece and end of square
                            view += "|";
                        }
                    }
                }

                view += "\n";
            }

           view += $"\n{currentPlayer} your turn:";

            return view;
        }

    }
}
