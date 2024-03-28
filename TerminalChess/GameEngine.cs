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
using static TerminalChess.ChessException;

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
        private Board board = new();
        private Utils utils = new();
        private List<string> p1BoardStates = new List<string>();
        private List<string> p2BoardStates = new List<string>();
        private int repetitions = 0;
        private int consecutiveMovesWithoutCapture = 0;
        private AI ai = new();

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
            board.AddPiecesToBoard();
        }

        public GameEngine()
        {
            TurnNo = 1;
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
                view += yAxis.ToString(); // Numbered axis
                view += "|"; // Start of the row

                for (int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;

                    if (board.GetSquareAtIndex(index).piece == null)
                    {
                        view += " |"; // Square border
                    }
                    else
                    {
                        view += $"{board.GetSquareAtIndex(index).piece.Name}"; // Piece
                        view += "|"; // Square border
                    }
                }

                string tmp = "";
                // Add captured pieces
                if (row == 0)
                {
                    foreach (string piece in p2.capturedPieces)
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

            view += "  A B C D E F G H\n"; // X axis

            return view;
        }

        /// <summary>
        /// In game options menu
        /// </summary>
        /// <returns>False if the game is ended. True if game continues</returns>
        private bool Options()
        {
            // Print menu and get response
            utils.Print(utils.optionsMenu);
            string optionsSelection = utils.GetMenuSelection(Utils.MENU_TYPES.OPTIONS);

            // Validate selection
            while (!optionsSelection.Equals("0") && !optionsSelection.Equals("1") && !optionsSelection.Equals("2") && !optionsSelection.Equals("3"))
            {
                utils.Print("Invalid response. Try again:");
                optionsSelection = utils.GetMenuSelection(Utils.MENU_TYPES.OPTIONS);
            }

            // Selection logic
            switch (optionsSelection)
            {
                // Stalemate
                case "0":
                    utils.Print("Are both player sure they want to end the game in a draw?(y/n)");
                    string response1 = utils.GetInput();

                    while (response1.ToUpper() != "Y" && response1.ToUpper() != "N")
                    {
                        utils.Print("Invalid response!");
                        response1 = utils.GetInput();
                    }

                    if (response1.ToUpper() == "Y")
                    {
                        p1.Winner = true;
                        p2.Winner = true;
                        return false;
                    }
                    break;
                // Concede
                case "1":
                    utils.Print("Are you sure you want to concede?(y/n)");
                    string response2 = utils.GetInput();

                    while (response2.ToUpper() != "Y" && response2.ToUpper() != "N")
                    {
                        utils.Print("Invalid response!");
                        response2 = utils.GetInput();
                    }

                    if (response2.ToUpper() == "Y")
                    {
                        OpponentPlayer.Winner = true;
                        return false;
                    }
                    break;
                // Back
                case "2":
                    return true;
                default:
                    utils.Print("Option not recognised!");
                    break;
            }
            return true;
        }

        /// <summary>
        /// Moves a piece based on the inputted move command.
        /// Checks the move input against a regex pattern.
        /// </summary>
        public void Turn()
        {
            // Take the turn input
            string turn = "";

            if (CurrentPlayer.IsAI)
            {
                ai.GenerateBestMove(this, board, CurrentPlayer.Colour, OpponentPlayer);
            }
            else
            {
                turn = Console.ReadLine();
                turn = turn.ToUpper();


                // Regex pattern to match
                string movePattern = "^[A-H][1-8]TO[A-H][1-8]";
                Regex moveRegex = new(movePattern, RegexOptions.IgnoreCase);

                // Move matches the regex
                if (!moveRegex.IsMatch(turn))
                {
                    if (turn == "O")
                    {
                        Options();
                        return;
                    }
                    else
                    {
                        throw new ChessException(CHESS_EXCEPTION_TYPE.INVALID_MOVE, CurrentPlayer.IsAI);
                    }
                }

                // Check if the user is intending to castle this turn
                CheckCastling(turn);

                // Validate the turn string against the game rules
                ValidateTurn(turn);
            }

            castling = Castling.NOT_CASTLING;


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
        }

        /// <summary>
        /// Takes a valid turn string and checks if it is a legal chess move
        /// </summary>
        /// <param name="turn"></param>
        public void ValidateTurn(string turn)
        {
            bool pieceCaptured = false;

            // Extract the origin square coordinates from the move command
            int moveFromRow = int.Parse(turn[1].ToString()) - 1;
            int movefromCol = ParseCoordinates(turn[0]);

            // Extract the destination square coordinates from the move command
            int moveToRow = int.Parse(turn[5].ToString()) - 1;
            int moveToCol = ParseCoordinates(turn[4]);

            // Get the starting square
            Square curSquare = board.GetSquareAtPos(moveFromRow, movefromCol);

            //Check the player has selected a square that contains a piece
            if (curSquare.piece == null)
            {
                throw new ChessException(CHESS_EXCEPTION_TYPE.NO_PIECE, CurrentPlayer.IsAI);
            }

            // Check the player has selected the correct colour piece
            if (CurrentPlayer.Colour != curSquare.piece.colour)
            {
               throw new ChessException(CHESS_EXCEPTION_TYPE.OPPONENTS_PIECE, CurrentPlayer.IsAI); 
            }

            // Check if the proposed move is found in the list of possible moves
            List<(int, int)> validMoves = curSquare.piece.GetPossibleMoves(moveFromRow, movefromCol, board);

            if (!IsMoveValid(validMoves, moveToRow, moveToCol))
            {
                throw new ChessException(CHESS_EXCEPTION_TYPE.INVALID_MOVE, CurrentPlayer.IsAI);
            }

            // Get the destination square
            Square newSquare = board.GetSquareAtPos(moveToRow, moveToCol);

            // Store the state of the salient pieces incase the move is determined invalid and the board state needs resetting
            Square backupCurSquare = new(curSquare.row, curSquare.col, curSquare.piece);
            Square backupNewSquare = new(newSquare.row, newSquare.col, newSquare.piece);
            backupCurSquare.piece.HasMoved = curSquare.piece.HasMoved;

            if (newSquare.piece != null)
            {
                if (newSquare.piece.colour == CurrentPlayer.Colour)
                {
                    // Reset the board state
                    UndoMove(newSquare, curSquare, backupNewSquare, backupCurSquare, pieceCaptured);
                    throw new ChessException(CHESS_EXCEPTION_TYPE.INVALID_MOVE, CurrentPlayer.IsAI);
                }

                backupNewSquare.piece.HasMoved = newSquare.piece.HasMoved;
            }

            // Check if player is using en passent
            if (curSquare.piece is Pawn pawn && movefromCol != moveToCol)
            {
                if (newSquare.piece == null)
                {
                    pieceCaptured = true;
                    Square s = board.GetSquareAtPos(pawn.EPCapture.Item1, pawn.EPCapture.Item2);
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
            if (castling != Castling.NOT_CASTLING)
            {
                DoCastle(curSquare);
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

                throw new ChessException(CHESS_EXCEPTION_TYPE.END_IN_CHECK, CurrentPlayer.IsAI);
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
                        board.GetSquareAtPos(0, 5).piece = board.GetSquareAtPos(0, 7).piece;
                        board.GetSquareAtPos(0, 7).piece = null;
                        break;

                    case Castling.W_QUEEN_SIDE:
                        board.GetSquareAtPos(0, 3).piece = board.GetSquareAtPos(0, 0).piece;
                        board.GetSquareAtPos(0, 0).piece = null;
                        break;

                    case Castling.B_KING_SIDE:
                        board.GetSquareAtPos(7, 5).piece = board.GetSquareAtPos(7, 7).piece;
                        board.GetSquareAtPos(7, 7).piece = null;
                        break;

                    case Castling.B_QUEEN_SIDE:
                        board.GetSquareAtPos(7, 3).piece = board.GetSquareAtPos(7, 0).piece;
                        board.GetSquareAtPos(7, 0).piece = null;
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
        }

        /// <summary>
        /// Roll back a move
        /// </summary>
        /// <param name="newSquare"></param>
        /// <param name="curSquare"></param>
        /// <param name="backupNewSquare"></param>
        /// <param name="backupCurSquare"></param>
        /// <param name="pieceCaptured"></param>
        public void UndoMove(Square newSquare, Square curSquare, Square backupNewSquare, Square backupCurSquare, bool pieceCaptured)
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
        public void CheckCastling(string turn)
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
        private void DoCastle(Square curSquare)
        {
            // Check that the player is not trying to castle through check
            switch (castling)
            {
                case Castling.W_KING_SIDE:
                    Square f1 = board.GetSquareAtPos(0, 5);
                    f1.piece = curSquare.piece;
                    curSquare.piece = null;

                    if (IsPlayerInCheck(CurrentPlayer))
                    {
                        curSquare.piece = f1.piece;
                        f1.piece = null;

                        throw new ChessException(CHESS_EXCEPTION_TYPE.CASTLE_THROUGH_CHECK, CurrentPlayer.IsAI);
                    }

                    curSquare.piece = f1.piece;
                    f1.piece = null;
                    break;

                case Castling.W_QUEEN_SIDE:
                    for (int i = 1; i < 3; i++)
                    {
                        Square moveThroughSquare = board.GetSquareAtPos(0, curSquare.col - i);
                        moveThroughSquare.piece = curSquare.piece;
                        curSquare.piece = null;

                        if (IsPlayerInCheck(CurrentPlayer))
                        {
                            curSquare.piece = moveThroughSquare.piece;
                            moveThroughSquare.piece = null;
                            throw new ChessException(CHESS_EXCEPTION_TYPE.CASTLE_THROUGH_CHECK, CurrentPlayer.IsAI);
                        }

                        curSquare.piece = moveThroughSquare.piece;
                        moveThroughSquare.piece = null;
                    }
                    break;

                case Castling.B_KING_SIDE:
                    Square f8 = board.GetSquareAtPos(7, 5);
                    f8.piece = curSquare.piece;
                    curSquare.piece = null;

                    if (IsPlayerInCheck(CurrentPlayer))
                    {
                        curSquare.piece = f8.piece;
                        f8.piece = null;
                        throw new ChessException(CHESS_EXCEPTION_TYPE.CASTLE_THROUGH_CHECK, CurrentPlayer.IsAI);
                    }

                    curSquare.piece = f8.piece;
                    f8.piece = null;
                    break;

                case Castling.B_QUEEN_SIDE:
                    for (int i = 1; i < 3; i++)
                    {
                        Square moveThroughSquare = board.GetSquareAtPos(7, curSquare.col - i);
                        moveThroughSquare.piece = curSquare.piece;
                        curSquare.piece = null;

                        if (IsPlayerInCheck(CurrentPlayer))
                        {
                            curSquare.piece = moveThroughSquare.piece;
                            moveThroughSquare.piece = null;
                            throw new ChessException(CHESS_EXCEPTION_TYPE.CASTLE_THROUGH_CHECK, CurrentPlayer.IsAI);
                        }

                        curSquare.piece = moveThroughSquare.piece;
                        moveThroughSquare.piece = null;
                    }
                    break;
            }
        }

        /// <summary>
        /// Promote a pawn to a new piece
        /// </summary>
        /// <param name="square"></param>
        /// <param name="promotionSelection"></param>
        private void DoPromotion(Square square, string promotionSelection)
        {
            switch (promotionSelection)
            {
                case "0":
                    Knight knight = new(CurrentPlayer.Colour);
                    square.piece = knight;
                    break;
                case "1":
                    Bishop bishop = new(CurrentPlayer.Colour);
                    square.piece = bishop;
                    break;
                case "2":
                    Rook rook = new(CurrentPlayer.Colour);
                    square.piece = rook;
                    break;
                case "3":
                    Queen queen = new(CurrentPlayer.Colour);
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
        private bool IsMoveValid(List<(int, int)> validMoves, int moveToRow, int moveToCol)
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
        /// Returns true or false depending on if the player is currently in check or not
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsPlayerInCheck(Player p)
        {
            // Check every enemy piece to see if they are currently attacking the king
            var enemySquares = board.GetAllSquaresWithPiecesOfOneColour(OpponentPlayer.Colour);

            foreach (Square s in enemySquares)
            {
                foreach (var entry in s.piece.GetPossibleMoves(s.row, s.col, board))
                {
                    int validRow = entry.Item1;
                    int validCol = entry.Item2;

                    Square tmpSquare = board.GetSquareAtPos(validRow, validCol);

                    if (tmpSquare.piece != null)
                    {
                        if (tmpSquare.piece is King)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsOpponentCheckmated()
        {
            // Check if the opponent player is in check
            if (!IsPlayerInCheck(OpponentPlayer))
            {
                // If the opponent is not in check, they cannot be checkmated
                return false;
            }

            // Find the opponents king 
            Square kingSquare = board.GetSquareByPiece<King>(OpponentPlayer.Colour);

            // Obtain valid moves for the king
            List<(int, int)> validMoves = kingSquare.piece.GetPossibleMoves(kingSquare.row, kingSquare.col, board);

            // Check each valid move
            foreach (var move in validMoves)
            {
                int moveToRow = move.Item1;
                int moveToCol = move.Item2;

                // Simulate the move
                Square backupCurSquare = new Square(kingSquare.row, kingSquare.col, kingSquare.piece);
                Square newSquare = board.GetSquareAtPos(moveToRow, moveToCol);
                Square backupNewSquare = new Square(moveToRow, moveToCol, newSquare?.piece);

                // Update the board state
                newSquare.piece = kingSquare.piece;
                kingSquare.piece = null;

                // Check if the opponent is still in check after the move
                bool isStillInCheck = IsPlayerInCheck(OpponentPlayer);

                // Undo the move
                UndoMove(newSquare, kingSquare, backupNewSquare, backupCurSquare, false);

                // If the opponent is not in check after the move, they are not checkmated
                if (!isStillInCheck)
                {
                    return false;
                }
            }

            // Opponent has been checkmated
            return true;
        }

        /// <summary>
        /// Check if requirements are met for a stalemate
        /// </summary>
        /// <returns>True if stalemate. False if not</returns>
        public bool CheckStalemate()
        {
            // Repetition & consecutive moves without capture
            if (repetitions >= 3 || consecutiveMovesWithoutCapture >= 100)
            {
                Console.WriteLine($"repetitions {repetitions}, consecutive moves without capture {consecutiveMovesWithoutCapture}");
                return true;
            }

            // Dead position
            var remainingPieces = board.GetRemainingPieces();

            // If the position is King v King or King + Bishop/Knight v King the game is drawn
            if (remainingPieces.Count() == 2)
            {
                Console.WriteLine("Dead position");
                return true;
            }
            else if (remainingPieces.Count() == 3)
            {
                foreach (var piece in remainingPieces)
                {
                    if (piece is Knight || piece is Bishop)
                    {
                        Console.WriteLine("Dead position");
                        return true;
                    }
                }
            }

            // Find the opponents king 
            Square kingSquare = board.GetSquareByPiece<King>(OpponentPlayer.Colour);

            // Obtain valid moves for the piece
            List<(int, int)> validMoves = kingSquare.piece.GetPossibleMoves(kingSquare.row, kingSquare.col, board);

            // Check each valid move
            foreach (var move in validMoves)
            {
                int moveToRow = move.Item1;
                int moveToCol = move.Item2;

                // Simulate the move
                Square backupCurSquare = new Square(kingSquare.row, kingSquare.col, kingSquare.piece);
                Square newSquare = board.GetSquareAtPos(moveToRow, moveToCol);
                Square backupNewSquare = new Square(moveToRow, moveToCol, newSquare?.piece);

                // Update the board state
                newSquare.piece = kingSquare.piece;
                kingSquare.piece = null;

                // Check if the opponent is in check after the move
                bool isInCheck = IsPlayerInCheck(OpponentPlayer);

                // Undo the move
                UndoMove(newSquare, kingSquare, backupNewSquare, backupCurSquare, false);

                // If the opponent is not in check after the move, no stalemate
                if (!isInCheck)
                {
                    return false;
                }
            }

            // Check if there are other pieces that can move
            foreach (var square in board.GetAllSquaresWithPiecesOfOneColour(CurrentPlayer.Colour))
            {
                if (!(square.piece is King) && square.piece.GetPossibleMoves(square.row, square.col, board).Count() > 0)
                {
                    return false;
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
