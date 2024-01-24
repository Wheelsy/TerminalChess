﻿using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class GameEngine
    {
        private int cols = 8;
        private int rows = 8;
        private List<Square> board = new();
        private int numPieces;
        public Player p1 { get; }
        public Player p2 { get; }

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
            // Iterate through column
            for (int k = 0; k < cols; k++)
            {
                // Iterate through rows
                for (int i = 0; i < rows; i++)
                {
                    // First column
                    if (k == 0)
                    {
                        // Rook rows
                        if(i == 0 || i == 7)
                        {
                            Rook rook = new(Piece.Colour.White);
                            Square sqr = new(i, k, rook);
                            sqr.piece = rook;
                            board.Add(sqr);
                        }
                        // Knight rows
                        else if(i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.White);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            board.Add(sqr);
                        }
                        // Bishop rows
                        else if(i == 2 || i == 5)
                        {
                            Bishop bishop = new(Piece.Colour.White);
                            Square sqr = new(i, k, bishop);
                            sqr.piece = bishop;
                            board.Add(sqr);
                        }
                        // Queen row
                        else if (i == 3)
                        {
                            Queen queen = new(Piece.Colour.White);
                            Square sqr = new(i, k, queen);
                            sqr.piece = queen;
                            board.Add(sqr);
                        }
                        // King row
                        else if (i == 4)
                        {
                            King king = new(Piece.Colour.White);
                            Square sqr = new(i, k, king);
                            sqr.piece = king;
                            board.Add(sqr);
                        }
                    }
                    // Second column (pawns)
                    else if (k == 1)
                    {
                        Pawn pawn = new(Piece.Colour.White);
                        Square sqr = new(i, k, pawn);
                        sqr.piece = pawn;
                        board.Add(sqr);
                    }
                    // Seventh column (pawns)
                    else if (k == 6)
                    {
                        Pawn pawn = new(Piece.Colour.Black);
                        Square sqr = new(i, k, pawn);
                        sqr.piece = pawn;
                        board.Add(sqr);
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
                            board.Add(sqr);
                        }
                        // Knight rows
                        else if (i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.Black);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            board.Add(sqr);
                        }
                        // Bishop rows
                        else if (i == 2 || i == 5)
                        {
                            Bishop bishop = new(Piece.Colour.Black);
                            Square sqr = new(i, k, bishop);
                            sqr.piece = bishop;
                            board.Add(sqr);
                        }
                        // Queen row
                        else if (i == 3)
                        {
                            Queen queen = new(Piece.Colour.Black);
                            Square sqr = new(i, k, queen);
                            sqr.piece = queen;
                            board.Add(sqr);
                        }
                        // King row
                        else if (i == 4)
                        {
                            King king = new(Piece.Colour.Black);
                            Square sqr = new(i, k, king);
                            sqr.piece = king;
                            board.Add(sqr);
                        }
                    }
                    // All other squares are empty and are added with a NULL piece
                    else
                    {
                        Square sqr = new(i, k, null);
                        board.Add(sqr);
                    }
                }
            }
        }

        /// <summary>
        /// Construct a view of the board and return it as a string to be printed
        /// </summary>
        /// <returns>A string representing the state of the board</returns>
        public string View()
        {
            string view = "";

            // Get the player whos turn it is
            string currentPlayer = (p1.MyTurn) ? p1.username : p2.username;
            
            int yAxis = rows;

            // Iterate over every square and print the piece name as well as "|" deviders between squares
            for (int row = 0; row < rows; row++)
            {
                view += yAxis.ToString();
                view += "|"; // Start of the row

                for (int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;

                    if (board[index].piece == null)
                    {
                        view += " |"; // Square border
                    }
                    else
                    {
                            view += $"{board[index].piece.name}"; // Piece
                            view += "|"; // Square border
                    }
                }

                view += "\n";
                yAxis--;
            }

            view += "  A B C D E F G H\n";

            view += $"\n{currentPlayer} your turn:";

            return view;
        }

        public void Turn(string tmp)
        {
            string turn = tmp.ToUpper();

            // A valid move command
            string movePattern = "^[A-H][1-8]TO[A-H][1-8]";
            Regex moveRegex = new Regex(movePattern, RegexOptions.IgnoreCase);

            if (moveRegex.IsMatch(turn))
            {
                Console.WriteLine("Move matches regex pattern");
            }
            else
            {
                Console.WriteLine("Move does not match regex pattern");
            }
        }
    }
}
