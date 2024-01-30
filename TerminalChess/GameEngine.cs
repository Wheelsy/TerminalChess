using Pastel;
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
        public Player currentPlayer { get; set; }
        public int TurnNo { get; set; }

        /// <summary>
        /// Constructor for a new game
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public GameEngine(Player p1, Player p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            numPieces = 32;
            TurnNo = 1;
            currentPlayer = p1;

            AddPiecesToBoard();
        }

        public GameEngine()
        {
            numPieces = 32;
            TurnNo = 1;
        }

        /// <summary>
        /// Add all pieces in their starting position to the board
        /// </summary>
        public void AddPiecesToBoard()
        {
            // Iterate through column
            for (int k = cols - 1; k >= 0; k--)
            {
                // Iterate through rows
                for (int i = 0; i < rows; i++)
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
                            board.Add(sqr);
                        }
                        // Knight rows
                        else if (i == 1 || i == 6)
                        {
                            Knight knight = new(Piece.Colour.White);
                            Square sqr = new(i, k, knight);
                            sqr.piece = knight;
                            board.Add(sqr);
                        }
                        // Bishop rows
                        else if (i == 2 || i == 5)
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
                        view += $"{board[index].piece.Name}"; // Piece
                        view += "|"; // Square border
                    }
                }

                if (row == 0)
                {
                    view += $" {p2.username} ";
                }
                else if (row == 7)
                {
                    view += $" {p1.username} ";
                }

                view += "\n";
                yAxis--;
            }

            view += "  A B C D E F G H\n";

            view += $"\n{currentPlayer.username} your turn:";

            return view;
        }

        /// <summary>
        /// Moves a piece based on the inputted move command.
        /// Checks the move input against a regex pattern.
        /// </summary>
        public void Turn()
        {
            bool validCommand = false;

            while (!validCommand)
            {
                // Take the turn input
                string turn = Console.ReadLine();
                turn = turn.ToUpper();

                // Regex pattern to match
                string movePattern = "^[A-H][1-8]TO[A-H][1-8]";
                Regex moveRegex = new(movePattern, RegexOptions.IgnoreCase);

                // Move matches the regex
                if (moveRegex.IsMatch(turn))
                {
                    // Check if the move is legal
                    validCommand = ValidateTurn(turn);
                }

                if (!validCommand)
                {
                    Console.WriteLine("Invalid command. Try again:");
                }
            }

            if(currentPlayer == p2)
            {
                TurnNo++;
            }

            currentPlayer = (currentPlayer == p1) ? p2 : p1;
            return;
        }

        /// <summary>
        /// Takes a valid turn string and checks if it is a legal chess move
        /// </summary>
        /// <param name="turn"></param>
        private bool ValidateTurn(string turn)
        {
            // Extract the origin square coordinates from the move command
            int moveFromRow = int.Parse(turn[1].ToString()) - 1;
            int movefromCol = -1;

            switch (turn[0])
            {
                case 'A':
                    movefromCol = 0;
                    break;
                case 'B':
                    movefromCol = 1;
                    break;
                case 'C':
                    movefromCol = 2;
                    break;
                case 'D':
                    movefromCol = 3;
                    break;
                case 'E':
                    movefromCol = 4;
                    break;
                case 'F':
                    movefromCol = 5;
                    break;
                case 'G':
                    movefromCol = 6;
                    break;
                case 'H':
                    movefromCol = 7;
                    break;
            }

            // Extract the destination square coordinates from the move command
            int moveToRow = int.Parse(turn[5].ToString()) - 1;
            int moveToCol = -1;

            switch (turn[4])
            {
                case 'A':
                    moveToCol = 0;
                    break;
                case 'B':
                    moveToCol = 1;
                    break;
                case 'C':
                    moveToCol = 2;
                    break;
                case 'D':
                    moveToCol = 3;
                    break;
                case 'E':
                    moveToCol = 4;
                    break;
                case 'F':
                    moveToCol = 5;
                    break;
                case 'G':
                    moveToCol = 6;
                    break;
                case 'H':
                    moveToCol = 7;
                    break;
            }

            // Get the starting square
            Square curSquare = GetSquareAtPos(moveFromRow, movefromCol);

            // Check the player has selected the correct colour piece
            if (currentPlayer == p1)
            {
                if (curSquare.piece.colour != Piece.Colour.White)
                {
                    Console.WriteLine("That is your opponents piece!");
                    return false;
                }
            }
            else
            {
                if (curSquare.piece.colour != Piece.Colour.Black)
                {
                    Console.WriteLine("That is your opponents piece!");
                    return false;
                }
            }

            // Loop through all possible moves and see if the users input is among them
            bool moveIsPossible = false;

            foreach (var entry in curSquare.piece.GetPossibleMoves(moveFromRow, movefromCol, this))
            {
                int validRow = entry.Item1;
                int validCol = entry.Item2;

                if (validRow == moveToRow && validCol == moveToCol)
                {
                    moveIsPossible = true;
                }
            }

            if (!moveIsPossible)
            {
                Console.WriteLine("Move not found in possible moves");
                return false;
            }

            // Get the destination square
            Square newSquare = GetSquareAtPos(moveToRow, moveToCol);

            // If a piece was captured update the players score
            if (newSquare.piece != null)
            {
                currentPlayer.Score += newSquare.piece.Value;
            }

            // Update the moved pieces position
            newSquare.piece = curSquare.piece;

            // Remove the moved piece from the origin square
            curSquare.piece = null;

            return true;
        }

        /// <summary>
        /// Find the square at a given coordinate
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Square GetSquareAtPos(int row, int col)
        {
            foreach (Square s in board)
            {
                if (s.col == col && s.row == row)
                {
                    return s;
                }
            }

            return null;
        }
    }
}
