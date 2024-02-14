using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static TerminalChess.Piece;

namespace TerminalChess
{
    internal class GameEngine
    {
        enum Castling
        {
            B_KING_SIDE,
            B_QUEEN_SIDE,
            W_KING_SIDE,
            W_QUEEN_SIDE,
            NOT_CASTLING
        }

        private Castling castling;
        private int cols = 8;
        private int rows = 8;
        private List<Square> board = new();
        private Utils utils;
        private List<string> p1BoardStates = new List<string>();
        private List<string> p2BoardStates = new List<string>();
        private int repetitions = 0;
        private int consecutiveMovesWithoutCapture = 0;

        public Player p1 { get; }
        public Player p2 { get; }
        public Player CurrentPlayer { get; set; }
        public Player OpponentPlayer { get; set; }

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
            TurnNo = 1;
            CurrentPlayer = p1;
            OpponentPlayer = p2;
            utils = new();

            AddPiecesToBoard();
        }

        public GameEngine()
        {
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

                string tmp = "";
                if (row == 0)
                {
                    foreach(string piece in p2.capturedPieces)
                    {
                        tmp += $"{piece} ".Pastel(Color.SandyBrown);
                    }

                    view += $" {p2.username}: {tmp}";
                }
                else if (row == 7)
                {
                    foreach (string piece in p1.capturedPieces)
                    {
                        tmp += $"{piece} ".Pastel(Color.Chocolate);
                    }
                    view += $" {p1.username}: {tmp}";
                }

                view += "\n";
                yAxis--;
            }

            view += "  A B C D E F G H\n";

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
                    // Check if the user is intending to castle this turn
                    CheckCastling(turn);

                    // Validate the turn string against the game rules
                    validCommand = ValidateTurn(turn);

                    castling = Castling.NOT_CASTLING;
                }

                if (!validCommand)
                {
                    Console.WriteLine("Invalid command. Try again:");
                }
            }

            // Check for checkmate
            if (IsOpponentCheckmated())
            {
                CurrentPlayer.Winner = true;
                return;
            }

            // Update board states for each player
            UpdateBoardStates();

            // Check for repetition stalemate
            if (CurrentPlayer == p1 && p1BoardStates.Count == 3)
            {
                if (p1BoardStates[0] == p1BoardStates[2])
                {
                    repetitions++;
                }
            }
            else if (CurrentPlayer == p2 && p2BoardStates.Count == 3)
            {
                if (p2BoardStates[0] == p2BoardStates[2])
                {
                    repetitions++;
                }
            }

            // Check for stalemate
            if (CheckStalemate())
            {
                p1.Winner = true;
                p2.Winner = true;
                return;
            }

            // Update the current player
            CurrentPlayer = (CurrentPlayer == p1) ? p2 : p1;
            OpponentPlayer = (CurrentPlayer == p1) ? p2 : p1;

            TurnNo++;
            return;
        }

        /// <summary>
        /// Takes a valid turn string and checks if it is a legal chess move
        /// </summary>
        /// <param name="turn"></param>
        private bool ValidateTurn(string turn)
        {
            bool pieceCaptured = false;

            // Extract the origin square coordinates from the move command
            int moveFromRow = int.Parse(turn[1].ToString()) - 1;
            int movefromCol = ParseCoordinates(turn[0]);

            // Extract the destination square coordinates from the move command
            int moveToRow = int.Parse(turn[5].ToString()) - 1;
            int moveToCol = ParseCoordinates(turn[4]);

            // Get the starting square
            Square curSquare = GetSquareAtPos(moveFromRow, movefromCol);

            //Check the player has selected a square that contains a piece
            if(curSquare.piece == null){
                Console.WriteLine("There is no piece in the selected square.");
                return false;
            }

            // Check the player has selected the correct colour piece
            if (CurrentPlayer == p1)
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

            // Check if the proposed move is found in the list of possible moves
            List<(int, int)> validMoves = curSquare.piece.GetPossibleMoves(moveFromRow, movefromCol, this);

            if (!IsMoveValid(validMoves, moveToRow, moveToCol))
            {
                Console.WriteLine("Move not found in possible moves");
                return false;
            }

            // Get the destination square
            Square newSquare = GetSquareAtPos(moveToRow, moveToCol);

            // Store the state of the salient pieces incase the move is determined invalid and the board state needs resetting
            Square backupCurSquare = new(curSquare.row, curSquare.col, curSquare.piece);
            Square backupNewSquare = new(newSquare.row, newSquare.col, newSquare.piece);
            backupCurSquare.piece.HasMoved = curSquare.piece.HasMoved;

            if (newSquare.piece != null)
            {
                backupNewSquare.piece.HasMoved = newSquare.piece.HasMoved;
            }

            // Check if player is using en passent
            if (curSquare.piece is Pawn pawn && movefromCol != moveToCol)
            {
                if (newSquare.piece == null)
                {
                    pieceCaptured = true;
                    Square s = GetSquareAtPos(pawn.EPCapture.Item1, pawn.EPCapture.Item2);
                    CurrentPlayer.capturedPieces.Add(s.piece.Name);
                    CurrentPlayer.Score += pawn.Value;
                    s.piece = null;
                }
            }

            // If a piece was captured update the players score
            if (newSquare.piece != null)
            {
                pieceCaptured = true;
                CurrentPlayer.capturedPieces.Add(newSquare.piece.Name);
                CurrentPlayer.Score += newSquare.piece.Value;
            }

            // Validate castling
            if(castling != Castling.NOT_CASTLING)
            {
                if (!DoCastle(curSquare))
                {
                    return false;
                }
            }

            // Update the moved pieces position
            newSquare.piece = curSquare.piece;

            // Remove the moved piece from the origin square
            curSquare.piece = null;

            // Check if a pawn is being promoted
            if (newSquare.piece is Pawn && (moveToRow == 7 || moveToRow == 0))
            {
                utils.Print(utils.promotion);
                string promotionSelection = utils.GetMenuSelection(Utils.MENU_TYPES.PROMOTION);
                DoPromotion(newSquare, promotionSelection);
            }

            // See if the current player has ended their turn in check
            if (IsPlayerInCheck(CurrentPlayer))
            {
                // Reset the board state
                UndoMove(newSquare, curSquare, backupNewSquare, backupCurSquare, pieceCaptured);

                Console.WriteLine("You cannot end your turn in check");
                return false;
            }

            // Update double move bool for purposes of en passent
            if (newSquare.piece is Pawn pawn1 && (moveFromRow - moveToRow == 2 || moveFromRow - moveToRow == -2))
            {
                pawn1.LastMoveWasDouble = true;
            }
            else if (newSquare.piece is Pawn pawn2)
            {
                pawn2.LastMoveWasDouble = false;
            }

            if (castling != Castling.NOT_CASTLING)
            {
                // If castling move the rook
                switch (castling)
                {
                    case Castling.W_KING_SIDE:
                        GetSquareAtPos(0, 5).piece = GetSquareAtPos(0, 7).piece;
                        GetSquareAtPos(0, 7).piece = null;
                        break;

                    case Castling.W_QUEEN_SIDE:
                        GetSquareAtPos(0, 3).piece = GetSquareAtPos(0, 0).piece;
                        GetSquareAtPos(0, 0).piece = null;
                        break;

                    case Castling.B_KING_SIDE:
                        GetSquareAtPos(7, 5).piece = GetSquareAtPos(7, 7).piece;
                        GetSquareAtPos(7, 7).piece = null;
                        break;

                    case Castling.B_QUEEN_SIDE:
                        GetSquareAtPos(7, 3).piece = GetSquareAtPos(7, 0).piece;
                        GetSquareAtPos(7, 0).piece = null;
                        break;
                }
            }

            // Mark the piece as having moved for the purpose of castling
            if (!newSquare.piece.HasMoved)
            {
                newSquare.piece.HasMoved = true;
            }

            //Update the consecutive move stalemate counter
            if (pieceCaptured)
            {
                consecutiveMovesWithoutCapture = 0;
            }
            else
            {
                consecutiveMovesWithoutCapture++;
            }

            return true;
        }

        private void UndoMove(Square newSquare, Square curSquare, Square backupNewSquare, Square backupCurSquare, bool pieceCaptured)
        {
            // Restore the state of the moved pieces
            curSquare.piece = backupCurSquare.piece;
            newSquare.piece = backupNewSquare.piece;

            // Restore the 'HasMoved' flag if applicable
            if (curSquare.piece != null)
            {
                curSquare.piece.HasMoved = backupCurSquare.piece.HasMoved;
            }

            // Restore the captured piece if applicable
            if (pieceCaptured)
            {
                // Remove the last captured piece from the player's captured pieces list
                if (CurrentPlayer.capturedPieces.Count > 0)
                {
                    CurrentPlayer.capturedPieces.RemoveAt(CurrentPlayer.capturedPieces.Count - 1);
                }

                // Deduct the value of the captured piece from the player's score
                if (backupNewSquare.piece != null)
                {
                    CurrentPlayer.Score -= backupNewSquare.piece.Value;
                }
            }
        }

        /// <summary>
        /// Assigns a castling variable that lets the validation method
        /// know the players intention.
        /// </summary>
        /// <param name="turn"></param>
        private void CheckCastling(string turn)
        {
            switch (turn)
            {
                case "E1TOG1":
                    castling = Castling.W_KING_SIDE;
                    break;
                case "E1TOC1":
                    castling = Castling.W_QUEEN_SIDE;
                    break;
                case "E8TOG8":
                    castling = Castling.B_KING_SIDE;
                    break;
                case "E8TOC8":
                    castling = Castling.B_QUEEN_SIDE;
                    break;
                default:
                    castling = Castling.NOT_CASTLING;
                    break;
            }
        }

        /// <summary>
        /// Loop through all the individual moved involved in castling
        /// If castling moves through or ends in check return false
        /// Else returns true
        /// </summary>
        /// <param name="curSquare"></param>
        /// <returns></returns>
        private bool DoCastle(Square curSquare)
        {
            // Check that the player is not trying to castle through check
            switch (castling)
            {
                case Castling.W_KING_SIDE:
                    Square f1 = GetSquareAtPos(0, 5);
                    f1.piece = curSquare.piece;
                    curSquare.piece = null;

                    if (IsPlayerInCheck(CurrentPlayer))
                    {
                        curSquare.piece = f1.piece;
                        f1.piece = null;

                        Console.WriteLine("You cannot castle through check");
                        return false;
                    }

                    curSquare.piece = f1.piece;
                    f1.piece = null;
                    break;

                case Castling.W_QUEEN_SIDE:
                    for (int i = 1; i < 3; i++)
                    {
                        Square moveThroughSquare = GetSquareAtPos(0, curSquare.col - i);
                        moveThroughSquare.piece = curSquare.piece;
                        curSquare.piece = null;

                        if (IsPlayerInCheck(CurrentPlayer))
                        {
                            curSquare.piece = moveThroughSquare.piece;
                            moveThroughSquare.piece = null;
                            Console.WriteLine("You cannot castle through check");
                            return false;
                        }

                        curSquare.piece = moveThroughSquare.piece;
                        moveThroughSquare.piece = null;
                    }
                    break;

                case Castling.B_KING_SIDE:
                    Square f8 = GetSquareAtPos(7, 5);
                    f8.piece = curSquare.piece;
                    curSquare.piece = null;
                        
                    if (IsPlayerInCheck(CurrentPlayer))
                    {
                        curSquare.piece = f8.piece;
                        f8.piece = null;

                        Console.WriteLine("You cannot castle through check");
                        return false;
                    }

                    curSquare.piece = f8.piece;
                    f8.piece = null;
                    break;

                case Castling.B_QUEEN_SIDE:
                    for (int i = 1; i < 3; i++)
                    {
                        Square moveThroughSquare = GetSquareAtPos(7, curSquare.col - i);
                        moveThroughSquare.piece = curSquare.piece;
                        curSquare.piece = null;

                        if (IsPlayerInCheck(CurrentPlayer))
                        {
                            curSquare.piece = moveThroughSquare.piece;
                            moveThroughSquare.piece = null;
                            Console.WriteLine("You cannot castle through check");
                            return false;
                        }

                        curSquare.piece = moveThroughSquare.piece;
                        moveThroughSquare.piece = null;
                    }
                    break;
            }

            return true;
        }

        private void DoPromotion(Square square, string promotionSelection)
        {
            Colour colour = (CurrentPlayer == p1) ? Piece.Colour.White : Piece.Colour.Black;

            switch (promotionSelection)
            {
                case "0":
                    Knight knight = new(colour);
                    square.piece = knight;
                    break;
                case "1":
                    Bishop bishop = new(colour);
                    square.piece = bishop;
                    break;
                case "2":
                    Rook rook = new(colour);
                    square.piece = rook;
                    break;
                case "3":
                    Queen queen = new(colour);
                    square.piece = queen;
                    break;
            }
        }

        /// <summary>
        /// Loop through a list of possible move coordinates and return true
        /// If the proposed move is among them. Else returns false
        /// </summary>
        /// <param name="validMoves"></param>
        /// <param name="moveToRow"></param>
        /// <param name="moveToCol"></param>
        /// <returns></returns>
        private bool IsMoveValid(List<(int,int)>validMoves, int moveToRow, int moveToCol)
        {
            foreach (var entry in validMoves)
            {
                int validRow = entry.Item1;
                int validCol = entry.Item2;

                if (validRow == moveToRow && validCol == moveToCol)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Convert a char column coordinate into an integer
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private int ParseCoordinates(char column)
        {
            int tmp;

            switch (column)
            {
                case 'A':
                    tmp = 0;
                    break;
                case 'B':
                    tmp = 1;
                    break;
                case 'C':
                    tmp = 2;
                    break;
                case 'D':
                    tmp = 3;
                    break;
                case 'E':
                    tmp = 4;
                    break;
                case 'F':
                    tmp = 5;
                    break;
                case 'G':
                    tmp = 6;
                    break;
                case 'H':
                    tmp = 7;
                    break;
                default:
                    tmp = -1;
                    break;
            }

            return tmp;
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

        /// <summary>
        /// Returns true or false depending on if the player is currently in check or not
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsPlayerInCheck(Player p)
        {
            var colour = (p == p1) ? Piece.Colour.White : Piece.Colour.Black;

            // Loop through the board and check every enemy piece to see if they are currently attacking the king
            foreach (Square s in board)
                {
                    if (s.piece != null)
                    {
                        if (s.piece.colour != colour)
                        {
                            foreach (var entry in s.piece.GetPossibleMoves(s.row, s.col, this))
                            {
                                int validRow = entry.Item1;
                                int validCol = entry.Item2;

                                Square tmpSquare = GetSquareAtPos(validRow, validCol);

                                if (tmpSquare.piece != null)
                                {
                                    if (tmpSquare.piece.Name.Contains("K"))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            return false;
        }

        public bool IsOpponentCheckmated()
        {
            var colour = (OpponentPlayer == p1) ? Piece.Colour.White : Piece.Colour.Black;

            // Check if the opponent player is in check
            if (!IsPlayerInCheck(OpponentPlayer))
            {
                // If the opponent is not in check, they cannot be checkmated
                return false;
            }

            // Find the opponents king 
            foreach (Square square in board)
            {
                if (square.piece != null && square.piece.colour == colour && square.piece is King)
                {
                    // Obtain valid moves for the king
                    List<(int, int)> validMoves = square.piece.GetPossibleMoves(square.row, square.col, this);

                    // Check each valid move
                    foreach (var move in validMoves)
                    {
                        int moveToRow = move.Item1;
                        int moveToCol = move.Item2;

                        // Simulate the move
                        Square backupCurSquare = new Square(square.row, square.col, square.piece);
                        Square newSquare = GetSquareAtPos(moveToRow, moveToCol);
                        Square backupNewSquare = new Square(moveToRow, moveToCol, newSquare?.piece);

                        // Update the board state
                        newSquare.piece = square.piece;
                        square.piece = null;

                        // Check if the opponent is still in check after the move
                        bool isStillInCheck = IsPlayerInCheck(OpponentPlayer);

                        // Undo the move
                        UndoMove(newSquare, square, backupNewSquare, backupCurSquare, false);

                        // If the opponent is not in check after the move, they are not checkmated
                        if (!isStillInCheck)
                        {
                            return false;
                        }
                    }
                }
            }
            
            // Opponent has been checkmated
            return true;
        }

        /// <summary>
        /// Check if requirements are met for a stalemate
        /// </summary>
        /// <returns>True if stalemate. False if not</returns>
        private bool CheckStalemate()
        {

            if (repetitions >= 3 || consecutiveMovesWithoutCapture >= 100)
            {
                return true;
            }

            var colour = (CurrentPlayer == p1) ? Piece.Colour.Black : Piece.Colour.White;

            // Find the opponents king 
            foreach (Square square in board)
            {
                if (square.piece != null && square.piece.colour == colour )
                {
                    // Obtain valid moves for the piece
                    List<(int, int)> validMoves = square.piece.GetPossibleMoves(square.row, square.col, this);

                    // Check each valid move
                    foreach (var move in validMoves)
                    {
                        int moveToRow = move.Item1;
                        int moveToCol = move.Item2;

                        // Simulate the move
                        Square backupCurSquare = new Square(square.row, square.col, square.piece);
                        Square newSquare = GetSquareAtPos(moveToRow, moveToCol);
                        Square backupNewSquare = new Square(moveToRow, moveToCol, newSquare?.piece);

                        // Update the board state
                        newSquare.piece = square.piece;
                        square.piece = null;

                        // Check if the opponent is in check after the move
                        bool isInCheck = IsPlayerInCheck(OpponentPlayer);

                        // Undo the move
                        UndoMove(newSquare, square, backupNewSquare, backupCurSquare, false);

                        // If the opponent is not in check after the move, no stalemate
                        if (!isInCheck)
                        {
                            return false;
                        }
                    }
                }
            }

            // If no valid move could be found the game is a stalemate
            return true;
        }

        /// <summary>
        /// Update boards state for each player
        /// </summary>
        private void UpdateBoardStates()
        {
            // Store the current board state for each player
            if (CurrentPlayer == p1)
            {
                p1BoardStates.Add(View());
                // Ensure we only keep track of the last 3 states
                if (p1BoardStates.Count > 3)
                    p1BoardStates.RemoveAt(0);
            }
            else
            {
                p2BoardStates.Add(View());
                if (p2BoardStates.Count > 3)
                    p2BoardStates.RemoveAt(0);
            }
        }
    }
}
